using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace shared.dal.Converters
{
    public class DictionaryStringValueConverter<T> : ValueConverter<Dictionary<string, T>, string>
    {
        public DictionaryStringValueConverter() : base(
            v => JsonConvert
                .SerializeObject(v.Select(e => e.ToString()).ToList()),
            v => JsonConvert
                .DeserializeObject<Dictionary<string, T>>(v) ?? new Dictionary<string, T>()
                )
        {
        }
    }
}
