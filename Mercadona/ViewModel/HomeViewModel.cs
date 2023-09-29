using Mercadona.Models;

namespace Mercadona.ViewModel
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public Dictionary<int, decimal> NewPrices { get; set; }
        public List<Category> Categories { get; set; }
    }
}
