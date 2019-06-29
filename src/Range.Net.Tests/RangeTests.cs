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
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(3, 4, 1, 2, false)]
        [InlineData(1, 1, 2, 2, false)]
        [InlineData(1, 4, 2, 3, true)]
        [InlineData(2, 3, 1, 4, true)]
        [InlineData(1, 3, 2, 4, true)]
        [InlineData(2, 4, 1, 3, true)]
        public void WhenIntersects_ShouldBeInsectingRange(int aMin, int aMax, int bMin, int bMax, bool expected)
        {
            // Arrange
            var aRange = new Range<int>(aMin, aMax);
            var bRange = new Range<int>(bMin, bMax);

            // Act
            var actual = aRange.Intersects(bRange);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 1, 4)]
        [InlineData(1, 1, 4, 4, 1, 4)]
        [InlineData(4, 4, 1, 1, 1, 4)]
        [InlineData(1, 3, 2, 4, 1, 4)]
        public void WhenRangesCombined_ShouldBeUnion(
            int aMin,
            int aMax,
            int bMin,
            int bMax,
            int expectedMin,
            int expectedMax)
        {
            // Arrange
            var aRange = new Range<int>(aMin, aMax);
            var bRange = new Range<int>(bMin, bMax);
            var expected = new Range<int>(expectedMin, expectedMax);

            // Act
            var actual = aRange.Union(bRange);

            // Assert
            Assert.Equal(expected, actual);
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
        [InlineData(1, 4, 2, 3, true)]
        public void WhenRangeContainsRange_ShouldBeExpected(
            int r1Min, int r1Max,
            int r2Min, int r2Max,
            bool expected)
        {
            // Arrange
            var r1 = new Range<int>(r1Min, r1Max);
            var r2 = new Range<int>(r2Min, r2Max);

            // Act
            var actual = r1.Contains(r2);

            // Assert
            Assert.Equal(expected, actual);
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

        [Fact]
        public void WhenRangeIntersectsWithAnother_ShouldBeExpected()
        {
            // Arrange
            var range = new Range<int>(1, 10);

            // Act Assert
            Assert.True(range.Intersects(new Range<int>(0, 2)));
            Assert.True(range.Intersects(new Range<int>(0, 11)));
            Assert.True(range.Intersects(new Range<int>(9, 11)));
            Assert.True(range.Intersects(new Range<int>(1, 9)));
            Assert.False(range.Intersects(new Range<int>(-1, 0)));
            Assert.False(range.Intersects(new Range<int>(11, 12)));
        }

        [Fact]
        public void WhenUnionRange_ShouldBeExpectedMinAndMax()
        {
            // Arrange
            var range1 = new Range<int>(1, 6);
            var range2 = new Range<int>(3, 10);

            // Act
            var union = range1.Union(range2);

            // Assert
            Assert.Equal(1, union.Minimum);
            Assert.Equal(10, union.Maximum);
        }

        [Fact]
        public void WhenDifferentInclusivity_Contains_ShouldBeExpected()
        {
            var range1 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.ExclusiveMinExclusiveMax };
            var range2 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.ExclusiveMinInclusiveMax };
            var range3 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.InclusiveMinExclusiveMax };
            var range4 = new Range<int>(1, 10) { Inclusivity = RangeInclusivity.InclusiveMinInclusiveMax };

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
        public void WhenFilterQuertable_ShouldBeExpected()
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
    }
}
