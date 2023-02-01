using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using System.Threading;
using Api_Conta_Cliente.Service.Interface;
using Api_Conta_Cliente.Helper.Interface;
//[Authorize]
[ApiController]
[Route("[controller]")]
public class TipoContaController : ControllerBase
{
    private readonly ITipoContaService _tipocontaserv;
    private readonly ISplunkLogger _splunk;
    public TipoContaController(ITipoContaService tipocontaService, ISplunkLogger splunk)
    {
        _tipocontaserv = tipocontaService;
        _splunk = splunk;
    }

    [HttpPost("Cadastrar")]
    public async Task<ActionResult<dynamic>> CadastrarTipoConta([FromBody] TipoContaInputPostDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = await _tipocontaserv.CadastrarTipoConta(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpPatch("Atualizar")]
    public async Task<ActionResult<dynamic>> AtualizarTipoConta([FromBody] TipoContaInputPostDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = await _tipocontaserv.AtualizarTipoConta(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/Lista")]
    public async Task<ActionResult<dynamic>> ConsultarTipoContas()
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _tipocontaserv.ConsultarTipoContas();
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/{codigo_tipo_conta}")]
    public async Task<ActionResult<dynamic>> CadastrarTipoConta(int codigo_tipo_conta)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _tipocontaserv.ConsultarTipoConta(codigo_tipo_conta);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp.Dados);
    }
    [HttpDelete("Excluir/{codigo_tipo_conta}")]
    public async Task<ActionResult<dynamic>> ExcluirTipoConta(int codigo_tipo_conta)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _tipocontaserv.ExcluirTipoConta(codigo_tipo_conta);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
}