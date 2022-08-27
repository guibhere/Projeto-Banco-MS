namespace Api_Conta_Cliente.Models{
    public class TipoConta
    {
        public long Codigo_Conta{get;set;}
        public string Descricao{get;set;}
        public List<Conta> Contas{get;set;}
        public TipoConta(long codigo_Conta,string descricao)
        {
            Codigo_Conta = codigo_Conta;
            Descricao = descricao;
        }

    }
}

