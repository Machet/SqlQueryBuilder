using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class PagedQuery_FilteringTests : QueryBuilderTestBase
    {
        #region Searching by numeric types

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForGuidValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid? value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForGuidValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", default(Guid?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForDecimalValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            decimal? value = 123.45m;
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForDecimalValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", default(decimal?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForLongValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            long? value = 123;
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForLongValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", default(long?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForIntValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            int? value = 123;
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForIntValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", default(int?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForBoolValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            bool? value = true;
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForBoolValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.Search("Column1", default(bool?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForValueOnMultipleColumnsWithoutProvidingAnyColumn_ThenExceptionShouldBeThrown()
        {
            int? value = 3;
            Should.Throw<ArgumentException>(() => BuildPagedQuery(b => b.SearchValueOnMultipleColumns(value)))
                .Message.ShouldContain("at least one");
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForValueOnMultipleColumns_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            int? value = 3;
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchValueOnMultipleColumns(value, "Column1", "Column2", "Column3"));

            ShouldContainWhere(query, "WHERE (Column1 = @p1 OR Column2 = @p1 OR Column3 = @p1)");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForValueOnMultipleColumns_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchValueOnMultipleColumns(default(int?), "Column1", "Column2", "Column3"));

            ShouldNotContainWhere(query);
        }

        #endregion

        #region Searching by dates

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingColumnToBeWithinProvidedDateRange_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);

            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", startDate, endDate));

            ShouldContainWhere(query, "WHERE Column1 >= @p1 AND Column1 < @p2");
            ParameterValueShouldBe(query, "p1", startDate);
            ParameterValueShouldBe(query, "p2", endDate);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingColumnToBeWithinDateRangeWithProvidedStartOnly_ThenDataAndCountQueriesContainsWhereClauseOnlyWithStartCheck()
        {
            var startDate = new DateTime(2000, 10, 20);

            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", startDate, null));

            ShouldContainWhere(query, "WHERE Column1 >= @p1");
            ParameterValueShouldBe(query, "p1", startDate);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingColumnToBeWithinDateRangeWithProvidedEndOnly_ThenDataAndCountQueriesContainsWhereClauseOnlyWithEndCheck()
        {
            var endDate = new DateTime(2010, 10, 20);

            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", null, endDate));

            ShouldContainWhere(query, "WHERE Column1 < @p1");
            ParameterValueShouldBe(query, "p1", endDate);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingColumnToBeWithinDateRangeWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", null, null));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringForDatetimeValueBetweenTwoColumns_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var date = new DateTime(2010, 10, 20);

            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchDateToBeWithinColumnPeriod("Column1", "Column2", date));

            ShouldContainWhere(query, "WHERE Column1 <= @p1 AND Column2 > @p1");
            ParameterValueShouldBe(query, "p1", date);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringForDatetimeValueBetweenTwoColumnsWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchDateToBeWithinColumnPeriod("Column1", "Column2", null));

            ShouldNotContainWhere(query);
        }

        #endregion

        #region Searching by strings

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForTextLikeGivenValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchTextToBeLike("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 LIKE '%' + @p1 + '%'");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForTextLikeEmptyString_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchTextToBeLike("Column1", string.Empty));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForTextToBeLikeValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchTextToBeLike("Column1", null));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForTextToBeEqualGivenValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchTextToBeEqual("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForTextToBeEqualEmptyString_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchTextToBeEqual("Column1", string.Empty));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForTextToBeEqualValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.SearchTextToBeEqual("Column1", null));

            ShouldNotContainWhere(query);
        }

        #endregion

        #region Querying by nullability

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForNullValues_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.WhereIsNull("Column1"));

            ShouldContainWhere(query, "WHERE Column1 IS NULL");
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForNonNullValues_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.WhereIsNotNull("Column1"));

            ShouldContainWhere(query, "WHERE Column1 IS NOT NULL");
        }

        #endregion

        #region Querying by list

        public void PagedQueryFiltering_WhenQueryingItemsContainedInTheList_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var list = new[] { 1, 2, 3 };

            PagedQuery<Data> query = BuildPagedQuery(b => b.WhereIn("Column1", list));
            ShouldContainWhere(query, "WHERE Column1 IN @p1");
            ParameterValueShouldBe(query, "p1", list);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenQueryingItemsNotContainedInTheList_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var list = new[] { 1, 2, 3 };

            PagedQuery<Data> query = BuildPagedQuery(b => b.WhereNotIn("Column1", list));
            ShouldContainWhere(query, "WHERE Column1 NOT IN @p1");
            ParameterValueShouldBe(query, "p1", list);
        }

        #endregion

        [TestMethod]
        public void PagedQueryFiltering_WhenSpecyfyingMultipleFilteringCriteria_ThenDataAndCountQueriesDontContainWhereClauseWithAllOfThemConnectedByAndOperator()
        {
            int? intFilter = 10;
            var stringFilter = "Value";
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);


            PagedQuery<Data> query = BuildPagedQuery(b => b
                .Search("Column1", intFilter)
                .SearchColumnDoBeWithinDatePeriod("Column2", startDate, endDate)
                .SearchTextToBeLike("Column3", stringFilter));

            ShouldContainWhere(query, "WHERE Column1 = @p1 AND Column2 >= @p2 AND Column2 < @p3 AND Column3 LIKE '%' + @p4 + '%'");
            ParameterValueShouldBe(query, "p1", intFilter);
            ParameterValueShouldBe(query, "p2", startDate);
            ParameterValueShouldBe(query, "p3", endDate);
            ParameterValueShouldBe(query, "p4", stringFilter);
        }

        private PagedQuery<Data> BuildPagedQuery(Action<SqlQueryBuilder> filterAction)
        {
            return GetQueryBuilder()
                .Select("Column1", "Column2")
                .From("Table")
                .SortBy("Column1")
                .Apply(filterAction)
                .BuildPagedQuery<Data>(new SearchCriteria());
        }

        private static void ShouldNotContainWhere(PagedQuery<Data> query)
        {
            query.DataQuery.ShouldNotContain("WHERE");
            query.CountQuery.ShouldNotContain("WHERE");
        }

        private static void ShouldContainWhere(PagedQuery<Data> query, string clause)
        {
            query.DataQuery.ShouldContain(clause);
            query.CountQuery.ShouldContain(clause);
        }
    }
}
