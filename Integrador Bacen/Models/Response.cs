namespace Integrador.Models
{
    public class Response
    {
        public string Mensagem { get; set; }
        public string TipoRetorno { get; set; }
        public int CodigoRetoro { get; set; }
        public Object Dados { get; set; }
        public Response(){}
        public Response(string msg, string tipo, int cod, Object dados)
        {
            Mensagem = msg;
            TipoRetorno = tipo;
            CodigoRetoro = cod;
            Dados = dados;
        }
    }

}