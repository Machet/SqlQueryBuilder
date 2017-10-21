using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlQueryBuilder
{
    public class PagedQuery<T>
    {
        public string DataQuery { get; }
        public string CountQuery { get; }
        public Dictionary<string, object> Parameters { get; }
        public SearchCriteria SearchCriteria { get; }
        public IDbConnection Connection;

        public PagedQuery(IDbConnection connection, string dataQuery, string countQuery, Dictionary<string, object> parameters, SearchCriteria searchCriteria)
        {
            Ensure.NotNull(connection, nameof(connection));
            Ensure.NotNull(dataQuery, nameof(dataQuery));
            Ensure.NotNull(countQuery, nameof(countQuery));
            Ensure.NotNull(parameters, nameof(parameters));
            Ensure.NotNull(searchCriteria, nameof(searchCriteria));

            Connection = connection;
            DataQuery = dataQuery;
            CountQuery = countQuery;
            Parameters = parameters;
            SearchCriteria = searchCriteria;
        }        
    }
}
