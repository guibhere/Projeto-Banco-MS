using Integrador.Models;

namespace Integrador.Helper.Interface
{
    public interface ISplunkLogger
    {
        public Task EnviarLogAsync(Response response);
        public string LogarMensagem(string msg);
        public void IniciarLog(string rota,object objeto);
        public LogModel Log{get;set;}
    }
}