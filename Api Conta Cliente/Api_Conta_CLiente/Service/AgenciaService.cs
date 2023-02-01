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

    public async Task<dynamic> AtualizarAgencia(AgenciaInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var agencia = new Agencia(input.Numero_Agencia, input.Descricao);
        _context.Agencias.Update(agencia);
        await _context.SaveChangesAsync();
        _splunk.LogarMensagem("Agencia atualizada:" + agencia.Numero_Agencia);
        return new Response("Agencia atualizada", "OK", 200, agencia);
    }

    public async Task<dynamic> CadastrarAgencia(AgenciaInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var agencia = new Agencia(input.Numero_Agencia, input.Descricao);
        _context.Agencias.Add(agencia);
        await _context.SaveChangesAsync();
        _splunk.LogarMensagem("Agencia Cadastrada:" + agencia.Numero_Agencia);
        return new Response("Agencia cadastrada", "OK", 200, agencia);
    }

    public async Task<dynamic> ConsultarAgencia(string numero_Agencia)
    {
        _splunk.LogarMensagem("Iniciando :" + numero_Agencia.ToString() + MethodBase.GetCurrentMethod().Name);

        var agencia = await _context.Agencias.FirstOrDefaultAsync<Agencia>(a => a.Numero_Agencia == numero_Agencia.ToString());

        _splunk.LogarMensagem("Agencia consultada:" + agencia.Numero_Agencia);
        return new Response("Agencia cadastrada", "OK", 200, agencia);
    }

    public async Task<dynamic> ConsultarAgencias()
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var agencias = await _context.Agencias.ToListAsync<Agencia>();

        _splunk.LogarMensagem("Agencia consultadas");
        return new Response("Agencia consultadas", "OK", 200, agencias);
    }

    public async Task<dynamic> ExcluirAgencia(string numero_Agencia)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var agencia = ConsultarAgencia(numero_Agencia).Result.Dados;

        _context.Agencias.Remove(agencia);
        await _context.SaveChangesAsync();

        _splunk.LogarMensagem("Agencia removida:" + agencia.Descricao);
        return new Response("Agencia removida", "OK", 200, agencia);
    }
}