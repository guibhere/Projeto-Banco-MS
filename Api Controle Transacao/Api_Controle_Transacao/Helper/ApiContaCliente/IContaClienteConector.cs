namespace Api_Controle_Transacao.Helper.Interface
{
    public interface IContaClienteConector
    {
        public ContaClienteSaldoDTO ConsultarSaldoConta(TransacaoInputPostDTO input);
        public ContaClienteSaldoDTO DepositarSaldoConta(TransacaoInputPostDTO input);
        public ContaClienteSaldoDTO ExtrairSaldoConta(TransacaoInputPostDTO input);
        public List<ContaClienteListaContasDTO> ConsultarContasCliente(string cpf);
    }
}