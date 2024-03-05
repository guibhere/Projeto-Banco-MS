using Api_Conta_Cliente.Models;
using GraphQL.Types;
namespace Api_Conta_Cliente.Models.Types
{
    public class TesteType : ObjectGraphType<Teste>
    {
        public TesteType()
        {
            Name = "Teste";

            Field(x => x.Numero_Agencia, nullable: true).Description("Numero da agencia");

            Field(x => x.Numero_Conta, nullable: true).Description("Numero da Conta");

            Field(x => x.Cpf, nullable: true).Description("Cpf");

        }
    }
}