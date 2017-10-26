using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.General
{
    [TestClass]
    public class SortingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void Sorting_WhenThereIsNoColumnsProvided_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentNullException>(() => GetQueryBuilder().SortBy(null));
            Should.Throw<ArgumentException>(() => GetQueryBuilder().SortBy(""));
        }

        [TestMethod]
        public void Sorting_WhenSortableColumnsAreNotProvided_ThenQueryCanBeSortedByAnything()
        {
            Should.NotThrow(() => GetQueryBuilder().SortBy("Anything"));
        }

        [TestMethod]
        public void Sorting_WhenSortableColumnsAreProvided_ThenQueryCanBeSortedOnlyByThoseColumns()
        {
            Should.NotThrow(() => GetQueryBuilder().WithSortableColumns("Something").SortBy("Something"));
            Should.Throw<ArgumentException>(() => GetQueryBuilder().WithSortableColumns("Something").SortBy("Anything"))
                .Message.ShouldContain("column is not allowed");
        }
    }
}
