namespace QueryBuilder
{
	public class SearchCriteria
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
		public string OrderBy { get; set; }

        public SearchCriteria()
        {
            PageSize = 10;
            PageNumber = 1;
        }
	}
}
