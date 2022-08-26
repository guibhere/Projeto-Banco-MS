using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Api_Conta_Cliente.Helper.Interface;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate Next;


    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.Next = next;

    }

    public async Task Invoke(HttpContext context, ISplunkLogger _splunk)
    {
        try
        {
            await Next(context);
        }
        catch (Exception e)
        {
            await Handler(context, e,_splunk);
        }
    }

    private async Task Handler(HttpContext context, Exception e,ISplunkLogger _splunk)
    {
        var resposta = context.Response;
        resposta.ContentType = "application/json";
        if (e is Exception)
        {
            resposta.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        var result = JsonSerializer.Serialize(new { message = (e?.Message + "\n" + e?.InnerException + "\n Errou!!!!!!!!!! ") });
        _splunk.LogarMensagem("Ocorreu um erro:" + result.ToString());
        _splunk.EnviarLogAsync("Erro");
        await resposta.WriteAsync(result);
    }
}

