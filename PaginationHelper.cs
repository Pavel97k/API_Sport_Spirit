namespace API_Sport_Spirit
{
    public class PaginationHelper<T>
    {
        public IEnumerable<T> Paginate(IEnumerable<T> items, int pageNumber, int pageSize)
        {
            return items.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
