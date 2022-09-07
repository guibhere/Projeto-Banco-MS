using Api_Controle_Transacao.Models;
using Confluent.Kafka;

public interface ITrasacaoRepository
{
    Task<dynamic> InserirTransacaoDB(Transacao trans);
    Task<dynamic> InserirTransacaoCache(string id, string transacao);
    Task<dynamic> InserirContaCache(string id, string conta);
    Task<dynamic> ProduzirTransacaoKafka(string topico, Message<Null, string> msg);
    Task<dynamic> ConsultarContaCache(string id);
    Task<dynamic> ConsultarTransacoesDB(string index, string indexname, string indexvalue);
    Task<dynamic> ConsultarTransacoesKeyCache(string key);
    Task<dynamic> ConsultarKeysTransacoesCache();
}