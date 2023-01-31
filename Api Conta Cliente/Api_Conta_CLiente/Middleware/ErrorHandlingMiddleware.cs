using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Api_Conta_Cliente.Helper.Interface;
using Api_Conta_Cliente.Models;

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
            await Handler(context, e, _splunk);
        }
    }

    private async Task Handler(HttpContext context, Exception e, ISplunkLogger _splunk)
    {
        var resposta = context.Response;
        resposta.ContentType = "application/json";

        switch (e)
        {
            case NullReferenceException:
                resposta.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            default:
                resposta.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var response = new Response(e.Message, "Erro", resposta.StatusCode, e.InnerException != null ? e.InnerException.Message : null);

        _splunk.LogarMensagem("Ocorreu um erro: " + e.Message);
        _splunk.Log.evento.severity = "Error";
        _splunk.EnviarLogAsync(response);
    
        await resposta.WriteAsync(JsonSerializer.Serialize<Response>(response));
    }
}

