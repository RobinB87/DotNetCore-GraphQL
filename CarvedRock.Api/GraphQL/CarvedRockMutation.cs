using CarvedRock.Api.Data.Entities;
using CarvedRock.Api.GraphQL.Types;
using CarvedRock.Api.Repositories;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockMutation : ObjectGraphType
    {
        public CarvedRockMutation(ProductReviewRepository reviewRepository)
        {
            // Field async instead of field
            // Use with query: mutation ($review: reviewInput!) { createReview(review:$review) { id title } }
            FieldAsync<ProductReviewType>(
                    "createReview",
                    arguments: new QueryArguments(
                        new QueryArgument<NonNullGraphType<ProductReviewInputType>> { Name = "review" }),
                    resolve: async context =>
                    {
                        var review = context.GetArgument<ProductReview>("review");

                    // TryAsyncResolve monitors the outcome of AddReview
                    //  If everything is allright, the review is returned
                    //  Else the exception is added to the errorlist that is returned to the client
                    return await context.TryAsyncResolve(
                            async c => await reviewRepository.AddReview(review));
                    });
        }
    }
}