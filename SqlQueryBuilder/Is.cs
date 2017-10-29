using System.Collections.Generic;

namespace QueryBuilder
{
    public class Is
    {
        public string Operator { get; }
        public object Value { get; }
        public bool HasValue { get; }

        public Is(string operatorValue)
        {
            Ensure.NotNull(operatorValue, nameof(operatorValue));
            Operator = operatorValue;
            HasValue = false;
        }

        public Is(string operatorValue, object value)
            :this(operatorValue)
        {
            Ensure.NotNull(value, nameof(value));
            Value = value;
            HasValue = true;
        }

        public static Is EqualTo<T>(T value)
        {
            return new Is("=", value);
        }

        public static Is NotEqualTo<T>(T value)
        {
            return Is.DifferentThan(value);
        }

        public static Is DifferentThan<T>(T value)
        {
            return new Is("<>", value);
        }

        public static Is GreaterThan<T>(T value)
        {
            return new Is(">", value);
        }

        public static Is GreaterOrEqualThan<T>(T value)
        {
            return new Is(">=", value);
        }

        public static Is LowerThan<T>(T value)
        {
            return new Is("<", value);
        }

        public static Is LowerOrEqualThan<T>(T value)
        {
            return new Is("<=", value);
        }

        public static IEnumerable<Is> Between<T>(T startValue, T endValue, bool inclusive = true)
        {
            yield return GreaterOrEqualThan(startValue);
            yield return inclusive ? LowerOrEqualThan(endValue) : LowerThan(endValue);
        }

        public static IEnumerable<Is> InRange<T>(T startValue, T endValue)
        {
            return Between(startValue, endValue, false);
        }

        public static Is Like(string value, bool addWildcardOnBegining = true, bool addWildcardOnEnd = true)
        {
            var start = addWildcardOnBegining ? "%" : string.Empty;
            var end = addWildcardOnEnd ? "%" : string.Empty;
            return new Is("LIKE",  start + value + end);
        }

        public static Is NotLike(string value)
        {
            return new Is("NOT LIKE", value);
        }

        public static Is Null()
        {
            return new Is("IS NULL");
        }

        public static Is NotNull()
        {
            return new Is("IS NOT NULL");
        }

        public static Is In<T>(IEnumerable<T> values)
        {
            return new Is("IN", values);
        }

        public static Is NotIn<T>(IEnumerable<T> values)
        {
            return new Is("NOT IN", values);
        }
    }
}