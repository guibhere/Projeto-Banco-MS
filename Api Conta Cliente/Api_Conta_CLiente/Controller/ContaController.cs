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
    public async Task<ActionResult<dynamic>> CadastrarConta([FromBody] ContaInputPostDTO input,string cpf)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value,input);
        var resp =  _contaserv.CadastrarConta(input,cpf);
        _splunk.EnviarLogAsync("testando splunk");
        return Ok(resp);
    }
}