using System.Text.Json.Serialization;
using Integrador.Models;

public class LogModel{
    public string index{get;set;}
    [JsonPropertyName("event")]
    public Evento evento{get;set;}

    public LogModel()
    {
    }

    public class Evento
    {
        public string mensagem {get;set;}
        public string rota{get;set;}
        public string payload{get;set;}
        public string application{get;set;}
        public string topic{get;set;}
        public string kafka_role{get;set;}
        public Response response{get;set;}
    }

}