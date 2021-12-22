using CarvedRock.Api.Data.Entities;
using CarvedRock.Api.Repositories;
using GraphQL.DataLoader;
using GraphQL.Types;
using System.Security.Claims;

namespace CarvedRock.Api.GraphQL.Types
{
    public class ProductType : ObjectGraphType<Product>
    {
        // In real world apps products can have all kinds of properties (e.g. kayak has a length and a boot a size)
        // Each product will then get its own type.
        // Then interfaces can be used (e.g. product interface) which would define the common props for all products (interface polymorphism). 
        public ProductType(ProductReviewRepository reviewRepository,
            IDataLoaderContextAccessor dataLoaderContextAccessor)
        {
            Field(t => t.Id);
            Field(t => t.Name).Description("The name of the product");
            Field(t => t.Description);
            Field(t => t.IntroducedAt).Description("When the product was first introduced in the catalog");
            Field(t => t.PhotoFileName).Description("The file name of the photo so the client can render it");
            Field(t => t.Price);
            Field(t => t.Rating).Description("The (max 5) star customer rating");
            Field(t => t.Stock);
            Field<ProductTypeEnumType>("Type", "The type of product");

            Field<ListGraphType<ProductReviewType>>(
                "reviews",
                resolve: context =>
                {
                    // Get the user and then you can do all kinds of authorization (e.g. check for an admin role claim)
                    var user = (ClaimsPrincipal)context.UserContext;

                    // Use dictionary to cache data (int, ProductReview)
                    var loader = dataLoaderContextAccessor.Context.GetOrAddCollectionBatchLoader<int, ProductReview>(
                        "GetReviewsByProductId", reviewRepository.GetForProducts);

                    // Use loader to return all reviews for the current product id
                    // When reviews for the next product are loaded, the existing data loader will be used containing the cached lookup object
                    //  When running the project you will now see just two queries in stead of many in the logging
                    return loader.LoadAsync(context.Source.Id);
                });
        }
    }
}