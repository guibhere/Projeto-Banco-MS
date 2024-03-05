using Api_Conta_Cliente.Helper.Interface;
using Api_Conta_Cliente.Queries;
using Api_Conta_Cliente.Service.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class QueryController : ControllerBase
{
    private readonly IGraphQLService _graphserv;
    private readonly ISplunkLogger _splunk;
    public QueryController(IGraphQLService graphserv, ISplunkLogger splunk)
    {
        _graphserv = graphserv;
        _splunk = splunk;
    }
    [HttpPost("Query")]
    public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
    {
        _splunk.IniciarLog(ControllerContext.HttpContext.Request.Path.Value, query);

        var result = await _graphserv.ExecuteQuery(query);
        //_splunk.EnviarLogAsync(result);

        return Ok(result);
    }
}
