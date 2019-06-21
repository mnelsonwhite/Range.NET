using System;
using System.Linq;
using Range.Net.Abstractions;
using Xunit;

namespace Range.Net.Tests
{
    public class RangeQueryableExtensionsTests
    {
        private readonly IQueryable<TestClass> _values;

        public RangeQueryableExtensionsTests()
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
        public void When_ShouldBe()
        {
            var range = new Range<int>(3, 6);
            var queryable = Enumerable
                .Range(1, 10)
                .Select(i => (intVal: i, strVal: i.ToString()))
                .AsQueryable();
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
        public void WhenFilterByRange_ShouldBeExpected()
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
            var actual = _values.FilterByRange(e => e.NullableDate, range);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(
            RangeInclusivity.ExclusiveMinExclusiveMax,
            "e => ((e.Date > 1/01/2001 12:00:00 AM) And (e.Date < 3/01/2001 12:00:00 AM))")]
        [InlineData(
            RangeInclusivity.InclusiveMinExclusiveMax,
            "e => ((e.Date >= 1/01/2001 12:00:00 AM) And (e.Date < 3/01/2001 12:00:00 AM))")]
        [InlineData(
            RangeInclusivity.ExclusiveMinInclusiveMax,
            "e => ((e.Date > 1/01/2001 12:00:00 AM) And (e.Date <= 3/01/2001 12:00:00 AM))")]
        [InlineData(
            RangeInclusivity.InclusiveMinInclusiveMax,
            "e => ((e.Date >= 1/01/2001 12:00:00 AM) And (e.Date <= 3/01/2001 12:00:00 AM))")]
        public void WhenRangeExclusivity_ShouldBeExpectedExpression(
            RangeInclusivity inclusivity,
            string expected)
        {
            // Arrange
            var range = new Range<DateTime>(new DateTime(2001, 1, 1), new DateTime(2001, 1, 3))
            {
                Inclusivity = inclusivity
            };

            // Act
            var expr = _values.FilterExpressionByRange(e => e.Date, range);
            var actual = expr.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(
            RangeInclusivity.ExclusiveMinExclusiveMax,
            "e => ((e.NullableDate.Value > 1/01/2001 12:00:00 AM) And (e.NullableDate.Value < 3/01/2001 12:00:00 AM))")]
        [InlineData(
            RangeInclusivity.InclusiveMinExclusiveMax,
            "e => ((e.NullableDate.Value >= 1/01/2001 12:00:00 AM) And (e.NullableDate.Value < 3/01/2001 12:00:00 AM))")]
        [InlineData(
            RangeInclusivity.ExclusiveMinInclusiveMax,
            "e => ((e.NullableDate.Value > 1/01/2001 12:00:00 AM) And (e.NullableDate.Value <= 3/01/2001 12:00:00 AM))")]
        [InlineData(
            RangeInclusivity.InclusiveMinInclusiveMax,
            "e => ((e.NullableDate.Value >= 1/01/2001 12:00:00 AM) And (e.NullableDate.Value <= 3/01/2001 12:00:00 AM))")]
        public void WhenRangeExclusivity_OnNullable_ShouldBeExpectedExpression(
            RangeInclusivity inclusivity,
            string expected)
        {
            // Arrange
            var range = new Range<DateTime>(new DateTime(2001, 1, 1), new DateTime(2001, 1, 3))
            {
                Inclusivity = inclusivity
            };

            // Act
            var expr = _values.FilterExpressionByRange(e => e.NullableDate, range);
            var actual = expr.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        internal class TestClass
        {
            public DateTime Date { get;set; }
            public DateTime? NullableDate { get; set; }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestClass) obj);
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
