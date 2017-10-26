using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class PagedQuery_FromTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void PagedQueryFrom_WhenTableIsSpecified_ThenDataAndCountQueryHaveFromClauseWithThatTable()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column1")
                .From("ATable")
                .SortBy("Column1")
                .BuildPagedQuery<Data>(new SearchCriteria());

            query.DataQuery.ShouldContain(" FROM ATable");
            query.CountQuery.ShouldContain(" FROM ATable");
        }

        [TestMethod]
        public void PagedQueryFrom_WhenTableNotSpecified_ThenExceptionIsThrown()
        {
            Should.Throw<InvalidOperationException>(() => GetQueryBuilder().Select("Column").BuildPagedQuery<Data>(new SearchCriteria()))
                .Message.ShouldContain("table is not specified");
        }
    }
}
