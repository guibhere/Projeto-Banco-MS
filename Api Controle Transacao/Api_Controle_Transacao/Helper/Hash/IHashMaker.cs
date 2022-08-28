namespace Api_Controle_Transacao.Helper.Interface
{
    public interface IHashMaker
    {
        public  string HashString(string text, string salt = "");
    }

}