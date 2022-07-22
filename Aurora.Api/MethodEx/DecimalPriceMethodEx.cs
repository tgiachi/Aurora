using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Api.MethodEx
{
    public static class DecimalPriceMethodEx
    {

        public static decimal ToPrice(this decimal price)
        {
            return Math.Round(price, 2);
        }
    }
}
