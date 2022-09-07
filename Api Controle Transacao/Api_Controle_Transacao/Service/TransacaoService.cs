using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Api_Controle_Transacao.Helper;
using Api_Controle_Transacao.Helper.Interface;
using Api_Controle_Transacao.Models;
using Api_Controle_Transacao.Service.Interface;
using Confluent.Kafka;
using Extensions;

public class TrasacaoService : ITrasacaoService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    private readonly IContaClienteConector _contacliente;
    private readonly IHashMaker _hash;
    private readonly ITrasacaoRepository _transrepo;
    public TrasacaoService(IWebHostEnvironment environment, ISplunkLogger splunk, IContaClienteConector contacliente, IHashMaker hash, ITrasacaoRepository transrepo)
    {
        _environment = environment;
        _splunk = splunk;
        _contacliente = contacliente;
        _hash = hash;
        _transrepo = transrepo;
    }

    public async Task<dynamic> ProcessarTransacao(TransacaoInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        TransacaoOutputPostDTO output = new TransacaoOutputPostDTO();
        output.Valor_Transacao = input.Valor_Transacao;

        var contahashkey = _hash.HashString(input.Numero_Conta_Origem + input.Numero_Agencia_Origem + input.Numero_Digito_Origem);

        var cacheresp = await _transrepo.ConsultarContaCache(contahashkey);
        var saldo = (cacheresp.HasValue == false) ? null : JsonSerializer.Deserialize<ContaClienteSaldoDTO>(cacheresp);

        if (saldo == null)
        {
            _splunk.LogarMensagem("Saldo não encontrado em cache");
            _splunk.LogarMensagem("Consultando saldo conta origem :" + input.Numero_Conta_Origem);
            saldo = _contacliente.ConsultarSaldoConta(input);
            _splunk.LogarMensagem("Saldo conta origem consultado:" + saldo.saldo);
        }
        else
            _splunk.LogarMensagem("Saldo encontrado em cache");

        if (saldo.saldo < input.Valor_Transacao)
            throw new Exception("Saldo insuficiente na conta origem!");

        _splunk.LogarMensagem("Retirando valor conta origem");
        saldo = _contacliente.ExtrairSaldoConta(input);
        output.Saldo_Conta_Origem = saldo.saldo;

        if (await _transrepo.InserirContaCache(contahashkey, JsonSerializer.Serialize<ContaClienteSaldoDTO>(saldo)))
            _splunk.LogarMensagem("Conta salva em cache");
        else
            _splunk.LogarMensagem("Erro ao salvar conta em cache");

        _splunk.LogarMensagem("Depositando valor conta destino");
        saldo = _contacliente.DepositarSaldoConta(input);
        output.Saldo_Conta_Destino = saldo.saldo;

        Transacao trans = new Transacao(input.Numero_Conta_Origem, input.Numero_Agencia_Origem, input.Numero_Digito_Origem,
                                        input.Numero_Conta_Destino, input.Numero_Agencia_Destino, input.Numero_Digito_Destino,
                                        input.Valor_Transacao, DateTime.Now);

        trans.HashIndexOrigem = _hash.HashString(input.Numero_Conta_Origem + input.Numero_Agencia_Origem + input.Numero_Digito_Origem);
        trans.HashIndexDestino = _hash.HashString(input.Numero_Conta_Destino + input.Numero_Agencia_Destino + input.Numero_Digito_Destino);

        await _transrepo.InserirTransacaoDB(trans);
        _splunk.LogarMensagem("Transação persistida!");

        _transrepo.ProduzirTransacaoKafka("Api_Controle_Transacao", new Message<Null, string> { Value = JsonSerializer.Serialize<Transacao>(trans) });

        if (await _transrepo.InserirTransacaoCache(trans.Id, JsonSerializer.Serialize<Transacao>(trans)))
            _splunk.LogarMensagem("Transação salva em cache");
        else
            _splunk.LogarMensagem("Erro ao salvar no cache");

        return new Response("Transacao concluida", "OK", 200, output);
    }
    public async Task<dynamic> ConsultarTransacoesDate(TransacaoInputGetDateDTO input, string agencia, string conta, char digito)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        var index = _hash.HashString(conta + agencia + digito);
        List<TransacaoOutputGetDTO> listaTransacoesDTO = new List<TransacaoOutputGetDTO>();

        _splunk.LogarMensagem("Consultando Transacões de Origem");
        var transacoesOrigem = await _transrepo.ConsultarTransacoesDB("HashIndexOrigem", "Index_Origem", index);
        _splunk.LogarMensagem(transacoesOrigem.Count + " Transacões de origem encontrada(s).");

        _splunk.LogarMensagem("Consultando Transacões de Origem");
        var transacoesDestino = await _transrepo.ConsultarTransacoesDB("HashIndexDestino", "Index_Destino", index);
        _splunk.LogarMensagem(transacoesDestino.Count + " Transacões de destino encontrada(s).");

        var listaTransacoes = new List<dynamic>();
        listaTransacoes.AddRange(transacoesOrigem);
        listaTransacoes.AddRange(transacoesDestino);

        _splunk.LogarMensagem("Filtrando Transacões por data: " + input.Data_Inicial.ToString() + " >=data<= " + input.Data_Final.ToString());
        var listaFiltrada = listaTransacoes.Where(t => t.Data_Transacao >= input.Data_Inicial && t.Data_Transacao <= input.Data_Final);
        _splunk.LogarMensagem(listaFiltrada.Count().ToString() + " Transacões encontrada(s)");

        if (listaFiltrada.Count() == 0)
            throw new NullReferenceException("Nenhum resultado encontrado");

        foreach (Transacao trans in listaFiltrada)
        {
            listaTransacoesDTO.Add(new TransacaoOutputGetDTO(trans.Id, trans.Numero_Conta_Origem, trans.Numero_Agencia_Origem, trans.Numero_Digito_Origem,
                                                            trans.Numero_Conta_Destino, trans.Numero_Agencia_Destino, trans.Numero_Digito_Destino,
                                                            trans.Data_Transacao, trans.Valor_Transacao));
        }

        var resp = new Response(listaFiltrada.Count().ToString() + " Transacoes encontrada(s)", "OK", 200, listaTransacoesDTO);
        return resp;
    }
    public async Task<dynamic> ConsultarTransacoesCache()
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        _splunk.LogarMensagem("Consultando Transacões em cache");

        var keys = await _transrepo.ConsultarKeysTransacoesCache();
        var listaResposta = new List<dynamic>();

        foreach (var key in keys)
        {
            listaResposta.Add(JsonSerializer.Deserialize<Transacao>(await _transrepo.ConsultarTransacoesKeyCache(key)));
        }
        return new Response(listaResposta.Count() + " Transcoe(s) encontradas em cache", "OK", 200, listaResposta);
    }

    public async Task<dynamic> ConsultarTransacoesCpf(string cpf, DateTime datainicio, DateTime datafinal)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        var contas = _contacliente.ConsultarContasCliente(cpf);
        var listatrans = new List<Transacao>();
        List<TransacaoOutputGetDTO> listaresposta = new List<TransacaoOutputGetDTO>();
        foreach (ContaClienteListaContasDTO conta in contas)
        {
            var indexvalue = _hash.HashString(conta.numeroConta + conta.agencia + conta.digito);
            _splunk.LogarMensagem("Consultando transações para o index : " + indexvalue);
            var transorigem = await _transrepo.ConsultarTransacoesDB("HashIndexOrigem", "Index_Origem", indexvalue);
            var transdestino = await _transrepo.ConsultarTransacoesDB("HashIndexDestino", "Index_Destino", indexvalue);

            listatrans.AddRange(transorigem);
            listatrans.AddRange(transdestino);

            if (listatrans.Count > 0)
            {
                _splunk.LogarMensagem(listatrans.Count + " Transações encontradas");
                var listafiltrada = listatrans.Where(t => t.Data_Transacao >= datainicio && t.Data_Transacao <= datafinal).ToList();
                _splunk.LogarMensagem(listafiltrada.Count + " Transações filtradas");
                foreach (Transacao trans in listafiltrada)
                {
                    listaresposta.Add(new TransacaoOutputGetDTO(trans.Id, trans.Numero_Conta_Origem, trans.Numero_Agencia_Origem, trans.Numero_Digito_Origem,
                                                                    trans.Numero_Conta_Destino, trans.Numero_Agencia_Destino, trans.Numero_Digito_Destino,
                                                                    trans.Data_Transacao, trans.Valor_Transacao));
                }
            }
            else
                _splunk.LogarMensagem("Não foram encontradas transações");
        }
        listaresposta = listaresposta.DistinctBy(c => c.Id).ToList();
        if (listaresposta.Count() == 0)
            throw new NullReferenceException("Nenhum resultado encontrado");

        return new Response(listaresposta.Count().ToString() + " Transacoes encontrada(s)", "OK", 200, listaresposta);
    }

}