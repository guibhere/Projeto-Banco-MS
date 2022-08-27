using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Api_Controle_Transacao.Helper.Interface;
using Api_Controle_Transacao.Models;

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
        Response response = new Response();
        resposta.ContentType = "application/json";
        if (e is Exception)
        {
            resposta.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        response.TipoRetorno = "Erro";
        response.CodigoRetoro = resposta.StatusCode;
        response.Mensagem = e.Message;
        response.Dados = e.InnerException;

        var result = JsonSerializer.Serialize<Response>(response);
        _splunk.LogarMensagem("Ocorreu um erro: " + e.Message);
        _splunk.EnviarLogAsync(response);
        await resposta.WriteAsync(result);
    }
}

