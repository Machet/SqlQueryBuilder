using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace QueryBuilder.Dapper
{
    public static class DapperExtensions
    {
        public static Page<T> Execute<T>(this PagedQuery<T> query)
        {
            string combinedQuery = string.Concat(query.CountQuery, Environment.NewLine, query.DataQuery);
            using (var reader = query.Connection.QueryMultiple(combinedQuery, GetParameters(query.Parameters)))
            {
                return new Page<T>
                {
                    PageNumber = query.SearchCriteria.PageNumber,
                    PageSize = query.SearchCriteria.PageSize,
                    OrderedBy = query.SearchCriteria.OrderBy,
                    Total = reader.Read<int>().First(),
                    Records = reader.Read<T>().ToList()
                };
            }
        }

        public static List<T> ExecuteToList<T>(this DataQuery<T> query)
        {
            return query.Connection.Query<T>(query.SelectQuery, GetParameters(query.Parameters)).ToList();
        }

        public static T ExecuteToFirstOrDefault<T>(this DataQuery<T> query)
        {
            return query.Connection.Query<T>(query.SelectQuery, GetParameters(query.Parameters)).FirstOrDefault();
        }

        public static T ExecuteToSingleOrDefault<T>(this DataQuery<T> query)
        {
            return query.Connection.Query<T>(query.SelectQuery, GetParameters(query.Parameters)).SingleOrDefault();
        }

        private static DynamicParameters GetParameters(Dictionary<string, object> parameters)
        {
            var dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return dynamicParameters;
        }
    }
}
