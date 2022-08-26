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

    public async Task<dynamic> CadastrarClienteAsync(ClienteInputPostDTO input)
    {
        var sda =  MethodBase.GetCurrentMethod();
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = new Cliente(input.Nome, input.Cpf);
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        _splunk.LogarMensagem("Cliente Cadastrado:" + cliente.Nome);
        return "";
    }

    public dynamic CadastrarCliente(ClienteInputPostDTO input)
    {
        var sda =  MethodBase.GetCurrentMethod();
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = new Cliente(input.Nome, input.Cpf);
        _context.Clientes.Add(cliente);
        _context.SaveChanges();
        _splunk.LogarMensagem("Cliente Cadastrado:" + cliente.Nome);
        return "";
    }
}