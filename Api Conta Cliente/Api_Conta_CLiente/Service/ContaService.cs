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

    public dynamic CadastrarConta(ContaInputPostDTO input, string cpf)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var cliente = _context.Clientes.FirstOrDefault(c => c.Cpf == cpf);

        if (cliente == null)
        {
            throw new Exception("Cliente não encontrado");
        }
        var conta = new Conta(input.Agencia, input.NumeroConta, input.Digito, input.Saldo, cpf, input.Codigo_Tipo_Conta);
        _context.Contas.Add(conta);
        _context.SaveChanges();
        _splunk.LogarMensagem("Conta Cadastrada:" + conta.Id);
        return ("Conta Cadastrada:" + conta.Id);
    }
    public dynamic ConsultarSaldo(string agencia, string numero_conta, char digito)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var conta = _context.Contas.FirstOrDefault(c => c.Numero_Agencia == agencia && c.Numero_Conta == numero_conta && c.Digito == digito);
        return new ContaOutputConsultaSaldoDTO(conta.Saldo);
    }
    public dynamic DepositarSaldo(ContaInputPatchAtualizarSaldoDTO input, string agencia, string numero_conta, char digito)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var conta = _context.Contas.FirstOrDefault(c => c.Numero_Agencia == agencia && c.Numero_Conta == numero_conta && c.Digito == digito);
        if (conta != null)
        {
            _splunk.LogarMensagem("Depositando saldo :" + input.valor);
            conta.Saldo += input.valor;
            _context.Contas.Update(conta);
            _context.SaveChanges();
        }
        else
            throw new Exception("Conta não encontrada!");

        return new ContaOutputConsultaSaldoDTO(conta.Saldo);
    }
    public dynamic ExtrairSaldo(ContaInputPatchAtualizarSaldoDTO input, string agencia, string numero_conta, char digito)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var conta = _context.Contas.FirstOrDefault(c => c.Numero_Agencia == agencia && c.Numero_Conta == numero_conta && c.Digito == digito);
        if (conta != null)
        {
            _splunk.LogarMensagem("Retirando saldo :" + input.valor);
            conta.Saldo -= input.valor;
            if (conta.Saldo < 0)
                throw new Exception("Saldo Insuficiente");
            _context.Contas.Update(conta);
            _context.SaveChanges();
        }
        else
            throw new Exception("Conta não encontrada!");

        return new ContaOutputConsultaSaldoDTO(conta.Saldo);
    }
    public dynamic ConsultarConstasCpf(string cpf)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);
        var contas = _context.Contas.ToList().Where(c => c.Cpf == cpf);
        var contasResponse = new List<ContaoOutuputGetListaContasDTO>();

        foreach(Conta conta in contas)
        {
            contasResponse.Add(new ContaoOutuputGetListaContasDTO{NumeroConta = conta.Numero_Conta, Agencia = conta.Numero_Agencia, Digito = conta.Digito});
        }
        return contasResponse;
    }
}