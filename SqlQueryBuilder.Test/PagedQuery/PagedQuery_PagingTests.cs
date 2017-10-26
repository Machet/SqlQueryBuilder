using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class PagedQuery_PagingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void PagedQueryPaging_WhenNegativePageIsSpecified_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentException>(() =>
            {
                GetQueryBuilder()
                    .Select("Column1")
                    .From("Table")
                    .SortBy("Column1")
                    .BuildPagedQuery<Data>(new SearchCriteria { PageSize = 20, PageNumber = -10 });
            }).Message.ShouldContain("should be greater than 0");

            Should.Throw<ArgumentException>(() =>
            {
                GetQueryBuilder()
                    .Select("Column1")
                    .From("Table")
                    .SortBy("Column1")
                    .BuildPagedQuery<Data>(new SearchCriteria { PageSize = 20, PageNumber = 0 });
            }).Message.ShouldContain("should be greater than 0");
        }

        [TestMethod]
        public void PagedQueryPaging_WhenNegativePageSizeIsSpecified_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentException>(() =>
            {
                GetQueryBuilder()
                    .Select("Column1")
                    .From("Table")
                    .SortBy("Column1")
                    .BuildPagedQuery<Data>(new SearchCriteria { PageSize = -20, PageNumber = 1 });
            }).Message.ShouldContain("should be greater than 0");

            Should.Throw<ArgumentException>(() =>
            {
                GetQueryBuilder()
                    .Select("Column1")
                    .From("Table")
                    .SortBy("Column1")
                    .BuildPagedQuery<Data>(new SearchCriteria { PageSize = 0, PageNumber = 1 });
            }).Message.ShouldContain("should be greater than 0");
        }

        [TestMethod]
        public void PagedQueryPaging_WhenPagingIsSpecified_ThenDataQueryContainsOffsetClauseAndCountQueryDoesnt()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column1")
                .From("Table")
                .SortBy("Column1")
                .BuildPagedQuery<Data>(new SearchCriteria { PageSize = 20, PageNumber = 1 });

            Assert.IsTrue(query.DataQuery.Contains("OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY"));
            Assert.IsTrue(!query.CountQuery.Contains("OFFSET"));

            PagedQuery<Data> query2 = GetQueryBuilder()
                .Select("Column1")
                .From("Table")
                .SortBy("Column1")
                .BuildPagedQuery<Data>(new SearchCriteria { PageSize = 1, PageNumber = 20 });

            query2.DataQuery.ShouldContain("OFFSET 19 ROWS FETCH NEXT 1 ROWS ONLY");
            query2.CountQuery.ShouldNotContain("OFFSET");
        }        
    }
}
