using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Diagnostics;
using Api_Conta_Cliente.Helper.Interface;
using Api_Conta_Cliente.Models;

namespace Api_Conta_Cliente.Helper
{
    public class SplunkLogger : ISplunkLogger
    {
        public LogModel Log { get; set; }
        private readonly IOptions<SplunkConfig> _options;
        private string callerClass;
        public SplunkLogger()
        {
            this.Log = new LogModel();
        }
        public class SplunkConfig
        {
            public string SplunkCollectorUrl { get; set; }
            public string Token { get; set; }
            public string Application { get; set; }

        }

        public SplunkLogger(IOptions<SplunkConfig> options)
        {
            _options = options;
        }
        public void IniciarLog(string rota, object objeto)
        {
            this.Log = new LogModel { index = "history", evento = new LogModel.Evento() };
            this.Log.evento.rota = rota;
            this.Log.evento.payload = JsonSerializer.Serialize(objeto);
            this.Log.evento.application = _options.Value.Application;
            this.Log.evento.severity = "Sucess";
            LogarMensagem("Iniciando API");
        }

        public void LogarMensagem(string msg, [CallerMemberName] string nomemetodo = "", [CallerLineNumber] int numerolinha = 0, [CallerFilePath] string path ="")
        {
            var datetime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            string mensagem = datetime.ToString();
            mensagem += " " + GetClassFromPath(path) + "." + nomemetodo;
            mensagem += " " + "linha(" + numerolinha + ")";
            mensagem += " " + msg;

            if (String.IsNullOrEmpty(this.Log.evento.mensagem))
                this.Log.evento.mensagem += mensagem;
            else
                this.Log.evento.mensagem += "\n" + mensagem;
        }

        public async Task EnviarLogAsync(Response response)
        {
            var url = _options.Value.SplunkCollectorUrl;
            var options = new RestClientOptions(url) { RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true };
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
        public string GetClassFromPath(string path)
        {
            var splitstring = path.Split("\\");
            var classe = splitstring.Last().Split(".").First();
            return classe;
        }
    }
}