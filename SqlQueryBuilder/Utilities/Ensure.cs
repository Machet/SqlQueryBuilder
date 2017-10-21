using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryBuilder
{
    internal static class Ensure
    {
        internal static void That(bool condition, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }

        internal static void NotNull(object obj, string argName, string message = null)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(argName, message);
            }
        }

        internal static void ThatGreaterThan<T>(T argument, T toCompare, string argumentName)
            where T : IComparable<T>
        {
            if (argument.CompareTo(toCompare) <= 0)
            {
                throw new ArgumentException(
                    string.Format("{0} {1} should be greater than {2}", argumentName, argument, toCompare));
            }
        }

        internal static void ThatGreaterThanOrEqual<T>(T argument, T toCompare, string argumentName)
            where T : IComparable<T>
        {
            if (argument.CompareTo(toCompare) < 0)
            {
                throw new ArgumentException(
                    string.Format("{0} {1} should be greater or equal than {2}", argumentName, argument, toCompare));
            }
        }

        internal static void ThatLessThan<T>(T argument, T toCompare, string argumentName)
            where T : IComparable<T>
        {
            if (argument.CompareTo(toCompare) >= 0)
            {
                throw new ArgumentException(
                    string.Format("{0} {1} should be less than {2}", argumentName, argument, toCompare));
            }
        }

        internal static void ThatLessThanOrEqual<T>(T argument, T toCompare, string argumentName)
            where T : IComparable<T>
        {
            if (argument.CompareTo(toCompare) > 0)
            {
                throw new ArgumentException(
                    string.Format("{0} {1} should be less or equal than {2}", argumentName, argument, toCompare));
            }
        }

        internal static void AreDifferent<T>(T first, T second, string message)
        {
            if (first.Equals(second))
            {
                throw new InvalidOperationException(message);
            }
        }

        internal static void AtLeastOneElement<T>(IEnumerable<T> collection, string argumentName)
        {
            NotNull(collection, argumentName);

            if (!collection.Any())
            {
                throw new ArgumentException(argumentName + " should have at least one element");
            }
        }

        internal static void IsNotEmpty(string argument, string argumentName)
        {
            NotNull(argument, argumentName);

            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException(argumentName + " should not be empty string");
            }
        }

        internal static void IsEmpty<T>(IEnumerable<T> collection, string message)
        {
            if (collection == null || collection.Any())
            {
                throw new ArgumentException(message);
            }
        }

        internal static void AreEqual<T>(T v1, T v2, string message)
        {
            if (!v1.Equals(v2))
            {
                throw new ArgumentException(message);
            }
        }

        internal static void IsNotIn<T>(T status, IEnumerable<T> values, string message)
        {
            if (values.Contains(status))
            {
                throw new InvalidOperationException(message);
            };
        }

        internal static void IsIn<T>(T status, IEnumerable<T> values, string message)
        {
            if (!values.Contains(status))
            {
                throw new InvalidOperationException(message);
            };
        }
    }
}
