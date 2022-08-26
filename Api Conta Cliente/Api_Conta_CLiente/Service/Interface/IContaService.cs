namespace Api_Conta_Cliente.Service.Interface
{
    public interface IContaService
    {
        public dynamic CadastrarConta(ContaInputPostDTO input,string cpf);
    }
}