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
public class AgenciaController : ControllerBase
{
    private readonly IAgenciaService _agenciaserv;
    private readonly ISplunkLogger _splunk;
    public AgenciaController(IAgenciaService agenciaService, ISplunkLogger splunk)
    {
        _agenciaserv = agenciaService;
        _splunk = splunk;
    }

    [HttpPost("Cadastrar")]
    public async Task<ActionResult<dynamic>> CadastrarAgencia([FromBody] AgenciaInputPostDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = await _agenciaserv.CadastrarAgencia(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpPatch("Atualizar")]
    public async Task<ActionResult<dynamic>> AtualizarAgencia([FromBody] AgenciaInputPostDTO input)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = await _agenciaserv.AtualizarAgencia(input);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/Lista")]
    public async Task<ActionResult<dynamic>> ConsultarAgencias()
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _agenciaserv.ConsultarAgencias();
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Consultar/{numero_Agencia}")]
    public async Task<ActionResult<dynamic>> ConsultarAgencia(string numero_Agencia)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _agenciaserv.ConsultarAgencia(numero_Agencia);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp.Dados);
    }
    [HttpDelete("Excluir/{numero_Agencia}")]
    public async Task<ActionResult<dynamic>> ExcluirAgencia(string numero_Agencia)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _agenciaserv.ExcluirAgencia(numero_Agencia);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
}