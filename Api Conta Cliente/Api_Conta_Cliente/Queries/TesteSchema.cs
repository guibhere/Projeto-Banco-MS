using GraphQL.Types;

namespace Api_Conta_Cliente.Queries
{

    public class TesteSchema : Schema
    {
        public TesteSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<QueryTeste>();
        }
    }
}