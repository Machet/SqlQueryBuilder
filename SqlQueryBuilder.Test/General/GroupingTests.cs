using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test.General
{
    [TestClass]
    public class GroupingTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void Grouping_WhenThereIsNoColumnsProvided_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentNullException>(() => GetQueryBuilder().Select(null));
            Should.Throw<ArgumentException>(() => GetQueryBuilder().Select());
        }
    }
}
