namespace Api_Conta_Cliente.Service.Interface
{
    public interface IClienteService
    {

        Task<dynamic> CadastrarClienteAsync(ClienteInputPostDTO input);
        public dynamic CadastrarCliente(ClienteInputPostDTO input);
    }
}