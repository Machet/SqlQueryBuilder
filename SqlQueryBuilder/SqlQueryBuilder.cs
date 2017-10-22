using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QueryBuilder
{
    public class SqlQueryBuilder
    {
        private IDbConnection _connection;
        private string _table;
        private List<string> _columnsToSelect;
        private List<string> _whereConditions;
        private List<string> _order;
        private List<string> _groupings;
        private HashSet<string> _sortableColumns;
        private int _parameterGenerator;
        private Dictionary<string, object> _parameters;

        public SqlQueryBuilder(IDbConnection connection)
        {
            Ensure.NotNull(connection, "connection");
            _connection = connection;
            Clear();
        }

        public SqlQueryBuilder Select(params string[] columns)
        {
            Ensure.NotNull(columns, "columns");
            _columnsToSelect.AddRange(columns);
            return this;
        }

        public SqlQueryBuilder From(string table)
        {
            Ensure.IsNotEmpty(table, "table");
            _table = table;
            return this;
        }

        public SqlQueryBuilder Filter(string column, Guid? value)
        {
            if (value != null)
            {
                AddFilter(column, " = ", value.Value);
            }

            return this;
        }

        internal SqlQueryBuilder Filter(string column, bool? value)
        {
            if (value != null)
            {
                AddFilter(column, " = ", value.Value ? 1 : 0);
            }

            return this;
        }

        public SqlQueryBuilder Filter(string column, int? value)
        {
            if (value != null)
            {
                AddFilter(column, " = ", value.Value);
            }

            return this;
        }

        public SqlQueryBuilder Filter(string column1, string column2, Guid? value)
        {
            if (value != null)
            {
                string paramName = GetNextParameterName();
                _whereConditions.Add($"({column1} = {paramName} OR {column2} = {paramName})");
                _parameters.Add(paramName, value);
            }

            return this;
        }

        public SqlQueryBuilder Filter(string column, DateTime? start, DateTime? end)
        {
            if (start != null)
            {
                AddFilter(column, " > ", start.Value);
            }

            if (end != null)
            {
                AddFilter(column, " < ", end.Value);
            }

            return this;
        }

        public SqlQueryBuilder Filter(string column, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string paramName = GetNextParameterName();
                _whereConditions.Add(string.Concat(column, " LIKE '%' + ", paramName, " + '%'"));
                _parameters.Add(paramName, value);
                return this;
            }

            return this;
        }

        internal SqlQueryBuilder FilterToNotNull(string column)
        {
            _whereConditions.Add(string.Concat(column, " IS NOT NULL"));
            return this;
        }

        public SqlQueryBuilder WithSortableColumns(params string[] columns)
        {
            Ensure.NotNull(columns, "columns");
            foreach (var c in columns)
            {
                _sortableColumns.Add(c);
            }

            return this;
        }

        public SqlQueryBuilder SortBy(string column, bool ascending = true)
        {
            if (string.IsNullOrEmpty(column))
            {
                return this;
            }

            if (_sortableColumns.Any() && !_sortableColumns.Contains(column))
            {
                throw new ArgumentException("column is not allowed: " + column);
            }

            _order.Add(column + (ascending ? " ASC" : " DESC"));
            return this;
        }

        public SqlQueryBuilder GroupBy(params string[] columns)
        {
            if (columns == null || !columns.Any())
            {
                return this;
            }

            _groupings.AddRange(columns);
            return this;
        }

        public DataQuery<T> BuildQuery<T>()
        {
            Ensure.AtLeastOneElement(_columnsToSelect, "columns");
            Ensure.NotNull(_table, "table");
            string selectQuery = BuildSelectQuery();

            var query = new DataQuery<T>(_connection, selectQuery, _parameters);
            Clear();
            return query;
        }

        public DataQuery<T> BuildTopQuery<T>(int elementsCount)
        {
            Ensure.AtLeastOneElement(_columnsToSelect, "columns");
            Ensure.NotNull(_table, "table");

            string selectQuery = BuildSelectQuery().Replace("SELECT", $"SELECT TOP {elementsCount}");

            var query = new DataQuery<T>(_connection, selectQuery, _parameters);
            Clear();
            return query;
        }

        public PagedQuery<T> BuildPagedQuery<T>(SearchCriteria searchCriteria)
        {
            Ensure.AtLeastOneElement(_order, "sortings");
            Ensure.AtLeastOneElement(_columnsToSelect, "columns");
            Ensure.NotNull(_table, "table");
            Ensure.ThatGreaterThan(searchCriteria.PageSize, 0, "pageSize");
            Ensure.IsEmpty(_groupings, "Could not page grouped query");

            string selectQuery = BuildSelectQuery(searchCriteria.PageNumber, searchCriteria.PageSize);
            string countQuery = BuildCountQuery();

            var query = new PagedQuery<T>(_connection, selectQuery, countQuery, _parameters, searchCriteria);
            Clear();
            return query;
        }

        public PagedQuery<T> BuildPagedQuery<T>(int pageNumber, int pageSize)
        {
            return BuildPagedQuery<T>(new SearchCriteria { PageSize = pageSize, PageNumber = pageNumber });
        }

        private string BuildSelectQuery(int? pageNumber = null, int? pageSize = null)
        {
            var builder = new StringBuilder();
            builder.Append("SELECT " + _columnsToSelect.Join());
            builder.Append(" FROM " + _table);

            if (_whereConditions.Any())
            {
                builder.Append(" WHERE " + _whereConditions.Join(" AND "));
            }

            if (_groupings.Any())
            {
                builder.Append(" GROUP BY " + _groupings.Join());
            }

            if (_order.Any())
            {
                builder.Append(" ORDER BY " + _order.Join());
            }

            if (pageNumber != null && pageSize != null)
            {
                Ensure.ThatGreaterThan(pageNumber.Value, 0, "pageNumber");
                builder.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", (pageNumber - 1) * pageSize, pageSize);
            }

            return builder.ToString();
        }

        private string BuildCountQuery()
        {
            var builder = new StringBuilder();
            builder.Append("SELECT COUNT(*) FROM " + _table);

            if (_whereConditions.Any())
            {
                builder.Append(" WHERE " + _whereConditions.Join(" AND "));
            }

            return builder.ToString();
        }

        private void Clear()
        {
            _table = null;
            _columnsToSelect = new List<string>();
            _whereConditions = new List<string>();
            _order = new List<string>();
            _groupings = new List<string>();
            _sortableColumns = new HashSet<string>();
            _parameters = new Dictionary<string, object>();
        }

        private void AddFilter(string column, string filterOperator, object valueToFilter)
        {
            string paramName = GetNextParameterName();
            _whereConditions.Add(string.Concat(column, filterOperator, paramName));
            _parameters.Add(paramName, valueToFilter);
        }

        private string GetNextParameterName()
        {
            _parameterGenerator++;
            return "@p" + _parameterGenerator;
        }
    }
}
