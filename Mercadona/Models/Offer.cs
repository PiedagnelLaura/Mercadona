using System.Text.Json.Serialization;

namespace Mercadona.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Discount { get; set; }

        public virtual List<Product>? Products { get; set; }
    }
}
