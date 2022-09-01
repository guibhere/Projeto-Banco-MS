public interface IIntegradorService
{
    public Task IniciarIntegracao();
    public Task ProduzirMensagemBacem(dynamic Mensagem);
}