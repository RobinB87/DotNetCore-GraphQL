using CarvedRock.Web.Models;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using System;
using System.Threading.Tasks;

namespace CarvedRock.Web.Clients
{
    public class ProductGraphClient
    {
        [Obsolete]
        private readonly GraphQLClient _client;

        [Obsolete]
        public ProductGraphClient(GraphQLClient client)
        {
            _client = client;
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var query = new GraphQLRequest
            {
                Query = @" 
                query productQuery($productId: ID!)
                { product(id: $productId) 
                    { id name price rating photoFileName description stock introducedAt 
                      reviews { title review }
                    }
                }",
                Variables = new { productId = id }
            };
            var response = await _client.PostAsync(query);
            return response.GetDataFieldAs<ProductModel>("product");
        }

        public async Task<ProductReviewModel> AddReview(ProductReviewInputModel review)
        {
            var query = new GraphQLRequest
            {
                Query = @" 
                mutation($review: reviewInput!)
                {
                    createReview(review: $review)
                    {
                        id
                    }
                }",
                Variables = new { review }
            };
            var response = await _client.PostAsync(query);
            return response.GetDataFieldAs<ProductReviewModel>("createReview");
        }

        /// <summary>
        /// Sends subscription query to the api
        /// This builds the websockets connection to the api
        /// When a new message comes in, the OnReceive delegate is triggered,
        /// which will get a GraphQLResponse from the receive method
        /// </summary>
        /// <returns></returns>
        public async Task SubscribeToUpdates()
        {
            var result = await _client.SendSubscribeAsync("subscription { reviewAdded { title productId } }");
            result.OnReceive += Receive;
        }

        /// <summary>
        /// Use the data property to get the message data by defining the node it's under ("reviewAdded")
        /// </summary>
        /// <param name="resp"></param>
        private void Receive(GraphQLResponse resp)
        {
            var review = resp.Data["reviewAdded"];
        }
    }
}