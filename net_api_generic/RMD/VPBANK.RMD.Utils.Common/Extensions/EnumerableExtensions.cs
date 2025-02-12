using System.Collections.Generic;
using System.Linq;

namespace VPBANK.RMD.Utils.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] target)
        {
            return source.Concat(target);
        }
    }
}
