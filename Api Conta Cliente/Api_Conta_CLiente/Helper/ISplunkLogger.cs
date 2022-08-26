namespace Api_Conta_Cliente.Helper.Interface
{
    public interface ISplunkLogger
    {
        public Task EnviarLogAsync(string msg);
        public string LogarMensagem(string msg);
        public void IniciarLog(string rota,object objeto);
    }
}