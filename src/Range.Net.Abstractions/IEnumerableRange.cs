using System;
using System.Collections.Generic;

namespace Range.Net.Abstractions
{
    public interface IEnumerableRange<T> : IRange<T>, IEnumerable<T> where T : IComparable<T> { }
}