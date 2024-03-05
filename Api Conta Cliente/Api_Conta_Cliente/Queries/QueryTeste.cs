using Api_Conta_Cliente.Models;
using Api_Conta_Cliente.Models.Types;
using Api_Conta_Cliente.Service.Interface;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace Api_Conta_Cliente.Queries
{
    public class QueryTeste : ObjectGraphType
    {
        [Obsolete]
        public QueryTeste()
        {
            Field<ListGraphType<TesteType>>(
            "testes", resolve: context =>
            {
                var db = context.RequestServices.GetRequiredService<ApplicationDbContext>();
                return db.Contas
                .Select(c => new Teste()
                {
                    Numero_Agencia = c.Numero_Agencia,
                    Numero_Conta = c.Numero_Conta,
                    Cpf = c.Cpf
                });
            });
        }
    }
}