using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Api_Controle_Transacao.Helper.Interface;
using Api_Controle_Transacao.Models;
using Confluent.Kafka;
using Extensions;
using StackExchange.Redis;

public class TrasacaoRepository : ITrasacaoRepository
{
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    private readonly IDynamoDBContext _dynamocontext;
    private readonly IProducer<Null, string> _kafkaproducer;
    private readonly IConnectionMultiplexer _redis;
    public TrasacaoRepository(IWebHostEnvironment environment, ISplunkLogger splunk,
    IDynamoDBContext dynamo, IProducer<Null, string> producer, IConnectionMultiplexer redis)
    {
        _environment = environment;
        _splunk = splunk;
        _dynamocontext = dynamo;
        _kafkaproducer = producer;
        _redis = redis;
    }

    public async Task<dynamic> ConsultarContaCache(string id)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        _splunk.LogarMensagem("Consultando saldo em cache");
        return await _redis.GetDatabase().StringGetAsync(id);
    }

    public async Task<dynamic> InserirTransacaoCache(string id, string transacao)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        _splunk.LogarMensagem("Gravando transação em cache");
        var db = _redis.GetDatabase();
        var resp = await db.StringSetAsync(id, transacao, TimeSpan.FromSeconds(600));
        return resp;
    }

    public async Task<dynamic> InserirContaCache(string id, string transacao)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        _splunk.LogarMensagem("Gravando conta em cache");
        var db = _redis.GetDatabase();
        var resp = await db.StringSetAsync(id, transacao, TimeSpan.FromSeconds(600));
        return resp;
    }
    public async Task<dynamic> InserirTransacaoDB(Transacao trans)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        _splunk.LogarMensagem("Persistindo Transação");
        return _dynamocontext.SaveAsync(trans);
    }

    public async Task<dynamic> ProduzirTransacaoKafka(string topico, Message<Null, string> msg)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        _splunk.LogarMensagem("Produzindo mensagem KAFKA");
        var resp = await _kafkaproducer.ProduceAsync(topico, msg);
        return resp;
    }

    public async Task<dynamic> ConsultarTransacoesDB(string index, string indexname, string indexvalue)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        var queryFilter = new QueryFilter(index, QueryOperator.Equal, indexvalue);
        var queryConf = new QueryOperationConfig()
        {
            IndexName = indexname,
            Filter = queryFilter
        };
        return await _dynamocontext.FromQueryAsync<Transacao>(queryConf).GetRemainingAsync();
    }
    public async Task<dynamic> ConsultarKeysTransacoesCache()
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        var db = _redis.GetDatabase();
        var endPoint = _redis.GetEndPoints().First();
        var keys = _redis.GetServer(endPoint).Keys(pattern: "*").ToArray();

        return keys;
    }
    public async Task<dynamic> ConsultarTransacoesKeyCache(string key)
    {
        _splunk.LogarMensagem("Iniciando: " + this.GetType().Name + "." + MethodBase.GetCurrentMethod().GetDeclaringName());
        return await _redis.GetDatabase().StringGetAsync(key);
    }
}