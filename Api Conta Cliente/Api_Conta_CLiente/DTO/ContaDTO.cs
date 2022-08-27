using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


public class ContaInputPostDTO
{
    public string Agencia { get; set; }
    public string NumeroConta { get; set; }
    public char Digito { get; set; }
    public Decimal Saldo { get; set; }
    public long Codigo_Tipo_Conta{get;set;}
}
public class ContaOutputConsultaSaldoDTO
{
    public Decimal Saldo{get;set;}
    public ContaOutputConsultaSaldoDTO(decimal saldo)
    {
        Saldo = saldo;
    }
}
public class ContaInputPatchAtualizarSaldoDTO
{
    public Decimal valor{get;set;}
}