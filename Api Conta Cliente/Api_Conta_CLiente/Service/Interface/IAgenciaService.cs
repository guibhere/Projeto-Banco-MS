namespace Api_Conta_Cliente.Service.Interface
{
    public interface IAgenciaService
    {
        Task<dynamic> CadastrarAgencia(AgenciaInputPostDTO input);
        Task<dynamic> AtualizarAgencia(AgenciaInputPostDTO input);
        Task<dynamic> ConsultarAgencias();
        Task<dynamic> ConsultarAgencia(string numero_Agencia);
        Task<dynamic> ExcluirAgencia(string numero_Agencia);
    }
}