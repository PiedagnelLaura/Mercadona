using Mercadona.Models;

namespace Mercadona.ViewModel
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public Dictionary<int, decimal> NewPrices { get; set; }
    }
}
