using Microsoft.AspNetCore.Mvc;
using Api_Conta_Cliente.Service.Interface;
using Api_Conta_Cliente.Helper.Interface;
//[Authorize]
[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _clienteserv;
    private readonly ISplunkLogger _splunk;
    public ClienteController(IClienteService clienteService, ISplunkLogger splunk)
    {
        _clienteserv = clienteService;
        _splunk = splunk;
    }

    [HttpPost("Cadastrar")]
    public async Task<ActionResult<dynamic>> CadastrarCliente([FromBody] ClienteInputPostDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = await _clienteserv.CadastrarCliente(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/Lista")]
    public async Task<ActionResult<dynamic>> ConsultarClientesLista()
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _clienteserv.ConsultarClientesLista();
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/{cpf}")]
    public async Task<ActionResult<dynamic>> ConsultarCliente(string cpf)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _clienteserv.ConsultarCliente(cpf);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp.Dados);
    }
    [HttpPatch("Atualizar")]
    public async Task<ActionResult<dynamic>> AtualizarCliente([FromBody] ClienteInputPatchDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _clienteserv.AtualizarCliente(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpDelete("Excluir/{cpf}")]
    public async Task<ActionResult<dynamic>> ExcluirCliente(string cpf)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _clienteserv.ExcluirCliente(cpf);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
}