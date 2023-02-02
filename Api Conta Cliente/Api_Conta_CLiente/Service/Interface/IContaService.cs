namespace Api_Conta_Cliente.Service.Interface
{
    public interface IContaService
    {
        public dynamic CadastrarConta(ContaInputPostDTO input,string cpf);
        public dynamic ConsultarSaldo(string agencia,string conta,char digito);
        public dynamic DepositarSaldo(ContaInputPatchAtualizarSaldoDTO input,string agencia,string conta,char digito);
        public dynamic ExtrairSaldo(ContaInputPatchAtualizarSaldoDTO input,string agencia,string conta,char digito);
        public dynamic ConsultarConstasCpf(string cpf);
        public Task<dynamic> ConsultarContas();
    }
}