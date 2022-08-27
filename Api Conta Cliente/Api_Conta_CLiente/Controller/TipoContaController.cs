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
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value,input);
        var resp =  _tipocontaserv.CadastrarTipoConta(input);
        _splunk.EnviarLogAsync("testando splunk");
        return Ok(resp);
    }
}