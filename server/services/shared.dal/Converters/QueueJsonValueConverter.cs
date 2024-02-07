using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace shared.dal.Converters
{
    public class QueueJsonValueConverter<T> : ValueConverter<Queue<T>, string> where T : notnull
    {
        public QueueJsonValueConverter() : base(
            v => SerializeQueue(v),
            v => DeserializeQueue(v))
        {
        }

        private static string SerializeQueue(Queue<T> queue)
        {
            return JsonConvert.SerializeObject(queue.Select(e => JsonConvert.SerializeObject(e)).ToList());
        }

        private static Queue<T> DeserializeQueue(string value)
        {
            var list = JsonConvert.DeserializeObject<ICollection<string>>(value) ?? new List<string>();
            var queue = new Queue<T>();
            foreach (var item in list)
            {
                var itemDeserialized = JsonConvert.DeserializeObject<T>(item);
                if (itemDeserialized != null)
                {
                    queue.Enqueue(itemDeserialized);
                }
            }
            return queue;
        }
    }
}
