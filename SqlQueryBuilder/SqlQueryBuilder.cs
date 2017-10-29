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
            Ensure.NotNull(connection, nameof(connection));
            _connection = connection;
            Clear();
        }

        public SqlQueryBuilder Select(params string[] columns)
        {
            Ensure.NotNull(columns, nameof(columns));
            Ensure.AtLeastOneElement(columns, nameof(columns));

            _columnsToSelect.AddRange(columns);
            return this;
        }

        public SqlQueryBuilder From(string table)
        {
            Ensure.IsNotEmpty(table, nameof(table));
            Ensure.That(_table == null, "From clause already specified");

            _table = table;
            return this;
        }

        public SqlQueryBuilder Search<T>(string column, T? value) where T : struct
        {
            Ensure.NotNull(column, nameof(column));

            if (value != null)
            {
                AddFilter(column, " = ", value.Value);
            }

            return this;
        }

        public SqlQueryBuilder SearchValueOnMultipleColumns<T>(T? value, params string[] columns) where T : struct
        {
            Ensure.AtLeastOneElement(columns, nameof(columns));

            if (value != null)
            {
                string paramName = GetNextParameterName();
                var strings = columns.Select(c => $"{c} = {paramName}");
                _whereConditions.Add($"({string.Join(" OR ", strings)})");
                _parameters.Add(paramName, value);
            }

            return this;
        }

        public SqlQueryBuilder SearchColumnDoBeWithinDatePeriod(string column, DateTime? start, DateTime? end)
        {
            Ensure.NotNull(column, nameof(column));

            if (start != null)
            {
                AddFilter(column, " >= ", start.Value);
            }

            if (end != null)
            {
                AddFilter(column, " < ", end.Value);
            }

            return this;
        }

        public SqlQueryBuilder SearchDateToBeWithinColumnPeriod(string startColumn, string endColumn, DateTime? date)
        {
            Ensure.NotNull(startColumn, nameof(startColumn));
            Ensure.NotNull(endColumn, nameof(endColumn));

            if (date != null)
            {
                string paramName = GetNextParameterName();
                _parameters.Add(paramName, date);
                _whereConditions.Add(string.Concat(startColumn, " <= ", paramName));
                _whereConditions.Add(string.Concat(endColumn, " > ", paramName));
            }

            return this;
        }

        public SqlQueryBuilder SearchTextToBeLike(string column, string value)
        {
            Ensure.NotNull(column, nameof(column));

            if (!string.IsNullOrEmpty(value))
            {
                string paramName = GetNextParameterName();
                _whereConditions.Add(string.Concat(column, " LIKE '%' + ", paramName, " + '%'"));
                _parameters.Add(paramName, value);
            }

            return this;
        }

        public SqlQueryBuilder SearchTextToBeEqual(string column, string value)
        {
            Ensure.NotNull(column, nameof(column));

            if (!string.IsNullOrEmpty(value))
            {
                AddFilter(column, " = ", value);
            }

            return this;
        }

        public SqlQueryBuilder Where<T>(string column, T value)
        {
            Ensure.NotNull(column, nameof(column));

            AddFilter(column, " = ", value);

            return this;
        }

        public SqlQueryBuilder Where(string column, Is filterType)
        {
            Ensure.NotNull(column, nameof(column));
            Ensure.NotNull(filterType, nameof(filterType));

            if (filterType.HasValue)
            {
                AddFilter(column, " " + filterType.Operator + " ", filterType.Value);
            }
            else
            {
                _whereConditions.Add(string.Concat(column, " ", filterType.Operator));
            }

            return this;
        }

        public SqlQueryBuilder Where(string column, IEnumerable<Is> filterTypes)
        {
            foreach (var filterType in filterTypes)
            {
                Where(column, filterType);
            }

            return this;
        }

        public SqlQueryBuilder WhereIn<T>(string column, IEnumerable<T> values)
        {
            Ensure.NotNull(column, nameof(column));

            AddFilter(column, " IN ", values);
            return this;
        }

        public SqlQueryBuilder WhereNotIn<T>(string column, IEnumerable<T> values)
        {
            Ensure.NotNull(column, nameof(column));

            AddFilter(column, " NOT IN ", values);
            return this;
        }

        public SqlQueryBuilder WhereIsNull(string column)
        {
            Ensure.NotNull(column, nameof(column));

            _whereConditions.Add(string.Concat(column, " IS NULL"));
            return this;
        }

        public SqlQueryBuilder WhereIsNotNull(string column)
        {
            Ensure.NotNull(column, nameof(column));

            _whereConditions.Add(string.Concat(column, " IS NOT NULL"));
            return this;
        }

        public SqlQueryBuilder WithSortableColumns(params string[] columns)
        {
            Ensure.NotNull(columns, nameof(columns));

            foreach (var c in columns)
            {
                _sortableColumns.Add(c);
            }

            return this;
        }

        public SqlQueryBuilder SortBy(string column, bool ascending = true)
        {
            Ensure.IsNotEmpty(column, nameof(column));

            if (_sortableColumns.Any() && !_sortableColumns.Contains(column))
            {
                throw new ArgumentException("column is not allowed: " + column);
            }

            _order.Add(column + (ascending ? " ASC" : " DESC"));
            return this;
        }

        public SqlQueryBuilder GroupBy(params string[] columns)
        {
            Ensure.NotNull(columns, nameof(columns));
            Ensure.AtLeastOneElement(columns, nameof(columns));

            _groupings.AddRange(columns);
            return this;
        }

        public SqlQueryBuilder Apply(Action<SqlQueryBuilder> actionToApply)
        {
            actionToApply?.Invoke(this);
            return this;
        }

        public SqlQueryBuilder When(bool condition, Action<SqlQueryBuilder> thenAction, Action<SqlQueryBuilder> elseAction)
        {
            return condition ? Apply(thenAction) : Apply(elseAction);
        }

        public DataQuery<T> BuildQuery<T>()
        {
            Ensure.AtLeastOneElement(_columnsToSelect, "columns");
            Ensure.That(!string.IsNullOrEmpty(_table), "table is not specified");
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
            Ensure.AtLeastOneElement(_columnsToSelect, "columns");
            Ensure.That(!string.IsNullOrEmpty(_table), "table is not specified");
            Ensure.AtLeastOneElement(_order, "sortings");
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
