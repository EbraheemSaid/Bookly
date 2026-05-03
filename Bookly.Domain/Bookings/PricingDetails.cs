using Bookly.Domain.Shared;

namespace Bookly.Domain.Bookings
{
    public record PricingDetails(
        Money PriceForPeriod,
        Money CleaningFee,
        Money AmenetiesUpCharge,
        Money TotalPrice)
    {

    }
}
