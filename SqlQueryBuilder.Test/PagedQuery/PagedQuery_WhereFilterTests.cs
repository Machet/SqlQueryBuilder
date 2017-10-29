using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.PagedQuery
{
    [TestClass]
    public class PagedQuery_WhereFilterTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringEqualValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.EqualTo(value)));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringNotEqualValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.NotEqualTo(value)));

            ShouldContainWhere(query, "WHERE Column1 <> @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringDifferentValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.DifferentThan(value)));

            ShouldContainWhere(query, "WHERE Column1 <> @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringGreaterThanValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.GreaterThan(value)));

            ShouldContainWhere(query, "WHERE Column1 > @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringGreaterOrEqualThanValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.GreaterOrEqualThan(value)));

            ShouldContainWhere(query, "WHERE Column1 >= @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringLowerThanValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.LowerThan(value)));

            ShouldContainWhere(query, "WHERE Column1 < @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringLowerOrEqualThanValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            Guid value = Guid.NewGuid();
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.LowerOrEqualThan(value)));

            ShouldContainWhere(query, "WHERE Column1 <= @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringValueBetween_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);

            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.Between(startDate, endDate)));

            ShouldContainWhere(query, "WHERE Column1 >= @p1 AND Column1 <= @p2");
            ParameterValueShouldBe(query, "p1", startDate);
            ParameterValueShouldBe(query, "p2", endDate);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringValueBetweenExclusive_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);

            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.Between(startDate, endDate, inclusive: false)));

            ShouldContainWhere(query, "WHERE Column1 >= @p1 AND Column1 < @p2");
            ParameterValueShouldBe(query, "p1", startDate);
            ParameterValueShouldBe(query, "p2", endDate);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringValueInRange_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);

            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.InRange(startDate, endDate)));

            ShouldContainWhere(query, "WHERE Column1 >= @p1 AND Column1 < @p2");
            ParameterValueShouldBe(query, "p1", startDate);
            ParameterValueShouldBe(query, "p2", endDate);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringForTextLikeGivenValueWithBothWildcards_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.Like(value)));

            ShouldContainWhere(query, "WHERE Column1 LIKE @p1");
            ParameterValueShouldBe(query, "p1", '%' + value + '%');
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringForTextLikeGivenValueWithStartWildcard_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.Like(value, addWildcardOnBegining: true, addWildcardOnEnd: false)));

            ShouldContainWhere(query, "WHERE Column1 LIKE @p1");
            ParameterValueShouldBe(query, "p1", '%' + value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenFilteringForTextLikeGivenValueWithEndWildcard_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.Like(value, addWildcardOnBegining: false)));

            ShouldContainWhere(query, "WHERE Column1 LIKE @p1");
            ParameterValueShouldBe(query, "p1", value + '%');
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForTextToBeEqualGivenValue_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            const string value = "value";
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.EqualTo(value)));

            ShouldContainWhere(query, "WHERE Column1 = @p1");
            ParameterValueShouldBe(query, "p1", value);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForNullValues_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.Null()));

            ShouldContainWhere(query, "WHERE Column1 IS NULL");
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSearchingForNonNullValues_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.NotNull()));

            ShouldContainWhere(query, "WHERE Column1 IS NOT NULL");
        }

        public void PagedQueryFiltering_WhenQueryingItemsContainedInTheList_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var list = new[] { 1, 2, 3 };

            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.In(list)));
            ShouldContainWhere(query, "WHERE Column1 IN @p1");
            ParameterValueShouldBe(query, "p1", list);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenQueryingItemsNotContainedInTheList_ThenDataAndCountQueriesContainsWhereClauseWithProperCondition()
        {
            var list = new[] { 1, 2, 3 };

            PagedQuery<Data> query = BuildPagedQuery(b => b.Where("Column1", Is.NotIn(list)));
            ShouldContainWhere(query, "WHERE Column1 NOT IN @p1");
            ParameterValueShouldBe(query, "p1", list);
        }

        [TestMethod]
        public void PagedQueryFiltering_WhenSpecyfyingMultipleFilteringCriteria_ThenDataAndCountQueriesDontContainWhereClauseWithAllOfThemConnectedByAndOperator()
        {
            int? intFilter = 10;
            var stringFilter = "Value";
            var startDate = new DateTime(2000, 10, 20);
            var endDate = new DateTime(2010, 10, 20);


            PagedQuery<Data> query = BuildPagedQuery(b => b
                .Where("Column1", Is.EqualTo(intFilter))
                .Search("ColumnX", (int?)null)
                .Search("Column2", intFilter)
                .Where("Column3", Is.InRange(startDate, endDate))
                .Where("Column4", Is.Like(stringFilter)));


            ShouldContainWhere(query, "WHERE Column1 = @p1 AND Column2 = @p2 AND Column3 >= @p3 AND Column3 < @p4 AND Column4 LIKE @p5");
            ParameterValueShouldBe(query, "p1", intFilter);
            ParameterValueShouldBe(query, "p2", intFilter);
            ParameterValueShouldBe(query, "p3", startDate);
            ParameterValueShouldBe(query, "p4", endDate);
            ParameterValueShouldBe(query, "p5", '%' + stringFilter + '%');
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
