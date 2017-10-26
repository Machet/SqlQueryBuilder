using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class PagedQuery_SelectingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void PagedQuerySelecting_WhenColumnsAreSpecified_ThenThoseAppearWithinDataQuery()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column1", "Column2")
                .Select("Column3", "Column4")
                .From("Table")
                .SortBy("Column1")
                .BuildPagedQuery<Data>(new SearchCriteria());

            query.DataQuery.ShouldContain("SELECT Column1,Column2,Column3,Column4 ");
        }

        [TestMethod]
        public void PagedQuerySelecting_WhenColumnsAreSpecified_ThenThoseDontAppearWithinDataQuery()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column1", "Column2")
                .Select("Column3", "Column4")
                .From("Table")
                .SortBy("Column1")
                .BuildPagedQuery<Data>(new SearchCriteria());

            query.CountQuery.ShouldContain("SELECT COUNT(*) ");
        }

        [TestMethod]
        public void PagedQuerySelecting_WhenSelectCriteriaNotSpecified_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentException>(() => GetQueryBuilder().BuildPagedQuery<Data>(new SearchCriteria()))
                .Message.ShouldContain("columns should have at least one element");
        }
    }
}
