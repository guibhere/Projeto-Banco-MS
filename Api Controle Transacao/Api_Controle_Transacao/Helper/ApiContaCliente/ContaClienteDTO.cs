namespace Api_Controle_Transacao.Helper
{
    public class ContaClienteSaldoDTO
    {
        public decimal saldo { get; set; }
    }
    public class TransacaoValorDTO
    {
        public decimal valor { get; set; }
    }
    public class ContaClienteListaContasDTO
    {
        public string agencia { get; set; }
        public string numeroConta { get; set; }
        public char digito { get; set; }
    }
}