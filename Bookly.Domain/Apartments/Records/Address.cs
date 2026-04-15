using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Domain.Apartments.Records
{
    public record Address(string Country, string State, string ZipCode, string City, string Street)
    {
    }
}
