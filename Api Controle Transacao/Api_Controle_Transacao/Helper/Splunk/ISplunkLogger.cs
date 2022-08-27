using Api_Controle_Transacao.Models;

namespace Api_Controle_Transacao.Helper.Interface
{
    public interface ISplunkLogger
    {
        public Task EnviarLogAsync(Response response);
        public string LogarMensagem(string msg);
        public void IniciarLog(string rota,object objeto);
    }
}