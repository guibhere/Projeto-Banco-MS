using System.Reflection;
using Api_Controle_Transacao.Helper.Interface;
using Api_Controle_Transacao.Models;
using Api_Controle_Transacao.Service.Interface;


public class TrasacaoService : ITrasacaoService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    private readonly IContaClienteConector _contacliente;
    public TrasacaoService(ApplicationDbContext context, IWebHostEnvironment environment, ISplunkLogger splunk, IContaClienteConector contacliente)
    {
        _context = context;
        _environment = environment;
        _splunk = splunk;
        _contacliente = contacliente;
    }

    public dynamic ProcessarTransacao(TransacaoInputPostDTO input)
    {
        TransacaoOutputPostDTO output = new TransacaoOutputPostDTO();
        output.Valor_Transacao = input.Valor_Transacao;

        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        _splunk.LogarMensagem("Consultando saldo conta origem :" + input.Numero_Conta_Origem);
        var saldo = _contacliente.ConsultarSaldoConta(input);
        _splunk.LogarMensagem("Saldo conta origem consultado:" + saldo.saldo);

        if(saldo.saldo < input.Valor_Transacao)
            throw new Exception("Saldo insuficiente na conta origem!");
        
        _splunk.LogarMensagem("Retirando valor conta origem");
        saldo = _contacliente.ExtrairSaldoConta(input);
        output.Saldo_Conta_Origem = saldo.saldo;

        _splunk.LogarMensagem("Depositando valor conta destino");
        saldo = _contacliente.DepositarSaldoConta(input);
        output.Saldo_Conta_Destino = saldo.saldo;

        _context.SaveChanges();
        return new Response("Transacao concluida","OK",200,output);
    }
}