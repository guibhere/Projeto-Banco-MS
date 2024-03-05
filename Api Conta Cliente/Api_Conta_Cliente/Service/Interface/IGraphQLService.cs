using Api_Conta_Cliente.Queries;
namespace Api_Conta_Cliente.Service.Interface
{
    public interface IGraphQLService
    {
        Task<dynamic> ExecuteQuery(GraphQLQuery query);

    }
}