using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


public class TransacaoInputPostDTO
{
    public long Id { get; set; }
    public string Numero_Conta_Origem { get; set; }
    public string Numero_Agencia_Origem { get; set; }
    public char Numero_Digito_Origem { get; set; }
    public string Numero_Conta_Destino { get; set; }
    public string Numero_Agencia_Destino { get; set; }
    public char Numero_Digito_Destino { get; set; }
    public decimal Valor_Transacao { get; set; }
}

public class TransacaoOutputPostDTO
{
    public decimal Valor_Transacao { get; set; }
    public decimal Saldo_Conta_Origem { get; set; }
    public decimal Saldo_Conta_Destino { get; set; }
}

public class TransacaoInputGetDateDTO
{
    public DateTime Data_Inicial { get; set; }
    public DateTime Data_Final { get; set; }
}
public class TransacaoOutputGetDTO
{
    public string Id { get; set; }
    public string Conta_Origem { get; set; }
    public string Agencia_Origem { get; set; }
    public char Dac_Origem { get; set; }
    public string Conta_Destino { get; set; }
    public string Agencia_Destino { get; set; }
    public char Dac_Destino { get; set; }
    public DateTime Data_Transacao { get; set; }
    public decimal Valor_Transacao { get; set; }
    public TransacaoOutputGetDTO(string id, string contaorigem, string agenciaorigem, char dacorigem,
                                 string contadestino, string agenciadestino, char dacdestino,
                                 DateTime datatransacao, decimal valortransacao)
    {
        Id = id;
        Conta_Origem = contaorigem;
        Agencia_Origem = agenciaorigem;
        Dac_Origem = dacorigem;
        Conta_Destino = contadestino;
        Agencia_Destino = agenciadestino;
        Dac_Destino = dacdestino;
        Data_Transacao = datatransacao;
        Valor_Transacao = valortransacao;
    }
}
