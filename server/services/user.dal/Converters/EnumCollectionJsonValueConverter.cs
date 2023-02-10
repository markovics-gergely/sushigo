using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace user.dal.Converters
{
    public class EnumCollectionJsonValueConverter<T> : ValueConverter<ICollection<T>, string> where T : Enum
    {
        public EnumCollectionJsonValueConverter() : base(
            v => JsonConvert
                .SerializeObject(v.Select(e => e.ToString()).ToList()),
            v => (JsonConvert
                .DeserializeObject<ICollection<string>>(v) ?? new List<string>()
                ).Select(e => (T)Enum.Parse(typeof(T), e)).ToList())
        {
        }
    }
}
