namespace Api_Conta_Cliente.Models
{
    public class Response
    {
        public string Mensagem { get; set; }
        public string TipoRetorno { get; set; }
        public int CodigoRetoro { get; set; }
        public Object Dados { get; set; }
        public Response(string msg, string tipo, int cod, Object dados)
        {
            Mensagem = msg;
            TipoRetorno = tipo;
            CodigoRetoro = cod;
            Dados = dados;
        }
    }

}