using Range.Net.Abstractions;
using System;

namespace Range.Net
{
    public static class RangeExtensions
    {
        /// <summary>
        /// Determined hows to range compares to the provided value.
        /// Less than zero: This range precedes value.
        /// Zero: This range containes the value.
        /// Greater than zero: This range follows value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="range">The range to test</param>
        /// <param name="value">The value to test</param>
        /// <returns>
        /// Less than zero: This range precedes value.
        /// Zero: This range containes the value.
        /// Greater than zero: This range follows value.
        /// </returns>
        public static int CompareTo<T>(this IRange<T> range, T value)
            where T : IComparable<T>
        {
            if (range.LessThan(value))
            {
                return -1;
            }
            else if (range.GreaterThan(value)) {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Determines if the range is less than the provided value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="range">The range to test</param>
        /// <param name="value">The value to test</param>
        /// <returns>True of the range is less than the value, else false</returns>
        public static bool LessThan<T>(this IRange<T> range, T value)
            where T : IComparable<T>
        {
            var maxInclusive = ((int)range.Inclusivity & 1) == 1; // If the first bit set then max is inclusive
            return maxInclusive ? range.Maximum.CompareTo(value) < 0 : range.Maximum.CompareTo(value) <= 0;
        }

        /// <summary>
        /// Determines if the range is greather than the provided value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="range">The range to test</param>
        /// <param name="value">The value to test</param>
        /// <returns>True if the range is greater than the value, else false</returns>
        public static bool GreaterThan<T>(this IRange<T> range, T value)
            where T : IComparable<T>
        {
            var minInclusive = ((int)range.Inclusivity & 2) == 2; // If the second bit set then min is inclusive
            return minInclusive ? range.Minimum.CompareTo(value) > 0 : range.Minimum.CompareTo(value) >= 0;
        }

        /// <summary>
        /// Determines if the provided value is inside the range
        /// </summary>
        /// <param name="range">The range to test</param>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public static bool Contains<T>(this IRange<T> range, T value)
            where T : IComparable<T>
        {
            var minInclusive = ((int) range.Inclusivity & 2) == 2; // If the second bit set then min is inclusive
            var maxInclusive = ((int) range.Inclusivity & 1) == 1; // If the first bit set then max is inclusive

            var testMin = minInclusive ? range.Minimum.CompareTo(value) <= 0 : range.Minimum.CompareTo(value) < 0;
            var testMax = maxInclusive ? range.Maximum.CompareTo(value) >= 0 : range.Maximum.CompareTo(value) > 0;

            return testMin && testMax;
        }

        /// <summary>
        /// Determines if another range is inside the bounds of this range
        /// </summary>
        /// <param name="range">The range to test</param>
        /// <param name="value">The value to test</param>
        /// <returns>True if range is inside, else false</returns>
        public static bool Contains<T>(this IRange<T> range, IRange<T> value)
            where T : IComparable<T>
        {
            var rangeMinInclusive = ((int)range.Inclusivity & 2) == 2;
            var valueMinInclusive = ((int)value.Inclusivity & 2) == 2;
            var rangeMaxInclusive = ((int)range.Inclusivity & 1) == 1;
            var valueMaxInclusive = ((int)value.Inclusivity & 1) == 1;

            return
                (rangeMinInclusive || rangeMinInclusive == valueMinInclusive) && range.Minimum.CompareTo(value.Minimum) <= 0 &&
                (rangeMaxInclusive || rangeMaxInclusive == valueMaxInclusive) && range.Maximum.CompareTo(value.Maximum) >= 0;
        }

        /// <summary>
        /// Determines if another range intersects with this range.
        /// The either range may completely contain the other
        /// </summary>
        /// <param name="range">The range</param>
        /// <param name="value">The other range</param>
        /// <returns>True of the this range intersects the other range</returns>
        public static bool Intersects<T>(this IRange<T> range, IRange<T> value)
            where T : IComparable<T>
        {
            return
                range.Contains(value.Minimum) || // For when A contains B
                range.Contains(value.Maximum) ||
                value.Contains(range.Minimum) || // For when B contains A
                value.Contains(range.Maximum);
        }

        /// <summary>
        /// Create a union of two ranges so that a new range with the minimum of
        /// the minimum of both ranges and the maximum of the maximum of bother ranges
        /// </summary>
        /// <param name="range">A range with which to union</param>
        /// <param name="value">A range with which to union</param>
        /// <returns>A new range with the minimum of the minimum of both ranges and
        /// the maximum of the maximum of bother ranges</returns>
        public static IRange<T> Union<T>(this IRange<T> range, IRange<T> value)
            where T : IComparable<T>
        {
            return new Range<T>(
                range.Minimum.CompareTo(value.Minimum) < 0 ? range.Minimum : value.Minimum,
                range.Maximum.CompareTo(value.Maximum) > 0 ? range.Maximum : value.Maximum);
        }

        public static IRange<T2> As<T1,T2>(this IRange<T1> range, Func<T1,T2> converter)
            where T1 : IComparable<T1>
            where T2 : IComparable<T2>
        {
            return new Range<T2>(converter(range.Minimum), converter(range.Maximum), range.Inclusivity);
        }
    }
}