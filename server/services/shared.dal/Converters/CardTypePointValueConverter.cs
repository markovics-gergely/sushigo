using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using shared.dal.Models;

namespace shared.dal.Converters
{
    internal class CardTypePointValueConverter : ValueConverter<CardTypePoint, string>
    {
        public CardTypePointValueConverter() : base(
            v => v.ToString(),
            v => CardTypePoint.FromString(v)
        )
        {
        }
    }
}
