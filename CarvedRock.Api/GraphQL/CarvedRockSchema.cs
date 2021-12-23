using GraphQL;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockSchema: Schema
    {
        // Configure query by using DI
        public CarvedRockSchema(IDependencyResolver resolver): base(resolver)
        {           
            Query = resolver.Resolve<CarvedRockQuery>();
            Mutation = resolver.Resolve<CarvedRockMutation>();
        }
    }
}
