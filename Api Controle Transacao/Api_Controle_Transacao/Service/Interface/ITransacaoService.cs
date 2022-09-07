namespace Api_Controle_Transacao.Service.Interface
{
    public interface ITrasacaoService
    {
        public Task<dynamic> ProcessarTransacao(TransacaoInputPostDTO input);
        public Task<dynamic> ConsultarTransacoesDate(TransacaoInputGetDateDTO input, string agencia, string conta, char digito);
        public Task<dynamic> ConsultarTransacoesCache();
        public Task<dynamic> ConsultarTransacoesCpf(string cpf, DateTime datainicio, DateTime datafinal);
    }
}