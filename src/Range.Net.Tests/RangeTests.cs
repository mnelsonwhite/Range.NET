using Range.Net.Abstractions;
using System.Linq;
using Xunit;

namespace Range.Net.Tests
{
    public class RangeTests
    {
        [Theory]
        [InlineData(0, RangeInclusivity.InclusiveMinInclusiveMax, false)]
        [InlineData(1, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(1, RangeInclusivity.InclusiveMinExclusiveMax, true)]
        [InlineData(1, RangeInclusivity.ExclusiveMinExclusiveMax, false)]
        [InlineData(1, RangeInclusivity.ExclusiveMinInclusiveMax, false)]
        [InlineData(5, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(10, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(10, RangeInclusivity.InclusiveMinExclusiveMax, false)]
        [InlineData(10, RangeInclusivity.ExclusiveMinExclusiveMax, false)]
        [InlineData(10, RangeInclusivity.ExclusiveMinInclusiveMax, true)]
        [InlineData(11, RangeInclusivity.InclusiveMinInclusiveMax, false)]
        public void WhenContainsSpecificInclusivity_ShouldBeExpected(int value, RangeInclusivity inclusivitiy, bool expected)
        {
            // Arrange
            var r = new Range<int>()
            {
                Maximum = 10,
                Minimum = 1,
                Inclusivity = inclusivitiy
            };

            // Act
            var actual = r.Contains(value);
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0.00, RangeInclusivity.InclusiveMinInclusiveMax, false)]
        [InlineData(0.99, RangeInclusivity.InclusiveMinInclusiveMax, false)]
        [InlineData(1.00, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(1.00, RangeInclusivity.InclusiveMinExclusiveMax, true)]
        [InlineData(1.00, RangeInclusivity.ExclusiveMinInclusiveMax, false)]
        [InlineData(1.00, RangeInclusivity.ExclusiveMinExclusiveMax, false)]
        [InlineData(1.01, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(5.00, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(9.99, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(10.00, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(10.00, RangeInclusivity.ExclusiveMinInclusiveMax, true)]
        [InlineData(10.00, RangeInclusivity.InclusiveMinExclusiveMax, false)]
        [InlineData(10.00, RangeInclusivity.ExclusiveMinExclusiveMax, false)]
        [InlineData(11.00, RangeInclusivity.InclusiveMinInclusiveMax, false)]
        public void WhenContainsSpecificInclusivity_ShouldBeExpected2(double value, RangeInclusivity inclusivitiy, bool expected)
        {
            // Arrange
            var r = new Range<double>()
            {
                Maximum = 10.00,
                Minimum = 1.00,
                Inclusivity = inclusivitiy
            };

            // Act
            var actual = r.Contains(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, false)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, false)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, false)]
        public void WhenZeroWidthRange_ShouleBeExpected(RangeInclusivity inclusivitiy, bool expected)
        {
            // Arrange
            var r = new Range<int>()
            {
                Maximum = 100,
                Minimum = 100,
                Inclusivity = inclusivitiy
            };

            // Act
            var actual = r.Contains(100);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2, 2, 3, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(1, 2, 2, 3, RangeInclusivity.ExclusiveMinExclusiveMax, false)]
        [InlineData(1, 2, 2, 3, RangeInclusivity.ExclusiveMinInclusiveMax, false)]
        [InlineData(1, 2, 2, 3, RangeInclusivity.InclusiveMinExclusiveMax, false)]

        [InlineData(2, 3, 1, 2, RangeInclusivity.InclusiveMinInclusiveMax, true)]
        [InlineData(2, 3, 1, 2, RangeInclusivity.ExclusiveMinExclusiveMax, false)]
        [InlineData(2, 3, 1, 2, RangeInclusivity.ExclusiveMinInclusiveMax, false)]
        [InlineData(2, 3, 1, 2, RangeInclusivity.InclusiveMinExclusiveMax, false)]
        public void WhenIntersects_ShouldBeInsectingRange(
            int aMin,
            int aMax,
            int bMin,
            int bMax,
            RangeInclusivity inclusivity,
            bool expected)
        {
            // Arrange
            var aRange = new Range<int>(aMin, aMax, inclusivity);
            var bRange = new Range<int>(bMin, bMax, inclusivity);

            // Act
            var actual = aRange.Intersects(bRange);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(1, 1, 4, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(1, 2, 2, 3, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(1, 2, 2, 3, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 2, 2, 3, RangeInclusivity.InclusiveMinExclusiveMax)]
        public void WhenRangesCombined_ShouldBeIntersect(
            int aMin,
            int aMax,
            int bMin,
            int bMax,
            RangeInclusivity inclusivity)
        {
            // Arrange
            var aRange = new Range<int>(aMin, aMax, inclusivity);
            var bRange = new Range<int>(bMin, bMax, inclusivity);

            // Act
            var actual = aRange.TryIntersect(bRange, out _);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void WhenMinExceedsMax_ShouldBeSwapped()
        {
            // Arrange
            var expected = new Range<int>(1, 3);

            // Act
            var actual = new Range<int>(3, 1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenSetMinExceedsMax_ShouldBeThrown()
        {
            // Arrange
            var actual = new Range<int>(1, 3);
            var expected = new Range<int>(3, 4);

            // Act
            // Act in assertion
            actual.Minimum = 4;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenSetMaxBelowMin_ShouldBeThrown()
        {
            // Arrange
            var actual = new Range<int>(1, 3);
            var expected = new Range<int>(0, 1);

            // Act
            actual.Maximum = 0;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenEqualsNull_ShouldBeFalse()
        {
            // Arrange
            var range = new Range<int>(1, 3);

            // Act
            var actual = range.Equals(null);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 2, 3, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 2, 3, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 2, 3, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 2, 3, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinInclusiveMax, 2, 3, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinInclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinInclusiveMax, 2, 3, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinInclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinInclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinExclusiveMax, 2, 3, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinExclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinExclusiveMax, 2, 3, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinExclusiveMax, 2, 3, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.InclusiveMinExclusiveMax, 1, 4, RangeInclusivity.InclusiveMinExclusiveMax)]
        public void WhenRangeContainsRange_ContainsShouldBeTrue(
            int r1Min, int r1Max, RangeInclusivity r1Inc,
            int r2Min, int r2Max, RangeInclusivity r2Inc)
        {
            // Arrange
            var r1 = new Range<int>(r1Min, r1Max, r1Inc);
            var r2 = new Range<int>(r2Min, r2Max, r2Inc);

            // Act
            var actual = r1.Contains(r2);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(1, 4, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinExclusiveMax, 1, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinInclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinInclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinInclusiveMax, 1, 4, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.ExclusiveMinInclusiveMax, 1, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinExclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinExclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinExclusiveMax, 1, 4, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinExclusiveMax, 1, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(2, 3, RangeInclusivity.InclusiveMinInclusiveMax, 1, 4, RangeInclusivity.InclusiveMinInclusiveMax)]
        public void WhenRangeDoesNotContainsRange_ContainsShouldBeFalse(
            int r1Min, int r1Max, RangeInclusivity r1Inc,
            int r2Min, int r2Max, RangeInclusivity r2Inc)
        {
            // Arrange
            var r1 = new Range<int>(r1Min, r1Max, r1Inc);
            var r2 = new Range<int>(r2Min, r2Max, r2Inc);

            // Act
            var actual = r1.Contains(r2);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(RangeInclusivity.InclusiveMinInclusiveMax)]
        public void WhenAnyInclusivity_RangeShouldContainItself(RangeInclusivity inclusivity)
        {
            // Arrange
            var range = new Range<int>(1, 4, inclusivity);

            // Act
            var actual = range.Contains(range);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, RangeInclusivity.InclusiveMinInclusiveMax)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, RangeInclusivity.ExclusiveMinInclusiveMax)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, RangeInclusivity.InclusiveMinExclusiveMax)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, RangeInclusivity.InclusiveMinInclusiveMax)]
        public void WhenSameRange_ExclusiveShouldNotContainInclusive(RangeInclusivity r1Inc, RangeInclusivity r2Inc)
        {
            // Arrange
            var r1 = new Range<int>(1, 4, r1Inc);
            var r2 = new Range<int>(1, 4, r2Inc);

            // Act
            var actual = r1.Contains(r2);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void WhenIntRangeToString_ShouldBeExpected()
        {
            // Arrange
            var range = new Range<int>(1, 3);

            // Act
            var actual = range.ToString();

            // Assert
            Assert.Equal("[1 - 3]", actual);
        }

        [Fact]
        public void WhenCreateNewRange_ShouldBeExpectedMinAndMax()
        {
            // Act
            var range = new Range<int>(1, 10);

            // Assert
            Assert.Equal(1, range.Minimum);
            Assert.Equal(10, range.Maximum);
        }

        [Fact]
        public void WhenRangeContainsValue_ShouldBeExpected()
        {
            // Arrange
            var range = new Range<int>(1, 10);

            // Act Assert
            Assert.True(range.Contains(1));
            Assert.True(range.Contains(3));
            Assert.True(range.Contains(10));
            Assert.False(range.Contains(0));
            Assert.False(range.Contains(11));
        }

        [Theory]
        [InlineData(0, 2, true)]
        [InlineData(0, 11, true)]
        [InlineData(9, 11, true)]
        [InlineData(1, 9, true)]
        [InlineData(-1, 0, false)]
        [InlineData(11, 12, false)]
        public void WhenRangeIntersectsWithAnother_ShouldBeExpected(int min, int max, bool expected)
        {
            // Arrange
            var range = new Range<int>(1, 10);

            // Act Assert
            Assert.Equal(expected, range.Intersects(new Range<int>(min, max)));
        }

        [Fact]
        public void WhenUnionRange_ShouldBeExpectedMinAndMax()
        {
            // Arrange
            var range1 = new Range<int>(1, 6, RangeInclusivity.ExclusiveMinExclusiveMax);
            var range2 = new Range<int>(3, 10, RangeInclusivity.InclusiveMinInclusiveMax);

            // Act
            var actual = range1.TryIntersect(range2, out var actualUnioned);

            // Assert
            Assert.True(actual);
            Assert.Equal(3, actualUnioned.Minimum);
            Assert.Equal(6, actualUnioned.Maximum);
        }

        [Fact]
        public void WhenDifferentInclusivity_Contains_ShouldBeExpected()
        {
            // Arrange
            var range1 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.ExclusiveMinExclusiveMax };
            var range2 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.ExclusiveMinInclusiveMax };
            var range3 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.InclusiveMinExclusiveMax };
            var range4 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.InclusiveMinInclusiveMax };

            // Act Assert
            Assert.False(range1.Contains(1));
            Assert.False(range1.Contains(10));

            Assert.False(range2.Contains(1));
            Assert.True(range2.Contains(10));

            Assert.True(range3.Contains(1));
            Assert.False(range3.Contains(10));

            Assert.True(range4.Contains(1));
            Assert.True(range4.Contains(10));
        }

        [Fact]
        public void WhenFilterQueryable_ShouldBeExpected()
        {
            // Arrange
            var range = new Range<int>(3, 6);
            var queryable = Enumerable
                .Range(1, 10)
                .Select(i => (intVal: i, strVal: i.ToString()))
                .AsQueryable();

            // Act
            var actual = queryable.FilterByRange(a => a.intVal, range);

            // Assert
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

        [Theory]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, -1, false)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 2, false)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 4, false)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 5, false)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 6, true)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 7, true)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, 4, 6, -1, false)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, 4, 6, 2, false)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, 4, 6, 4, false)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, 4, 6, 5, false)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, 4, 6, 6, false)]
        [InlineData(RangeInclusivity.ExclusiveMinInclusiveMax, 4, 6, 7, true)]
        
        public void WhenLessThan_ShouldBeExpected(RangeInclusivity inclusivity, int min, int max, int value, bool expected)
        {
            // Arrange
            var range = new Range<int>(min, max) { Inclusivity = inclusivity };

            // Act
            var actual = range.LessThan(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, -1, true)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 2, true)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 4, true)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 5, false)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 6, false)]
        [InlineData(RangeInclusivity.ExclusiveMinExclusiveMax, 4, 6, 7, false)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, 4, 6, -1, true)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, 4, 6, 2, true)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, 4, 6, 4, false)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, 4, 6, 5, false)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, 4, 6, 6, false)]
        [InlineData(RangeInclusivity.InclusiveMinExclusiveMax, 4, 6, 7, false)]
        public void WhenGreaterThan_ShouldBeExpected(RangeInclusivity inclusivity, int min, int max, int value, bool expected)
        {
            // Arrange
            var range = new Range<int>(min, max) { Inclusivity = inclusivity };

            // Act
            var actual = range.GreaterThan(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 0)]
        [InlineData(5, 0)]
        [InlineData(6, 0)]
        [InlineData(7, -1)]
        public void WhenCompareTo_ShouldBeExpected(int value, int expected)
        {
            // Arrange
            var range = new Range<int>(4, 6);

            // Act
            var actual = range.CompareTo(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2)]
        public void WhenInitialize_ShouldBeExpected(int min, int max)
        {
            // Arrange, Act
            var range = new Range<int>()
            {
                Minimum = min,
                Maximum = max
            };

            // Assert
            Assert.Equal(min, range.Minimum);
            Assert.Equal(max, range.Maximum);
        }
    }
}
