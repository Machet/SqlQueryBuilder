using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class DataQuery_FromTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void DataQueryFrom_WhenTableIsSpecified_ThenQueryHaveFromClauseWithThatTable()
        {
            DataQuery<Data> query = GetQueryBuilder()
                .Select("Column1")
                .From("ATable")
                .BuildQuery<Data>();

            query.SelectQuery.ShouldContain(" FROM ATable");
        }

        [TestMethod]
        public void DataQueryFrom_WhenTableNotSpecified_ThenExceptionIsThrown()
        {
            Should.Throw<InvalidOperationException>(() => GetQueryBuilder().Select("Column").BuildQuery<Data>())
                .Message.ShouldContain("table is not specified");
        }
    }
}
