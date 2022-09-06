using System.Reflection;
using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using Api_Controle_Transacao.Helper;
using Api_Controle_Transacao.Helper.Interface;
using Api_Controle_Transacao.Models;
using Api_Controle_Transacao.Service.Interface;
using Confluent.Kafka;
using StackExchange.Redis;

public class TrasacaoService : ITrasacaoService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    private readonly IContaClienteConector _contacliente;
    private readonly IDynamoDBContext _dynamocontext;
    private readonly IHashMaker _hash;
    private readonly IProducer<Null, string> _kafkaproducer;
    private readonly IConnectionMultiplexer _redis;
    public TrasacaoService(IWebHostEnvironment environment, ISplunkLogger splunk, IContaClienteConector contacliente,
    IDynamoDBContext dynamo, IHashMaker hash, IProducer<Null, string> producer, IConnectionMultiplexer redis)
    {
        _environment = environment;
        _splunk = splunk;
        _contacliente = contacliente;
        _dynamocontext = dynamo;
        _hash = hash;
        _kafkaproducer = producer;
        _redis = redis;
    }

    public async Task<dynamic> ProcessarTransacao(TransacaoInputPostDTO input)
    {
        TransacaoOutputPostDTO output = new TransacaoOutputPostDTO();
        output.Valor_Transacao = input.Valor_Transacao;

        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var contahashkey = _hash.HashString(input.Numero_Conta_Origem + input.Numero_Agencia_Origem + input.Numero_Digito_Origem);

        _splunk.LogarMensagem("Consultando saldo em cache");
        var cacheresp = await _redis.GetDatabase().StringGetAsync(contahashkey);
        var saldo = (cacheresp.HasValue == false)? null : JsonSerializer.Deserialize<ContaClienteSaldoDTO>(cacheresp);

        if (saldo == null)
        {
            _splunk.LogarMensagem("Saldo não encontrado em cache");
            _splunk.LogarMensagem("Consultando saldo conta origem :" + input.Numero_Conta_Origem);
            saldo = _contacliente.ConsultarSaldoConta(input);
            _splunk.LogarMensagem("Saldo conta origem consultado:" + saldo.saldo);
        }

        if (saldo.saldo < input.Valor_Transacao)
            throw new Exception("Saldo insuficiente na conta origem!");

        _splunk.LogarMensagem("Retirando valor conta origem");
        saldo = _contacliente.ExtrairSaldoConta(input);
        output.Saldo_Conta_Origem = saldo.saldo;

        _splunk.LogarMensagem("Gravando conta origem em cache");
        await _redis.GetDatabase().StringSetAsync(contahashkey, JsonSerializer.Serialize<ContaClienteSaldoDTO>(saldo), TimeSpan.FromSeconds(600));
        _splunk.LogarMensagem("Conta salva em cache");

        _splunk.LogarMensagem("Depositando valor conta destino");
        saldo = _contacliente.DepositarSaldoConta(input);
        output.Saldo_Conta_Destino = saldo.saldo;

        _splunk.LogarMensagem("Persistindo Transação");
        await SalvarDados(input);
        return new Response("Transacao concluida", "OK", 200, output);
    }

    public async Task SalvarDados(TransacaoInputPostDTO input)
    {
        Transacao trans = new Transacao(input.Numero_Conta_Origem, input.Numero_Agencia_Origem, input.Numero_Digito_Origem,
                                        input.Numero_Conta_Destino, input.Numero_Agencia_Destino, input.Numero_Digito_Destino,
                                        input.Valor_Transacao, DateTime.Now);

        trans.HashIndexOrigem = _hash.HashString(input.Numero_Conta_Origem + input.Numero_Agencia_Origem + input.Numero_Digito_Origem);
        trans.HashIndexDestino = _hash.HashString(input.Numero_Conta_Destino + input.Numero_Agencia_Destino + input.Numero_Digito_Destino);

        await _dynamocontext.SaveAsync(trans);
        _splunk.LogarMensagem("Transação persistida!");

        _splunk.LogarMensagem("Produzindo mensagem KAFKA");
        var msg = new Message<Null, string>();
        msg.Value = JsonSerializer.Serialize<Transacao>(trans);
        var resp = await _kafkaproducer.ProduceAsync("Api_Controle_Transacao", msg);
        _splunk.LogarMensagem("Mesagem postada");

        _splunk.LogarMensagem("Gravando transação em cache");
        var db = _redis.GetDatabase();
        await db.StringSetAsync(trans.Id, JsonSerializer.Serialize<Transacao>(trans), TimeSpan.FromSeconds(600));
        _splunk.LogarMensagem("Transação salva em cache");
    }

    public async Task<dynamic> ConsultarTransacoesDate(TransacaoInputGetDateDTO input, string agencia, string conta, string digito)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var index = _hash.HashString(conta + agencia + digito);
        List<TransacaoOutputGetDTO> listaTransacoesDTO = new List<TransacaoOutputGetDTO>();

        //Query Origem 
        var queryFilter = new QueryFilter("HashIndexOrigem", QueryOperator.Equal, index);
        var queryConf = new QueryOperationConfig()
        {
            IndexName = "Index_Origem",
            Filter = queryFilter
        };
        _splunk.LogarMensagem("Consultando Transacões de Origem");
        var transacoesOrigem = await _dynamocontext.FromQueryAsync<Transacao>(queryConf).GetRemainingAsync();
        _splunk.LogarMensagem(transacoesOrigem.Count().ToString() + " Transacões de origem encontrada(s).");

        // Query Destino
        queryFilter = new QueryFilter("HashIndexDestino", QueryOperator.Equal, index);
        queryConf = new QueryOperationConfig()
        {
            IndexName = "Index_Destino",
            Filter = queryFilter
        };
        _splunk.LogarMensagem("Consultando Transacões de Destino");
        var transacoesDestino = await _dynamocontext.FromQueryAsync<Transacao>(queryConf).GetRemainingAsync();
        _splunk.LogarMensagem(transacoesDestino.Count().ToString() + " Transacões de destino encontrada(s).");

        var listaTransacoes = new List<dynamic>();
        listaTransacoes.AddRange(transacoesOrigem);
        listaTransacoes.AddRange(transacoesDestino);

        _splunk.LogarMensagem("Filtrando Transacões por data: " + input.Data_Inicial.ToString() + " >=data<= " + input.Data_Final.ToString());
        var listaFiltrada = listaTransacoes.Where(t => t.Data_Transacao >= input.Data_Inicial && t.Data_Transacao <= input.Data_Final);
        _splunk.LogarMensagem(listaFiltrada.Count().ToString() + " Transacões encontrada(s)");

        if (listaFiltrada.Count() == 0)
            throw new Exception("Não foram encontrados resultados");

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
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        _splunk.LogarMensagem("Consultando Transacões em cache");
        var db = _redis.GetDatabase();
        var endPoint = _redis.GetEndPoints().First();
        RedisKey[] keys = _redis.GetServer(endPoint).Keys(pattern: "*").ToArray();
        var listResposta = new List<dynamic>();

        foreach (var key in keys)
        {
            var result = await db.StringGetAsync(key);
            listResposta.Add(JsonSerializer.Deserialize<Transacao>(result));
        }
        return new Response(listResposta.Count() + " Transcoe(s) encontradas em cache", "OK", 200, listResposta);
    }
}