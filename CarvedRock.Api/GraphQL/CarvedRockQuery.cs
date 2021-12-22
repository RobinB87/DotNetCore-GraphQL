using CarvedRock.Api.GraphQL.Types;
using CarvedRock.Api.Repositories;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockQuery : ObjectGraphType
    {
        // GraphQL.NET takes care of the awaiting, so you do not have to do that
        // It also takes care of the conversion of your entity type to the GraphQL type

        // Can use Postman:
        // POST https://localhost:5001/graphql 
        // In the body use e.g.: { "query": "{ products { id name price rating photoFileName } }" }
        // The GraphQL playground will give give nice intellisense though
        // You can also use { results: products { name photo: photoFileName } } to change names (hence make consumption more easy)
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
            //
            // Compare two products by using aliases { p10: product(id: 10) { name, price } p11: product(id: 11) { name, price } } 
            // Declare fragments to reduce typing queries, e.g.: fragment comparisonFields on ProductType { name, price, stock }
            //  and then { p10: product(id: 10) { ...comparisonFields } p11: product(id: 11) { ...comparisonFields } }
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
