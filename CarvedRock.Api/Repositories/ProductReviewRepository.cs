using CarvedRock.Api.Data;
using CarvedRock.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarvedRock.Api.Repositories
{
    public class ProductReviewRepository
    {
        private readonly CarvedRockDbContext _dbContext;

        public ProductReviewRepository(CarvedRockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductReview>> GetForProduct(int productId)
        {
            return await _dbContext.ProductReviews.Where(pr => pr.ProductId == productId).ToListAsync();
        }

        // ILookup: use when you need a 1:N map with values that are fixed and won't (and can't) change.
        // A dictionary on the other hand offers a mutable 1:1 mapping of key value pairs, so it can be updated to add or remove values.
        public async Task<ILookup<int, ProductReview>> GetForProducts(IEnumerable<int> productIds)
        {
            var reviews = await _dbContext.ProductReviews.Where(
                pr => productIds.Contains(pr.ProductId)).ToListAsync();

            // So, e.g. one product can have multiple reviews
            return reviews.ToLookup(r => r.ProductId);
        }
    }
}