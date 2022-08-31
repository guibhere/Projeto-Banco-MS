using Confluent.Kafka;
using Integrador.Helper.Interface;
using Integrador.Models;

public class IntegradorService : IIntegradorService
{
    private readonly ISplunkLogger _splunk;
    private readonly IProducer<Null, string> _kafkaproducer;
    private readonly IConsumer<Null, string> _kafconsumer;

    public IntegradorService(ISplunkLogger splunk, IProducer<Null, string> producer, IConsumer<Null, string> consumer)
    {
        _splunk = splunk;
        _kafconsumer = consumer;
        _kafkaproducer = producer;
    }

    public void IniciarIntegracao()
    {
        _splunk.IniciarLog("Integrador Bacen", null);
        _splunk.LogarMensagem("Iniciando Consumidor Kafka");
        _splunk.EnviarLogAsync(new Response());

        _kafconsumer.Subscribe("Api_Controle_Transacao");

        while (true)
        {
            _splunk.IniciarLog("Topico: Api_Controle_Transacao", null);
            _splunk.LogarMensagem("Lendo Topico: Api_Controle_Transacao");
            var resp = _kafconsumer.Consume();
            _splunk.EnviarLogAsync(new Response("Mensagem Linda", "OK", 200, resp.Message));
            ProduzirMensagemBacem(resp);
            Console.WriteLine($"Consumed message '{resp.Value}' at: '{resp.TopicPartitionOffset}'.");
        }
    }

    public void LerMensagem()
    {
    }
    public async Task ProduzirMensagemBacem(dynamic Mensagem)
    {
        _splunk.IniciarLog("Topico: Bacem", null);
        _splunk.LogarMensagem("Postando mensagem : Bacem");
        await _kafkaproducer.ProduceAsync("Bacem",Mensagem.Message);
        _splunk.EnviarLogAsync(new Response("Mensagem Postada","OK",200,Mensagem.Message));

    }


}