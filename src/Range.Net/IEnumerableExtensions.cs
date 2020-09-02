using Range.Net.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Range.Net
{
    public static class IEnumerableExtensions
    {
        public static IRange<TProperty> Range<TValue, TProperty>(
            this IEnumerable<TValue> enumerable,
            Func<TValue, TProperty> propertySelector,
            RangeInclusivity inclusivity = RangeInclusivity.InclusiveMinInclusiveMax) where TProperty : IComparable<TProperty>
        {
            var minmax = enumerable.Aggregate(
                new MutablePair<TProperty>(),
                (agg, val) => {
                    var propertyValue = propertySelector(val);
                    agg.Value1 = propertyValue.CompareTo(agg.Value1) < 0 ? propertyValue : agg.Value1;
                    agg.Value2 = propertyValue.CompareTo(agg.Value2) > 0 ? propertyValue : agg.Value2;
                    return agg;
                }
            );
            return new Range<TProperty>(
                minmax.Value1,
                minmax.Value2,
                inclusivity
            );
        }

        private class MutablePair<T>
        {
            public T Value1 { get; set; }
            public T Value2 { get; set; }
        }
    }
}
