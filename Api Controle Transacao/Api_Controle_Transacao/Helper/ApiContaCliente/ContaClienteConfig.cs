namespace Api_Controle_Transacao.Helper
{
    public class ContaClienteConfig
    {
        public string BaseUrl { get; set; }
        public Endpoints endpoints{get;set;}

        public class Endpoints
        {
            public string ConsultarSaldo{get;set;}
            public string DepositarSaldo{get;set;}
            public string RetirarSaldo{get;set;}
            public string ConsultarContasCpf{get;set;}
        }
    }
}