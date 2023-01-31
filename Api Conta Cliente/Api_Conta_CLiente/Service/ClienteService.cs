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

public class ClienteService : IClienteService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    public ClienteService(ApplicationDbContext context, IWebHostEnvironment environment, ISplunkLogger splunk)
    {
        _context = context;
        _environment = environment;
        _splunk = splunk;
    }

    public async Task<dynamic> AtualizarCliente(ClienteInputPatchDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = new Cliente(input.Nome, input.Cpf);
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
        _splunk.LogarMensagem("Cliente Atualizado:" + cliente.Nome);
        return new Response("Cliente Atualizado", "OK", 200, cliente);
    }

    public async Task<dynamic> CadastrarCliente(ClienteInputPostDTO input)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = new Cliente(input.Nome, input.Cpf);
        await _context.Clientes.AddAsync(cliente);
        _context.SaveChanges();
        _splunk.LogarMensagem("Cliente Cadastrado:" + cliente.Nome);
        return new Response("Cliente Cadastrado", "OK", 200, cliente);
    }

    public async Task<dynamic> ConsultarCliente(string cpf)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = await _context.Clientes.FirstOrDefaultAsync<Cliente>(c => c.Cpf == cpf);
        _splunk.LogarMensagem("Cliente Consultado:" + cliente.Nome);
        return new Response("Cliente Consultado", "OK", 200, cliente);
    }

    public async Task<dynamic> ConsultarClientesLista()
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var clientes = await _context.Clientes.ToListAsync<Cliente>();
        return new Response("Clientes Consultados", "OK", 200, clientes);
    }

    public async Task<dynamic> ExcluirCliente(string cpf)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = ConsultarCliente(cpf).Result.Dados;

        _context.Clientes.Remove(cliente);
        _context.SaveChanges();

        _splunk.LogarMensagem("Cliente removido:" + cliente.Nome);
        return new Response("Cliente removido", "OK", 200, cliente);
    }
}