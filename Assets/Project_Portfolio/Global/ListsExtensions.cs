using System.Collections.Generic;

namespace ProjectPortfolio.Global
{
    public static class ListsExtensions
    {
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> p_collection)
        {
            return p_collection.Count == 0;
        }
        
        public static T TryGetAt<T>(this IReadOnlyList<T> p_collection, int p_index, T p_default = default)
        {
            if (p_index < 0 || p_index >= p_collection.Count)
            {
                return p_default;
            }
            
            return p_collection[p_index];
        }
    }
}