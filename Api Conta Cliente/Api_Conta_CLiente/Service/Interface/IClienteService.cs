namespace Api_Conta_Cliente.Service.Interface
{
    public interface IClienteService
    {
        Task<dynamic> CadastrarCliente(ClienteInputPostDTO input);
        Task<dynamic> ConsultarClientesLista();
        Task<dynamic> ConsultarCliente(string cpf);
        Task<dynamic> ExcluirCliente(string cpf);
        Task<dynamic> AtualizarCliente(ClienteInputPatchDTO input);
    }
}