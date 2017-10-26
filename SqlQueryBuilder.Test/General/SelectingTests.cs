using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test
{
    [TestClass]
    public class SelectTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void Selecting_WhenThereIsNoColumnsProvided_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentNullException>(() => GetQueryBuilder().Select(null));
            Should.Throw<ArgumentException>(() => GetQueryBuilder().Select());
        }
    }
}
