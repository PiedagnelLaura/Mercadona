using Bogus;
using Mercadona.Models;

namespace Mercadona.Bogus
{

    public class OfferFaker : Faker<Offer>
    {
        public OfferFaker()
        {
            RuleFor(x => x.StartDate, x => x.Date.Past());
            RuleFor(x => x.EndDate, x => x.Date.Future());
        }
    }
}
