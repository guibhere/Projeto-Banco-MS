using System.Net.Http.Headers;
using Api_Controle_Transacao.Helper.Interface;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;
using Api_Controle_Transacao.Models;

namespace Api_Controle_Transacao.Helper
{
    public class SplunkLogger : ISplunkLogger
    {
        public LogModel Log {get;set;}
        private readonly IOptions<SplunkConfig> _options;
        public SplunkLogger(){
            this.Log = new LogModel();
        }
        public class SplunkConfig
        {
            public string SplunkCollectorUrl { get; set; }
            public string Token { get; set; }

        }

        public SplunkLogger(IOptions<SplunkConfig> options)
        {
            _options = options;
        }
        public void IniciarLog(string rota,object objeto)
        {
            this.Log = new LogModel{index = "history",evento = new LogModel.Evento()};
            this.Log.evento.rota = rota;
            this.Log.evento.payload = JsonSerializer.Serialize(objeto);
            LogarMensagem("Iniciando API");            
        }

        public string LogarMensagem(string msg)
        {
            if( String.IsNullOrEmpty(this.Log.evento.mensagem))
                this.Log.evento.mensagem += msg;
            else
                this.Log.evento.mensagem += "\n" + msg;
            return this.Log.evento.mensagem; 
        }

        public async Task EnviarLogAsync(Response response)
        {
            var url = _options.Value.SplunkCollectorUrl;
            var options = new RestClientOptions(url) {RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true};
            var client = new RestClient(options);
            var request = new RestRequest(url, Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Splunk " + _options.Value.Token);

            this.Log.evento.response = response;
            var json = JsonSerializer.Serialize(this.Log);
            request.AddBody(json, "application/json");
            RestResponse restresponse = await client.ExecuteAsync(request);
            var output = restresponse.Content;
        }
    }
}