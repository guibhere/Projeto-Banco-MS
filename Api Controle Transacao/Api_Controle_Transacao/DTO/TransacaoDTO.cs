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
    public decimal Valor_Transacao{get;set;}
    public decimal Saldo_Conta_Origem{get;set;}
    public decimal Saldo_Conta_Destino{get;set;}
}