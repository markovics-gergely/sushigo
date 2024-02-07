using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace shared.dal.Comparers
{
    public class QueueValueComparer<T> : ValueComparer<Queue<T>>
    {
        public QueueValueComparer() : base((c1, c2) => (c1 ?? new Queue<T>()).SequenceEqual(c2 ?? new Queue<T>()),
            c => c.Aggregate(0, (a, v) => v != null ? HashCode.Combine(a, v.GetHashCode()) : a), c => c)
        {
        }
    }
}
