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
public class ContaController : ControllerBase
{
    private readonly IContaService _contaserv;
    private readonly ISplunkLogger _splunk;
    public ContaController(IContaService contaService, ISplunkLogger splunk)
    {
        _contaserv = contaService;
        _splunk = splunk;
    }

    [HttpPost("Cadastrar/{cpf}")]
    public async Task<ActionResult<dynamic>> CadastrarConta([FromBody] ContaInputPostDTO input, string cpf)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = _contaserv.CadastrarConta(input, cpf);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }

    [HttpGet("Consultar/Saldo/{agencia}/{conta}/{digito}")]
    public async Task<ActionResult<dynamic>> ConsultarSaldo(string agencia, string conta, char digito)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = _contaserv.ConsultarSaldo(agencia, conta, digito);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);

    }
    [HttpPatch("Atualizar/Saldo/Depositar/{agencia}/{conta}/{digito}")]
    public async Task<ActionResult<dynamic>> DepositarSaldo([FromBody] ContaInputPatchAtualizarSaldoDTO input, string agencia, string conta, char digito)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = _contaserv.DepositarSaldo(input, agencia, conta, digito);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);

    }
    [HttpPatch("Atualizar/Saldo/Extrair/{agencia}/{conta}/{digito}")]
    public async Task<ActionResult<dynamic>> ExtrairSaldo([FromBody] ContaInputPatchAtualizarSaldoDTO input, string agencia, string conta, char digito)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, input);
        var resp = _contaserv.ExtrairSaldo(input, agencia, conta, digito);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }

    [HttpGet("Contas/Lista/{cpf}")]
    public async Task<ActionResult<dynamic>> ConsultarContasListaCpf(string cpf)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = _contaserv.ConsultarConstasCpf(cpf);
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
    [HttpGet("Contas/Lista")]
    public async Task<ActionResult<dynamic>> ConsultarContas()
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, "");
        var resp = await _contaserv.ConsultarContas();
        _splunk.EnviarLogAsync(resp);
        return Ok(resp);
    }
}