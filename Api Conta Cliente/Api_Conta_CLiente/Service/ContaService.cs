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

public class ContaService : IContaService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    public ContaService(ApplicationDbContext context, IWebHostEnvironment environment, ISplunkLogger splunk)
    {
        _context = context;
        _environment = environment;
        _splunk = splunk;
    }

    public dynamic CadastrarConta(ContaInputPostDTO input,string cpf)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = _context.Clientes.FirstOrDefault(c => c.Cpf== cpf);

        if(cliente == null)
        {
            _splunk.LogarMensagem("Cliente não encontrado:" + cpf);
            throw new Exception("Cliente não encontrado");
        }
        var conta = new Conta(input.Agencia, input.NumeroConta,input.Digito,input.Saldo,cpf);
        _context.Contas.Add(conta);
        _context.SaveChanges();
        _splunk.LogarMensagem("Conta Cadastrado:" + conta.Id);
        return "";
    }
}