namespace user.bll.Infrastructure.ViewModels
{
    public class EnumerableWithTotalViewModel<T>
    {
        public IEnumerable<T> Values { get; set; } = Enumerable.Empty<T>();

        public int Total { get; set; }
    }
}
