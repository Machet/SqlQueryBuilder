using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class PagedQuery_GroupingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void PagedQueryGrouping_WhenGroupingIsSpecified_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentException>(() => 
            {
                GetQueryBuilder()
                    .Select("Column")
                    .From("Table")
                    .SortBy("Column")
                    .GroupBy("Column")
                    .BuildPagedQuery<Data>(new SearchCriteria());
            }).Message.ShouldContain("Could not page grouped query");
        }
    }
}
