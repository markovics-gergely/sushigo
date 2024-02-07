using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace shared.dal.Converters
{
    public class CollectionJsonValueConverter<T> : ValueConverter<ICollection<T>, string> where T : notnull
    {
        public CollectionJsonValueConverter() : base(
            v => JsonConvert
                .SerializeObject(v.Select(e => e.ToString()).ToList()),
            v => (JsonConvert
                .DeserializeObject<ICollection<T>>(v) ?? new List<T>()
                ).ToList())
        {
        }
    }
}
