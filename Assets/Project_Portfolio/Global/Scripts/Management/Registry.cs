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

        public void Deregister<T>(T p_registry)
            where T : class
        {
            _objects.Remove(p_registry);
        }
        
        public T Get<T>()
            where T : class
        {
            return (T)_objects.First(static obj => obj is T);
        }

        public IEnumerable<T> GetAll<T>()
            where T : class
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] is T)
                {
                    yield return (T)_objects[i];
                }
            }
        }
    }
}