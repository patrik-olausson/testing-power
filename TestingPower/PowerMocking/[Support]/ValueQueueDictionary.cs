using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TestingPower.PowerMocking
{
    public class ValueQueueDictionary<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Queue<TValue>> _valueQueues = new ConcurrentDictionary<TKey, Queue<TValue>>();
        
        public void AddValue(TKey key, TValue value)
        {
            var queue = GetOrCreateQueue(key);
            queue.Enqueue(value);
        }

        public TValue GetValue(TKey key)
        {
            var queue = GetOrCreateQueue(key);
            
            if (queue.Count > 0)
            {
                return queue.Dequeue();
            }
            
            return default!;
        }
        
        private Queue<TValue> GetOrCreateQueue(TKey key) =>
            _valueQueues.AddOrUpdate(
                key,
                _ => new Queue<TValue>(),
                (_, queue) => queue);
    }
}