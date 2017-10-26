using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.DataQuery
{
    [TestClass]
    public class DataQuery_GroupingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void DataQueryGrouping_WhenGroupingIsSpecified_ThenGroupByClauseExistInQuery()
        {
            DataQuery<Data> query = GetQueryBuilder()
                 .Select("Column")
                 .From("Table")
                 .GroupBy("Column1", "Column2")
                 .GroupBy("Column3", "Column4")
                 .BuildQuery<Data>();

            query.SelectQuery.ShouldContain("GROUP BY Column1,Column2,Column3,Column4");
        }
    }
}
