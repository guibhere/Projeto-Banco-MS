using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Conta_Cliente.Models
{
    public class Conta
    {
        public long Id { get; set; }
        public string Numero_Agencia{get;set;}
        public virtual Agencia Agencia { get; set; }
        public string Numero_Conta { get; set; }
        public char Digito { get; set; }
        public Decimal Saldo { get; set; }
        public string Cpf { get; set; }
        public virtual Cliente Cliente{get;set;}
        public virtual TipoConta TipoConta{get;set;}
        public long Codigo_Tipo_Conta{get;set;}
        public Conta(string numero_Agencia, string numero_Conta, char digito, decimal saldo,string cpf,long codigo_Tipo_Conta)
        {
            Numero_Agencia = numero_Agencia;
            Numero_Conta = numero_Conta;
            Digito = digito;
            Saldo = saldo;
            Cpf = cpf;
            Codigo_Tipo_Conta = codigo_Tipo_Conta;
        }

    }

}
