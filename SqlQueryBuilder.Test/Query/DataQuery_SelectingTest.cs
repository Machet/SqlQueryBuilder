using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.Query
{
    [TestClass]
    public class DataQuery_SelectingTest : QueryBuilderTestBase
    {
        [TestMethod]
        public void DataQuerySelecting_WhenColumnsAreSpecifiedForPagedQuery_ThenThoseAppearWithinDataQuery()
        {
            DataQuery<Data> query = GetQueryBuilder()
                .Select("Column1", "Column2")
                .Select("Column3", "Column4")
                .From("Table")
                .BuildQuery<Data>();

            query.SelectQuery.ShouldContain("SELECT Column1,Column2,Column3,Column4 ");
        }

        [TestMethod]
        public void DataQuerySelecting_WhenSelectCriteriaIsNotSpecified_ThenExceptionIsThrownWhileBuilding()
        {
            Should.Throw<ArgumentException>(() => GetQueryBuilder().BuildQuery<Data>())
                .Message.ShouldContain("columns should have at least one element");
        }
    }
}
