using CarvedRock.Api.GraphQL.Types;
using CarvedRock.Api.Repositories;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockQuery : ObjectGraphType
    {
        // GraphQL.NET takes care of the awaiting, so you do not have to do that
        // It also takes care of the conversion of your entity type to the GraphQL type
        public CarvedRockQuery(ProductRepository productRepository)
        {
            // List
            Field<ListGraphType<ProductType>>(
                "products",
                resolve: context => productRepository.GetAll()
            );

            // One
            // Field name is "product"
            // Use arguments via QueryArguments - here of the IdGraphType -> expects id of product
            // You can use any graph type you want as arg
            // Express that it is required by NonNullGraphType
            //
            // Query ex: { product(id:10) { name } } 
            Field<ProductType>(
                "product",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>>
                { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return productRepository.GetOne(id);
                }
            );
        }
    }
}
