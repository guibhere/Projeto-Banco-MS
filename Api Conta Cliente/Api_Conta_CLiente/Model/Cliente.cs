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
    public class Cliente
    {
        [Key]
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public Cliente(string nome, string cpf)
        {
            Nome = nome;
            Cpf = cpf;
        }

    }

}
