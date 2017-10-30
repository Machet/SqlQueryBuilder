using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.Query
{
    [TestClass]
    public class DataQuery_FilteringTests : QueryBuilderTestBase
    {
        #region Searching by numeric types

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForGuidValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid? value = Guid.NewGuid();
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForGuidValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", default(Guid?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForDecimalValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            decimal? value = 123.45m;
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForDecimalValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", default(decimal?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForLongValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            long? value = 123;
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForLongValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", default(long?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForIntValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            int? value = 123;
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForIntValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", default(int?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForBoolValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            bool? value = true;
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForBoolValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.Search("Column1", default(bool?)));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForValueOnMultipleColumnsWithoutProvidingAnyColumn_ThenExceptionShouldBeThrown()
        {
            int? value = 3;
            Should.Throw<ArgumentException>(() => BuildQuery(b => b.SearchValueOnMultipleColumns(value)))
                .Message.ShouldContain("at least one");
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForValueOnMultipleColumns_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            int? value = 3;
            DataQuery<Data> query = BuildQuery(b => b.SearchValueOnMultipleColumns(value, "Column1", "Column2", "Column3"));

            ShouldContainWhere(query, "WHERE (Column1 = @p1 OR Column2 = @p1 OR Column3 = @p1)");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForValueOnMultipleColumns_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.SearchValueOnMultipleColumns(default(int?), "Column1", "Column2", "Column3"));

            ShouldNotContainWhere(query);
        }

        #endregion

        #region Searching by dates

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingColumnToBeWithinProvidedDateRange_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);

            DataQuery<Data> query = BuildQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", startDate, endDate));

            ShouldContainWhere(query, "WHERE Column1 >= @p1 AND Column1 < @p2");
            ParameterValueShouldBe(query, "p1", startDate);
            ParameterValueShouldBe(query, "p2", endDate);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingColumnToBeWithinDateRangeWithProvidedStartOnly_ThenDataAndCountQueriesContainsWhereClauseOnlyWithStartCheck()
        {
            var startDate = new DateTime(2000, 10, 20);

            DataQuery<Data> query = BuildQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", startDate, null));

            ShouldContainWhere(query, "WHERE Column1 >= @p1");
            ParameterValueShouldBe(query, "p1", startDate);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingColumnToBeWithinDateRangeWithProvidedEndOnly_ThenDataAndCountQueriesContainsWhereClauseOnlyWithEndCheck()
        {
            var endDate = new DateTime(2010, 10, 20);

            DataQuery<Data> query = BuildQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", null, endDate));

            ShouldContainWhere(query, "WHERE Column1 < @p1");
            ParameterValueShouldBe(query, "p1", endDate);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingColumnToBeWithinDateRangeWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.SearchColumnDoBeWithinDatePeriod("Column1", null, null));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenFilteringForDatetimeValueBetweenTwoColumns_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var date = new DateTime(2010, 10, 20);

            DataQuery<Data> query = BuildQuery(b => b.SearchDateToBeWithinColumnPeriod("Column1", "Column2", date));

            ShouldContainWhere(query, "WHERE Column1 <= @p1 AND Column2 > @p1");
            ParameterValueShouldBe(query, "p1", date);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenFilteringForDatetimeValueBetweenTwoColumnsWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.SearchDateToBeWithinColumnPeriod("Column1", "Column2", null));

            ShouldNotContainWhere(query);
        }

        #endregion

        #region Searching by strings

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForTextLikeGivenValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            DataQuery<Data> query = BuildQuery(b => b.SearchTextToBeLike("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 LIKE @p1");
            ParameterValueShouldBe(query, "p1", '%' + value + '%');
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForTextLikeEmptyString_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.SearchTextToBeLike("Column1", string.Empty));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForTextToBeLikeValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.SearchTextToBeLike("Column1", null));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForTextToBeEqualGivenValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            DataQuery<Data> query = BuildQuery(b => b.SearchTextToBeEqual("Column1", value));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForTextToBeEqualEmptyString_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.SearchTextToBeEqual("Column1", string.Empty));

            ShouldNotContainWhere(query);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForTextToBeEqualValueWhichIsNotProvided_ThenDataAndCountQueriesDontContainWhereClause()
        {
            DataQuery<Data> query = BuildQuery(b => b.SearchTextToBeEqual("Column1", null));

            ShouldNotContainWhere(query);
        }

        #endregion

        #region Querying by nullability

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForNullValues_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            DataQuery<Data> query = BuildQuery(b => b.WhereIsNull("Column1"));

            ShouldContainWhere(query, "WHERE Column1 IS NULL");
        }

        [TestMethod]
        public void DataQueryFiltering_WhenSearchingForNonNullValues_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            DataQuery<Data> query = BuildQuery(b => b.WhereIsNotNull("Column1"));

            ShouldContainWhere(query, "WHERE Column1 IS NOT NULL");
        }

        #endregion

        #region Querying by list

        public void DataQueryFiltering_WhenQueryingItemsContainedInTheList_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var list = new[] { 1, 2, 3 };

            DataQuery<Data> query = BuildQuery(b => b.WhereIn("Column1", list));
            ShouldContainWhere(query, "WHERE Column1 IN @p1");
            ParameterValueShouldBe(query, "p1", list);
        }

        [TestMethod]
        public void DataQueryFiltering_WhenQueryingItemsNotContainedInTheList_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var list = new[] { 1, 2, 3 };

            DataQuery<Data> query = BuildQuery(b => b.WhereNotIn("Column1", list));
            ShouldContainWhere(query, "WHERE Column1 NOT IN @p1");
            ParameterValueShouldBe(query, "p1", list);
        }

        #endregion

        [TestMethod]
        public void DataQueryFiltering_WhenSpecyfyingMultipleFilteringCriteria_ThenDataAndCountQueriesDontContainWhereClauseWithAllOfThemConnectedByAndOperator()
        {
            int? intFilter = 10;
            var stringFilter = "Value";
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);


            DataQuery<Data> query = BuildQuery(b => b
                .Search("Column1", intFilter)
                .SearchColumnDoBeWithinDatePeriod("Column2", startDate, endDate)
                .SearchTextToBeLike("Column3", stringFilter));

            ShouldContainWhere(query, "WHERE Column1 = @p1 AND Column2 >= @p2 AND Column2 < @p3 AND Column3 LIKE @p4");
            ParameterValueShouldBe(query, "p1", intFilter);
            ParameterValueShouldBe(query, "p2", startDate);
            ParameterValueShouldBe(query, "p3", endDate);
            ParameterValueShouldBe(query, "p4", '%' + stringFilter + '%');
        }

        private DataQuery<Data> BuildQuery(Action<SqlQueryBuilder> filterAction)
        {
            return GetQueryBuilder()
                .Select("Column1", "Column2")
                .From("Table")
                .Apply(filterAction)
                .BuildQuery<Data>();
        }

        private static void ShouldNotContainWhere(DataQuery<Data> query)
        {
            query.SelectQuery.ShouldNotContain("WHERE");
        }

        private static void ShouldContainWhere(DataQuery<Data> query, string clause)
        {
            query.SelectQuery.ShouldContain(clause);
        }
    }
}
