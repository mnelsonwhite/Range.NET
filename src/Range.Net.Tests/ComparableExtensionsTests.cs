using Xunit;

namespace Range.Net.Tests
{
    public class ComparableExtensionsTests
    {
        [Fact]
        public void WhenComparable_ShouldBeAbleToRange()
        {
            // Arrange
            var from = 1;
            var to = 10;
            var expected = new Range<int>(1, 10);

            // Act
            var actual = from.To(to);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
