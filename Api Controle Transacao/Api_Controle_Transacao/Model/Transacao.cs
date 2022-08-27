namespace Api_Controle_Transacao.Models
{
    public class Transacao
    {
        public long Id { get; set; }
        public string Numero_Conta_Origem { get; set; }
        public string Numero_Agencia_Origem { get; set; }
        public char Numero_Digito_Origem { get; set; }
        public string Numero_Conta_Destino { get; set; }
        public string Numero_Agencia_Destino { get; set; }
        public char Numero_Digito_Destino { get; set; }
        public decimal Valor_Transacao { get; set; }
        public DateTime Data_Transacao { get; set; }
        public Transacao(string numero_Conta_Origem, string numero_Agencia_Origem, char numero_Digito_Origem, string numero_Conta_Destino, string numero_Agencia_Destino, char numero_Digito_Destino, decimal valor_Transacao, DateTime data_Transacao)
        {
            Numero_Agencia_Origem = numero_Agencia_Origem;
            Numero_Conta_Origem = numero_Conta_Origem;
            Numero_Digito_Origem = numero_Digito_Origem;
            Numero_Agencia_Destino = numero_Agencia_Destino;
            Numero_Conta_Destino = numero_Conta_Destino;
            Numero_Digito_Destino = numero_Digito_Destino;
            Valor_Transacao = valor_Transacao;
            Data_Transacao = data_Transacao;
        }

    }

}
