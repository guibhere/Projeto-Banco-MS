namespace Api_Controle_Transacao.Service.Interface
{
    public interface ITrasacaoService
    {
        public dynamic ProcessarTransacao(TransacaoInputPostDTO input);
    }
}