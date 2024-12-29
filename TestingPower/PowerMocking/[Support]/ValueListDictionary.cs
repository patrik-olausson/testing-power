using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TestingPower.PowerMocking
{
    public class ValueListDictionary<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, List<TValue>> _valueLists = new ConcurrentDictionary<TKey, List<TValue>>();
        
        public void AddValue(TKey key, TValue value)
        {
            var list = GetOrCreateList(key);
            list.Add(value);
        }

        public List<TValue> GetValues(TKey key) => GetOrCreateList(key);

        private List<TValue> GetOrCreateList(TKey key) =>
            _valueLists.AddOrUpdate(
                key,
                _ => new List<TValue>(),
                (_, list) => list);

        public IReadOnlyCollection<TValue> GetAllValues()
        {
            return _valueLists.Values.SelectMany(x => x).ToList();
        }
    }
}