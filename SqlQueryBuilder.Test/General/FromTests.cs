using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace QueryBuilder.Test
{
    [TestClass]
    public class FromTests : QueryBuilderTestBase
    {
        [TestMethod]
        public void From_WhenTableIsNullOrEmpty_ThenExceptionIsThrown()
        {
            Should.Throw<ArgumentNullException>(() => GetQueryBuilder().From(null));
            Should.Throw<ArgumentException>(() => GetQueryBuilder().From(""));
        }

        [TestMethod]
        public void From_WhenFromIsSpecifiedTwice_ThenExceptionIsThrown()
        {
            Should.Throw<InvalidOperationException>(() => GetQueryBuilder().From("Table1").From("Table2"))
                .Message.ShouldContain("From clause already specified");
        }
    }
}
