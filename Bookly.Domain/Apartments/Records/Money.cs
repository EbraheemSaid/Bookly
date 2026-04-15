namespace Bookly.Domain.Apartments.Records
{
    public record Money(decimal amount, Currency currency)
    {
        public static Money operator +(Money first, Money second)
        {

            if (first.currency != second.currency)
            {
                throw new ApplicationException("Currencies have to be the same.");
            }

            return new Money(first.amount + second.amount, first.currency);
        }

        public static Money Zero() => new(0, Currency.None);
    }
}

