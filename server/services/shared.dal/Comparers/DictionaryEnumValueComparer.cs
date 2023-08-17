using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace shared.dal.Comparers
{
    public class DictionaryEnumValueComparer<T, U> : ValueComparer<Dictionary<T, U>> where T : Enum
    {
        public DictionaryEnumValueComparer() : base((c1, c2) => (c1 ?? new Dictionary<T, U>()).SequenceEqual(c2 ?? new Dictionary<T, U>()),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())))
        {
        }
    }
}
