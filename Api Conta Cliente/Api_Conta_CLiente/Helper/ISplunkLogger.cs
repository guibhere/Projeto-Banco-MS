using System.Runtime.CompilerServices;
using Api_Conta_Cliente.Models;

namespace Api_Conta_Cliente.Helper.Interface
{
    public interface ISplunkLogger
    {
        public Task EnviarLogAsync(Response response);
        public void LogarMensagem(string msg, [CallerMemberName] string nomemetodo = "", [CallerLineNumber] int numerolinha = 0,[CallerFilePath] string path ="");
        public void IniciarLog(string rota, object objeto);
        public LogModel Log { get; set; }
    }
}