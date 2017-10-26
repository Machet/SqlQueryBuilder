using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.Query
{
    [TestClass]
    public class DataQuery_SortingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void DataQuerySorting_WhenSortingNotSpecified_ThenOrderClauseDoesntExistInQuery()
        {
            DataQuery<Data> query = GetQueryBuilder()
                .Select("Column")
                .From("Table")
                .BuildQuery<Data>();

            query.SelectQuery.ShouldNotContain("ORDER BY");
        }

        [TestMethod]
        public void DataQuerySorting_WhenSortingIsSpecified_ThenQueryContainsOrderAscendingClause()
        {
            DataQuery<Data> query = GetQueryBuilder()
                .Select("Column")
                .From("Table")
                .SortBy("Column")
                .BuildQuery<Data>();

            query.SelectQuery.ShouldContain("ORDER BY Column ASC");
        }

        [TestMethod]
        public void DataQuerySorting_WhenDescendingSortingIsSpecified_ThenQueryContainsOrderDescendingClause()
        {
            DataQuery<Data> query = GetQueryBuilder()
                .Select("Column")
                .From("Table")
                .SortBy("Column", ascending: false)
                .BuildQuery<Data>();

            query.SelectQuery.ShouldContain("ORDER BY Column DESC");
        }

        [TestMethod]
        public void DataQuerySorting_SortingIsSpecifiedMultipleTimes_ThenQueryContainsMultipleColumnsInOrderClause()
        {
            DataQuery<Data> query = GetQueryBuilder()
                .Select("Column1", "Column2", "Column3")
                .From("Table")
                .SortBy("Column1")
                .SortBy("Column2", ascending: false)
                .SortBy("Column3")
                .BuildQuery<Data>();

            query.SelectQuery.ShouldContain("ORDER BY Column1 ASC,Column2 DESC,Column3 ASC");
        }
    }
}
