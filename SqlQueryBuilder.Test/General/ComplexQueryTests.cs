using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.General
{
    [TestClass]
    public class ComplexQueryTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void ComplexQuery_WhenUsingMultipleSearchesAndSortings_ThenDataQueryContainsCorrectSqlQuery()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column1", "Column2")
                .Select("Column3")
                .From("Table")
                .SearchTextToBeLike("Column1", "123")
                .Search("Column2", (int?)123)
                .SearchColumnDoBeWithinDatePeriod("Column3", new DateTime(2000, 10, 10), new DateTime(2020, 10, 10))
                .SortBy("Column1")
                .SortBy("Column2", false)
                .BuildPagedQuery<Data>(new SearchCriteria { PageSize = 10, PageNumber = 3 });

            query.DataQuery.ShouldBe("SELECT Column1,Column2,Column3 FROM Table WHERE Column1 LIKE @p1 AND Column2 = @p2 AND Column3 >= @p3 AND Column3 < @p4 ORDER BY Column1 ASC,Column2 DESC OFFSET 20 ROWS FETCH NEXT 10 ROWS ONLY");
        }

        [TestMethod]
        public void ComplexQuery_WhenUsingMultipleSearchesAndSortings_ThenCountQueryContainsCorrectSqlQuery()
        {
            PagedQuery<Data> query = GetQueryBuilder()
                .Select("Column1", "Column2", "Column3")
                .From("Table")
                .SearchTextToBeLike("Column1", "123")
                .Search("Column2", (int?)123)
                .SearchColumnDoBeWithinDatePeriod("Column3", new DateTime(2000, 10, 10), new DateTime(2020, 10, 10))
                .SortBy("Column1")
                .SortBy("Column2", false)
                .BuildPagedQuery<Data>(new SearchCriteria { PageSize = 10, PageNumber = 3 });

            query.CountQuery.ShouldBe("SELECT COUNT(*) FROM Table WHERE Column1 LIKE @p1 AND Column2 = @p2 AND Column3 >= @p3 AND Column3 < @p4");
        }

        [TestMethod]
        public void ComplexQuery_WhenReusingBuilder_ThenPropertiesAreNotCarriedOverToNextQuery()
        {
            var queryBuilder = GetQueryBuilder();

            PagedQuery<Data> query1 = queryBuilder
                .Select("Column1", "Column2")
                .SearchTextToBeEqual("Column1", "X")
                .From("Table")
                .SortBy("Column1")
                .BuildPagedQuery<Data>(new SearchCriteria { PageNumber = 1, PageSize = 10 });

            query1.DataQuery.ShouldBe("SELECT Column1,Column2 FROM Table WHERE Column1 = @p1 ORDER BY Column1 ASC OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY");
            query1.CountQuery.ShouldBe("SELECT COUNT(*) FROM Table WHERE Column1 = @p1");

            PagedQuery<Data> query2 = queryBuilder
                .Select("ColumnA")
                .From("TableA")
                .SortBy("ColumnA")
                .BuildPagedQuery<Data>(new SearchCriteria { PageNumber = 3, PageSize = 20 });

            query2.DataQuery.ShouldBe("SELECT ColumnA FROM TableA ORDER BY ColumnA ASC OFFSET 40 ROWS FETCH NEXT 20 ROWS ONLY");
            query2.CountQuery.ShouldBe("SELECT COUNT(*) FROM TableA");
        }
    }
}