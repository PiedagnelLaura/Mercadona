using Bogus;
using Mercadona.Models;

namespace Mercadona.Bogus
{

    public class ProductFaker : Faker<Product>
    {
        public ProductFaker()
        {
            RuleFor(x => x.Name, x => x.Commerce.ProductName());
            RuleFor(x => x.Description, x => x.Commerce.ProductDescription());
            RuleFor(x => x.Price, x => decimal.Parse(x.Commerce.Price()));
            RuleFor(x => x.Picture, x => x.Image.PicsumUrl());
        }
    }
}
