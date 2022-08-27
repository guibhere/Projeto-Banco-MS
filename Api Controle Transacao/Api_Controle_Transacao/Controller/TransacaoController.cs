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

    [HttpPost("ProcessarTransacao")]
    public async Task<ActionResult<dynamic>> ProcessarTransacao([FromBody] TransacaoInputPostDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = _transserv.ProcessarTransacao(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
}