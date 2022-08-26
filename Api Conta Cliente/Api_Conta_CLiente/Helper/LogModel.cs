using System.Text.Json.Serialization;

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
    }

}