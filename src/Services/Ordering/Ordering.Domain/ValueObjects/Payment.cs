using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record Payment(string CardName, string CardNumber, string Expiration, string CVV, int PaymentMethod);
}
