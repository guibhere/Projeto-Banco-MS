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

public class AgenciaService : IAgenciaService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    public AgenciaService(ApplicationDbContext context, IWebHostEnvironment environment, ISplunkLogger splunk)
    {
        _context = context;
        _environment = environment;
        _splunk = splunk;
    }

    public dynamic CadastrarAgencia(AgenciaInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var agencia = new Agencia(input.Numero_Agencia,input.Descricao);
        _context.Agencias.Add(agencia);
        _context.SaveChanges();
        _splunk.LogarMensagem("Agencia Cadastrada:" + agencia.Numero_Agencia);
        return "";
    }
}