using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


public class ClienteInputPostDTO{
    public string Nome { get; set; }
    public string Cpf { get; set; }
}
public class ClienteInputPatchDTO{
    public string Nome { get; set; }
    public string Cpf { get; set; }
}