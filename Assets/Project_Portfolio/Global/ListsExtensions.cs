using System.Collections.Generic;

namespace Project_Portfolio.Global
{
    public static class ListsExtensions
    {
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> p_collection)
        {
            return p_collection.Count == 0;
        }
    }
}