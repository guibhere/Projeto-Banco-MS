namespace Api_Conta_Cliente.Service.Interface
{
    public interface ITipoContaService
    {
        Task<dynamic> CadastrarTipoConta(TipoContaInputPostDTO input);
        Task<dynamic> AtualizarTipoConta(TipoContaInputPostDTO input);
        Task<dynamic> ConsultarTipoContas();
        Task<dynamic> ConsultarTipoConta(int codigo_tipo_conta);
        Task<dynamic> ExcluirTipoConta(int codigo_tipo_conta);
    }
}