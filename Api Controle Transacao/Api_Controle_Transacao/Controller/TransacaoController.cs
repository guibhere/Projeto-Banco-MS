using Microsoft.AspNetCore.Mvc;
using Api_Controle_Transacao.Service.Interface;
using Api_Controle_Transacao.Helper.Interface;
//[Authorize]
[ApiController]
[Route("[controller]")]
public class TransacaoController : ControllerBase
{
    private readonly ITrasacaoService _transserv;
    private readonly ISplunkLogger _splunk;
    public TransacaoController(ITrasacaoService TrasacaoService, ISplunkLogger splunk)
    {
        _transserv = TrasacaoService;
        _splunk = splunk;
    }

    [HttpPost("Processar/Transacao")]
    public async Task<ActionResult<dynamic>> ProcessarTransacao([FromBody] TransacaoInputPostDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = await _transserv.ProcessarTransacao(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpPost("Consultar/Transacao/Data/{agencia}/{conta}/{digito}")]
    public async Task<ActionResult<dynamic>> ConsultarTransacaoDate([FromBody] TransacaoInputGetDateDTO input, string agencia, string conta, char digito)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = await _transserv.ConsultarTransacoesDate(input, agencia, conta, digito);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/Transacao/Cache")]
    public async Task<ActionResult<dynamic>> ConsultarTransacaoCache()
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _transserv.ConsultarTransacoesCache();
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/Transacao/Cpf/{cpf}/{datainicio}/{datafinal}")]
    public async Task<ActionResult<dynamic>> ConsultarTransacaoCpf(string cpf, DateTime datainicio, DateTime datafinal)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _transserv.ConsultarTransacoesCpf(cpf,datainicio,datafinal);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
}