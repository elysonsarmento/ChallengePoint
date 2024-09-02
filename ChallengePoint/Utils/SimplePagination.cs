namespace ChallengePoint.Utils
{
    public class SimplePagination<T>
    {
        public int PageNumber { get; private set; }
        public int PageQuantity { get; private set; }
        public List<T> Items { get; private set; }

        public SimplePagination(IEnumerable<T> source, int pageNumber, int pageQuantity)
        {
            PageNumber = pageNumber;
            PageQuantity = pageQuantity;
            Items = source.Skip((pageNumber - 1) * pageQuantity).Take(pageQuantity).ToList();
        }

        public static SimplePagination<T> Create(IEnumerable<T> source, int pageNumber, int pageQuantity)
        {
            return new SimplePagination<T>(source, pageNumber, pageQuantity);
        }
    }
}
