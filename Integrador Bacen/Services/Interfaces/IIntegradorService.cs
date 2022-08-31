public interface IIntegradorService
{
    public void LerMensagem();
    public void IniciarIntegracao();
    public Task ProduzirMensagemBacem(dynamic Mensagem);
}