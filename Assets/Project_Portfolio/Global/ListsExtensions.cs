using System.Collections.Generic;

namespace ProjectPortfolio.Global
{
    public static class ListsExtensions
    {
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> p_collection)
        {
            return p_collection.Count == 0;
        }
    }
}