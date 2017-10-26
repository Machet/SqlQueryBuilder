using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class PagedQuery_SortingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void PagedQuerySorting_WhenSortingNotSpecified_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentException>(() =>
            {
                GetQueryBuilder()
                    .Select("Column")
                    .From("Table")
                    .BuildPagedQuery<Data>(new SearchCriteria());
            }).Message.ShouldContain("sortings should have at least one element");
        }

        [TestMethod]
        public void PagedQuerySorting_WhenSortingIsSpecified_ThenDataQueryContainsOrderAscendingClauseAndCountQueryDoesnt()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column")
                .From("Table")
                .SortBy("Column")
                .BuildPagedQuery<Data>(new SearchCriteria());

            query.DataQuery.ShouldContain("ORDER BY Column ASC");
            query.CountQuery.ShouldNotContain("ORDER BY");
        }

        [TestMethod]
        public void PagedQuerySorting_WhenDescendingSortingIsSpecified_ThenDataQueryContainsOrderDescendingClauseAndCountQueryDoesnt()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column")
                .From("Table")
                .SortBy("Column", ascending: false)
                .BuildPagedQuery<Data>(new SearchCriteria());

            query.DataQuery.ShouldContain("ORDER BY Column DESC");
            query.CountQuery.ShouldNotContain("ORDER BY");
        }

        [TestMethod]
        public void PagedQuerySorting_WhenSortingIsSpecifiedMultipleTimes_ThenDataQueryContainsMultipleColumnsInOrderClauseAndCountQueryDoesnt()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column1", "Column2", "Column3")
                .From("Table")
                .SortBy("Column1")
                .SortBy("Column2", ascending: false)
                .SortBy("Column3")
                .BuildPagedQuery<Data>(new SearchCriteria());

            query.DataQuery.ShouldContain("ORDER BY Column1 ASC,Column2 DESC,Column3 ASC");
            query.CountQuery.ShouldNotContain("ORDER BY");
        }
    }
}
