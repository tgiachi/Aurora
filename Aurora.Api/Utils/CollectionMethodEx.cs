﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Api.Utils
{
    public static class CollectionMethodEx
    {
        public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<IEnumerable<T1>>> func)
        {
            return (await Task.WhenAll(enumeration.Select(func))).SelectMany(s => s);
        }
    }
}
