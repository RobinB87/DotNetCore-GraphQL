using System.ComponentModel.DataAnnotations;

namespace CarvedRock.Api.Data.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [StringLength(200), Required]
        public int Title { get; set; }
        public int Review { get; set; }
    }
}