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

    public async Task<dynamic> AtualizarTipoConta(TipoContaInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var tipoConta = new TipoConta(input.Codigo_Tipo_Conta, input.Descricao);
        _context.TipoContas.Update(tipoConta);
        _context.SaveChanges();
        _splunk.LogarMensagem("Tipo Conta atualizado:" + tipoConta.Codigo_Conta);
        return new Response("Tipo conta atualizado", "OK", 200, tipoConta);
    }

    public async Task<dynamic> CadastrarTipoConta(TipoContaInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var tipoConta = new TipoConta(input.Codigo_Tipo_Conta, input.Descricao);
        _context.TipoContas.Add(tipoConta);
        _splunk.LogarMensagem("Tipo Conta Cadastrado:" + tipoConta.Codigo_Conta);
        return new Response("Tipo conta cadastrado", "OK", 200, tipoConta);
    }

    public async Task<dynamic> ConsultarTipoConta(int codigo_tipo_conta)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var tipoConta = await _context.TipoContas.FirstOrDefaultAsync<TipoConta>(tc => tc.Codigo_Conta == codigo_tipo_conta);
        var tipoContaDTO = new TipoContaInputPostDTO { Codigo_Tipo_Conta = tipoConta.Codigo_Conta, Descricao = tipoConta.Descricao };
        _splunk.LogarMensagem("Tipo Contas consultado: " + tipoContaDTO.Descricao);
        return new Response("Tipo Contas consultados", "OK", 200, tipoContaDTO);
    }

    public async Task<dynamic> ConsultarTipoContas()
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var tipoContas = await _context.TipoContas.ToListAsync<TipoConta>();

        var resplist = new List<TipoContaInputPostDTO>();
        foreach (TipoConta tipoconta in tipoContas)
        {
            resplist.Add(new TipoContaInputPostDTO { Codigo_Tipo_Conta = tipoconta.Codigo_Conta, Descricao = tipoconta.Descricao });
        }

        _splunk.LogarMensagem("Tipo Contas consultados");
        return new Response("Tipo Contas consultados", "OK", 200, resplist);
    }

    public async Task<dynamic> ExcluirTipoConta(int codigo_tipo_conta)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var result = ConsultarTipoConta(codigo_tipo_conta).Result.Dados;
        var tipoConta = await _context.TipoContas.FirstOrDefaultAsync<TipoConta>(tc => tc.Codigo_Conta == codigo_tipo_conta);
        _context.Remove(tipoConta);
        await _context.SaveChangesAsync();
        _splunk.LogarMensagem("Tipo Contas removido: " + tipoConta.Codigo_Conta);
        return new Response("Tipo Contas removido", "OK", 200, tipoConta);
    }
}