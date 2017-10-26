using System;
using System.Data;
using Shouldly;

namespace QueryBuilder.Test
{
    public class QueryBuilderTestBase
    {
        protected SqlQueryBuilder GetQueryBuilder()
        {
            return new SqlQueryBuilder(new ConnectionMock());
        }

        protected void ParameterValueShouldBe<T>(PagedQuery<Data> query, string parameterName, T expected)
        {
            var value = (T)query.Parameters['@' + parameterName];
            value.ShouldBe(expected);
        }

        protected void ParameterValueShouldBe<T>(DataQuery<Data> query, string parameterName, T expected)
        {
            var value = (T)query.Parameters['@' + parameterName];
            value.ShouldBe(expected);
        }

        protected class Data { }

        protected class ConnectionMock : IDbConnection
        {
            public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public int ConnectionTimeout => throw new NotImplementedException();

            public string Database => throw new NotImplementedException();

            public ConnectionState State => throw new NotImplementedException();

            public IDbTransaction BeginTransaction()
            {
                throw new NotImplementedException();
            }

            public IDbTransaction BeginTransaction(IsolationLevel il)
            {
                throw new NotImplementedException();
            }

            public void ChangeDatabase(string databaseName)
            {
                throw new NotImplementedException();
            }

            public void Close()
            {
                throw new NotImplementedException();
            }

            public IDbCommand CreateCommand()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public void Open()
            {
                throw new NotImplementedException();
            }
        }
    }
}