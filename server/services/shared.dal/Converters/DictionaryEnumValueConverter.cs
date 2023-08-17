using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace shared.dal.Converters
{
    public class DictionaryEnumValueConverter<T, U> : ValueConverter<Dictionary<T, U>, string> where T : Enum
    {
        public DictionaryEnumValueConverter() : base(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<Dictionary<T, U>>(v) ?? new Dictionary<T, U>()
                )
        {
        }
    }
}
