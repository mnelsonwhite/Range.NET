using Range.Net.Abstractions;
using System;

namespace Range.Net
{
    public static class ComparableExtensions
    {
        public static IRange<T> To<T>(
            this T from,
            T to,
            RangeInclusivity inclusivity = RangeInclusivity.InclusiveMinInclusiveMax
        )
            where T : IComparable<T>
        {
            return new Range<T>(from, to, inclusivity);
        }
    }
}
