using Api_Controle_Transacao.Helper.Interface;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;
namespace Api_Controle_Transacao.Helper
{
    public class ContaClienteConector : IContaClienteConector
    {
        private readonly IOptions<ContaClienteConfig> _options;
        private readonly ISplunkLogger _splunk;
        public ContaClienteConector(IOptions<ContaClienteConfig> options, ISplunkLogger splunk)
        {
            _options = options;
            _splunk = splunk;
        }
        public ContaClienteSaldoDTO ConsultarSaldoConta(TransacaoInputPostDTO input)
        {
            var url = _options.Value.BaseUrl;
            var options = new RestClientOptions(url) { RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true };
            var client = new RestClient(options);

            url += _options.Value.endpoints.ConsultarSaldo;
            var request = new RestRequest(url, Method.Get);

            request.AddParameter("agencia", input.Numero_Agencia_Origem, ParameterType.UrlSegment);
            request.AddParameter("conta", input.Numero_Conta_Origem, ParameterType.UrlSegment);
            request.AddParameter("digito", input.Numero_Digito_Origem, ParameterType.UrlSegment);

            request.AddHeader("Content-Type", "application/json");

            //var json = JsonSerializer.Serialize(this.Log);
            //request.AddBody(json, "application/json");
            RestResponse restresp = client.Execute(request);

            if (restresp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ContaClienteSaldoDTO resp = JsonSerializer.Deserialize<ContaClienteSaldoDTO>(restresp.Content);
                return resp;
            }
            else
            {
                throw new Exception(restresp.ErrorException.Message);
            }
        }
        public ContaClienteSaldoDTO DepositarSaldoConta(TransacaoInputPostDTO input)
        {
            var url = _options.Value.BaseUrl;
            var options = new RestClientOptions(url) { RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true };
            var client = new RestClient(options);

            url += _options.Value.endpoints.DepositarSaldo;
            var request = new RestRequest(url, Method.Patch);

            request.AddParameter("agencia", input.Numero_Agencia_Destino, ParameterType.UrlSegment);
            request.AddParameter("conta", input.Numero_Conta_Destino, ParameterType.UrlSegment);
            request.AddParameter("digito", input.Numero_Digito_Destino, ParameterType.UrlSegment);

            request.AddHeader("Content-Type", "application/json");

            request.AddBody(JsonSerializer.Serialize(new TransacaoValorDTO { valor = input.Valor_Transacao }), "application/json");

            RestResponse restresp = client.Execute(request);

            if (restresp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ContaClienteSaldoDTO resp = JsonSerializer.Deserialize<ContaClienteSaldoDTO>(restresp.Content);
                return resp;
            }
            else
            {
                throw new Exception(restresp.ErrorException.Message);
            }
        }
        public ContaClienteSaldoDTO ExtrairSaldoConta(TransacaoInputPostDTO input)
        {
            var url = _options.Value.BaseUrl;
            var options = new RestClientOptions(url) { RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true };
            var client = new RestClient(options);

            url += _options.Value.endpoints.RetirarSaldo;
            var request = new RestRequest(url, Method.Patch);

            request.AddParameter("agencia", input.Numero_Agencia_Origem, ParameterType.UrlSegment);
            request.AddParameter("conta", input.Numero_Conta_Origem, ParameterType.UrlSegment);
            request.AddParameter("digito", input.Numero_Digito_Origem, ParameterType.UrlSegment);

            request.AddHeader("Content-Type", "application/json");

            request.AddBody(JsonSerializer.Serialize(new TransacaoValorDTO { valor = input.Valor_Transacao }), "application/json");

            RestResponse restresp = client.Execute(request);

            if (restresp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ContaClienteSaldoDTO resp = JsonSerializer.Deserialize<ContaClienteSaldoDTO>(restresp.Content);
                return resp;
            }
            else
            {
                throw new Exception(restresp.ErrorException.Message);
            }
        }

        public List<ContaClienteListaContasDTO> ConsultarContasCliente(string cpf)
        {
            var url = _options.Value.BaseUrl;
            var options = new RestClientOptions(url) { RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true };
            var client = new RestClient(options);

            url += _options.Value.endpoints.ConsultarContasCpf;
            var request = new RestRequest(url, Method.Get);

            request.AddParameter("cpf", cpf, ParameterType.UrlSegment);

            request.AddHeader("Content-Type", "application/json");

            //var json = JsonSerializer.Serialize(this.Log);
            //request.AddBody(json, "application/json");
            RestResponse restresp = client.Execute(request);

            if (restresp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resp = JsonSerializer.Deserialize<List<ContaClienteListaContasDTO>>(restresp.Content);
                return resp;
            }
            else
            {
                throw new Exception(restresp.ErrorException.Message);
            }
        }
    }
}