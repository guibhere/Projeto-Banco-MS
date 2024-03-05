using System.Reflection;
using System.Text.Json;
using Api_Conta_Cliente.Helper.Interface;
using Api_Conta_Cliente.Models;
using Api_Conta_Cliente.Queries;
using Api_Conta_Cliente.Service.Interface;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class GraphQLService : IGraphQLService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ISplunkLogger _splunk;
    private readonly IContaService _conta;
    private readonly ISchema _schema;
    public GraphQLService(ApplicationDbContext context, IWebHostEnvironment environment, ISplunkLogger splunk, IContaService conta, ISchema schema)
    {
        _context = context;
        _environment = environment;
        _splunk = splunk;
        _conta = conta;
        _schema = schema;
    }

    public async Task<dynamic> ExecuteQuery(GraphQLQuery query)
    {
        _splunk.LogarMensagem("Iniciando :" + MethodBase.GetCurrentMethod().Name);

        var _documentExecuter = new DocumentExecuter();

        var executionOptions = new ExecutionOptions
        {
            Schema = _schema,
            Query = query.Query
        };
        var jsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var inputs = query.Variables.ToInputs(); 
        //executionOptions.SetInputs(inputs);
        var result = await _documentExecuter.ExecuteAsync(executionOptions);

        if (result.Errors?.Count > 0)
        {
            var listerro = new List<Exception>();
            foreach (dynamic error in result.Errors)
                listerro.Add(error);
            throw new AggregateException(listerro);
        }


        return result.Data;
    }
}