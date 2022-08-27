using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Api_Conta_Cliente.Helper.Interface;
using Api_Conta_Cliente.Models;
using Api_Conta_Cliente.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class TipoContaService : ITipoContaService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    public TipoContaService(ApplicationDbContext context, IWebHostEnvironment environment, ISplunkLogger splunk)
    {
        _context = context;
        _environment = environment;
        _splunk = splunk;
    }

    public dynamic CadastrarTipoConta(TipoContaInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var tipoConta = new TipoConta(input.Codigo_Tipo_Conta,input.Descricao);
        _context.TipoContas.Add(tipoConta);
        _context.SaveChanges();
        _splunk.LogarMensagem("Tipo Conta Cadastrado:" + tipoConta.Codigo_Conta);
        return "";
    }
}