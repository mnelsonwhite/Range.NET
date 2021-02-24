using System;
using System.Collections.Generic;
using System.Linq;
using Range.Net.Abstractions;
using Xunit;

namespace Range.Net.Tests
{
    public class EnumerableExtensionsTests
    {
        private readonly IEnumerable<TestClass> _values;

        public EnumerableExtensionsTests()
        {
            _values = new[]
            {
                new TestClass
                {
                    Date = new DateTime(2001, 1, 2),
                    NullableDate = new DateTime(2001, 1, 1)
                },
                new TestClass
                {
                    Date = new DateTime(2001, 2, 2),
                    NullableDate = new DateTime(2001, 1, 3)
                },
                new TestClass
                {
                    Date = new DateTime(2001, 1, 3),
                    NullableDate = new DateTime(2001, 10, 3)
                }
            }.AsQueryable();
        }

        [Fact]
        public void WhenTupleFilterByRange_ShouldBeExpected()
        {
            var range = new Range<int>(3, 6);
            var queryable = Enumerable
                .Range(1, 10)
                .Select(i => (intVal: i, strVal: i.ToString()));

            var actual = queryable.FilterByRange(a => a.intVal, range);

            Assert.Equal(
                new[] {
                    (3, "3"),
                    (4, "4"),
                    (5, "5"),
                    (6, "6")
                },
                actual
            );
        }

        [Fact]
        public void WhenComplexFilterByRange_ShouldBeExpected()
        {
            // Arrange
            var range = new Range<DateTime>(new DateTime(2001, 1, 1), new DateTime(2001, 1, 3))
            {
                Inclusivity = RangeInclusivity.InclusiveMinInclusiveMax
            };

            var expected = new[]
            {
                new TestClass
                {
                    Date = new DateTime(2001, 1, 2),
                    NullableDate = new DateTime(2001, 1, 1)
                },
                new TestClass
                {
                    Date = new DateTime(2001, 1, 3),
                    NullableDate = new DateTime(2001, 10, 3)
                }
            };

            // Act
            var actual = _values.FilterByRange(e => e.Date, range);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenFilterByRange_OnNullanble_ShouldBeExpected()
        {
            // Arrange
            var range = new Range<DateTime>(new DateTime(2001, 1, 1), new DateTime(2001, 1, 3))
            {
                Inclusivity = RangeInclusivity.InclusiveMinInclusiveMax
            };

            var expected = new[]
            {
                new TestClass
                {
                    Date = new DateTime(2001, 1, 2),
                    NullableDate = new DateTime(2001, 1, 1)
                },
                new TestClass
                {
                    Date = new DateTime(2001, 2, 2),
                    NullableDate = new DateTime(2001, 1, 3)
                }
            };

            // Act
            var actual = _values
                .Where(e => e.NullableDate.HasValue)
                .FilterByRange(e => e.NullableDate.Value, range);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenFilterByRange_ShouleBeExpected()
        {
            // Arrange
            var range = new Range<int>(3, 6);
            var queryable = Enumerable
                .Range(1, 10)
                .Select(i => new ProtectedPropertyClass(i));

            // Act
            var actual = queryable.FilterByRange(q => q.Value, range);

            // Assert
            Assert.Equal(4, actual.Count());
        }

        internal class ProtectedPropertyClass
        {
            public ProtectedPropertyClass(int value)
            {
                Value = value;
            }
            public int Value { get; protected set; }
        }

        internal class TestClass
        {
            public DateTime Date { get; set; }
            public DateTime? NullableDate { get; set; }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestClass)obj);
            }

            protected bool Equals(TestClass other)
            {
                return Date.Equals(other.Date) && NullableDate.Equals(other.NullableDate);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Date.GetHashCode() * 397) ^ NullableDate.GetHashCode();
                }
            }
        }
    }
}
