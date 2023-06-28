using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace shared.dal.Comparers
{
    public class DictionaryValueComparer<T> : ValueComparer<Dictionary<string, T>>
    {
        public DictionaryValueComparer() : base((c1, c2) => (c1 ?? new Dictionary<string, T>()).SequenceEqual(c2 ?? new Dictionary<string, T>()),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())))
        {
        }
    }
}
