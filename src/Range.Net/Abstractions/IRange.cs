using System;

namespace Range.Net.Abstractions
{
    public interface IRange<out T> where T : IComparable<T>
    {
        T Minimum { get; }
        T Maximum { get; }
        RangeInclusivity Inclusivity { get; }
    }
}