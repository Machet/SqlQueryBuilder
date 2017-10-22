using System.Collections.Generic;
using System.Data;

namespace QueryBuilder
{
    public class DataQuery<T>
    {
        public string SelectQuery { get; }
        public Dictionary<string, object> Parameters { get; }
        public IDbConnection Connection;

        public DataQuery(IDbConnection connection, string selectQuery, Dictionary<string, object> parameters)
        {
            Ensure.NotNull(connection, nameof(connection));
            Ensure.NotNull(selectQuery, nameof(selectQuery));
            Ensure.NotNull(parameters, nameof(parameters));

            Connection = connection;
            SelectQuery = selectQuery;
            Parameters = parameters;
        }
    }
}
