using System.Collections.Generic;
using System.Linq;

namespace SqlQueryBuilder
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
        {
            foreach (var v in values)
            {
                set.Add(v);
            }
        }

        public static string Join(this IEnumerable<string> values, string separator = ",")
        {
            return string.Join(separator, values.Where(v => !string.IsNullOrEmpty(v)));
        }
    }
}