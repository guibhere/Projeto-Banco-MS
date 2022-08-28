using Amazon.DynamoDBv2.DataModel;

namespace Api_Controle_Transacao.Models
{
    [DynamoDBTable("Transacoes")]
    public class Transacao
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBProperty]
        public string Numero_Conta_Origem { get; set; }
        [DynamoDBProperty]
        public string Numero_Agencia_Origem { get; set; }
        [DynamoDBProperty]
        public char Numero_Digito_Origem { get; set; }
        [DynamoDBProperty]
        public string Numero_Conta_Destino { get; set; }
        [DynamoDBProperty]
        public string Numero_Agencia_Destino { get; set; }
        [DynamoDBProperty]
        public char Numero_Digito_Destino { get; set; }
        [DynamoDBProperty]
        public decimal Valor_Transacao { get; set; }
        [DynamoDBRangeKey]
        public DateTime Data_Transacao { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string HashIndexOrigem{get;set;}
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string HashIndexDestino{get;set;}
        public Transacao(string numero_Conta_Origem, string numero_Agencia_Origem, char numero_Digito_Origem, string numero_Conta_Destino, string numero_Agencia_Destino, char numero_Digito_Destino, decimal valor_Transacao, DateTime data_Transacao)
        {
            Id = System.Guid.NewGuid().ToString();
            Numero_Agencia_Origem = numero_Agencia_Origem;
            Numero_Conta_Origem = numero_Conta_Origem;
            Numero_Digito_Origem = numero_Digito_Origem;
            Numero_Agencia_Destino = numero_Agencia_Destino;
            Numero_Conta_Destino = numero_Conta_Destino;
            Numero_Digito_Destino = numero_Digito_Destino;
            Valor_Transacao = valor_Transacao;
            Data_Transacao = data_Transacao;
        }
        public Transacao() { }

    }

}
