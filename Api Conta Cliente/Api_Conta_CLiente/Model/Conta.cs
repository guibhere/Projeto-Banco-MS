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
        [Key]
        public long Id { get; set; }
        public string Agencia { get; set; }
        public string Numero_Conta { get; set; }
        public char Digito { get; set; }
        public Decimal Saldo { get; set; }
        [ForeignKey("Cpf")]
        [Required]
        public string Cpf { get; set; }
        public Cliente Cliente{get;set;}
        public Conta(string agencia, string numero_Conta, char digito, decimal saldo,string cpf)
        {
            Agencia = agencia;
            Numero_Conta = numero_Conta;
            Digito = digito;
            Saldo = saldo;
            Cpf = cpf;
        }

    }

}
