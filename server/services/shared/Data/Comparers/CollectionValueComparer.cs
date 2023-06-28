using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace shared.Data.Comparers
{
    public class CollectionValueComparer<T> : ValueComparer<ICollection<T>>
    {
        public CollectionValueComparer() : base((c1, c2) => (c1 ?? new List<T>()).SequenceEqual(c2 ?? new List<T>()),
            c => c.Aggregate(0, (a, v) => v != null ? HashCode.Combine(a, v.GetHashCode()) : a), c => c.ToHashSet())
        {
        }
    }
}
