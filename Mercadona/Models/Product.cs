using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mercadona.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public int? OfferId { get; set; }
        public virtual Offer? Offer { get; set; }

    }
}
