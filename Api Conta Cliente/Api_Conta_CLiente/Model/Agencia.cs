namespace Api_Conta_Cliente.Models{
    public class Agencia
    {
        public string Numero_Agencia{get;set;}
        public string Descricao{get;set;}
        public List<Conta> Contas{get;set;}
        public Agencia(string numero_Agencia,string descricao)
        {
            Numero_Agencia = numero_Agencia;
            Descricao = descricao;
        }

    }
}

