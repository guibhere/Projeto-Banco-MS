using System.Text.Json.Serialization;
using Api_Conta_Cliente.Models;

public class LogModel{
    public string index{get;set;}
    [JsonPropertyName("event")]
    public Evento evento{get;set;}

    public LogModel()
    {
        evento = new Evento();
    }

    public class Evento
    {
        public string mensagem {get;set;}
        public string rota{get;set;}
        public string payload{get;set;}
        public string application{get;set;}
        public Response response{get;set;}
        public string severity {get;set;}
    }

}