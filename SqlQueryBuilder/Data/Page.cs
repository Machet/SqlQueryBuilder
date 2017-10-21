namespace SqlQueryBuilder
{
    public class Page<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrderedBy { get; set; }
        public object Total { get; set; }
        public object Records { get; set; }
    }
}