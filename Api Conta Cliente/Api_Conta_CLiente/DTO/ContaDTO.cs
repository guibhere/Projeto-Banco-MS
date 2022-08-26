using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


public class ContaInputPostDTO
{
    public string Agencia { get; set; }
    public string NumeroConta { get; set; }
    public char Digito { get; set; }
    public Decimal Saldo { get; set; }
}