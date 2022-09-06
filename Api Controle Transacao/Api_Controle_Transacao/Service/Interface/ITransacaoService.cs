namespace Api_Controle_Transacao.Service.Interface
{
    public interface ITrasacaoService
    {
        public Task<dynamic> ProcessarTransacao(TransacaoInputPostDTO input);
        public Task<dynamic> ConsultarTransacoesDate(TransacaoInputGetDateDTO input, string agencia, string conta, string digito);
        public Task<dynamic> ConsultarTransacoesCache();
    }
}