using System.Collections.Generic;
using System.Linq;

namespace ProjectPortfolio.Global
{
    public abstract class Registry
    {
        private readonly List<object> _objects = new(128);
        
        public void Register<T>(T p_object)
            where T : class
        {
            _objects.Add(p_object);
        }
        
        public T Get<T>()
            where T : class
        {
            return (T)_objects.First(static obj => obj is T);
        }
    }
}