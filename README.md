# Range.NET
[![Build Status](https://atlanticblue.visualstudio.com/Range.NET/_apis/build/status/mnelsonwhite.Range.NET?branchName=master)](https://atlanticblue.visualstudio.com/Range.NET/_build/latest?definitionId=2&branchName=master)
[![Nuget](https://img.shields.io/badge/nuget-Range.Net-blue.svg)](https://www.nuget.org/packages/Range.Net)

Range allows comparable types to be used as a range.

## Basic Usage

### Create a new range

``` c#
var range = new Range<int>(1,10);

Assert.Equal(1, range.Minimum);
Assert.Equal(10, range.Maximum);
```

### Range contains value

``` c#
var range = new Range<int>(1,10);

Assert.True(range.Contains(1));
Assert.True(range.Contains(3));
Assert.True(range.Contains(10));
Assert.False(range.Contains(0));
Assert.False(range.Contains(11));
```

### Range contains another

``` c#
var range1 = new Range<int>(1,10);
var range2 = new Range<int>(4,5);

Assert.True(range1.Contains(range2));
Assert.False(range2.Contains(range1));
```

### Range intersects with another

``` c#
var range = new Range<int>(1,10);

Assert.True(range.Intersects(new Range<int>(0,2)));
Assert.True(range.Intersects(new Range<int>(0,11)));
Assert.True(range.Intersects(new Range<int>(9,11)));
Assert.True(range.Intersects(new Range<int>(1,9)));
Assert.False(range.Intersects(new Range<int>(-1,0)));
Assert.False(range.Intersects(new Range<int>(11,12)));
```

### Union a range with another

``` c#
var range1 = new Range<int>(1,6);
var range2 = new Range<int>(3,10);

var union = range1.Union(range2);

Assert.Equal(1, union.Minimum);
Assert.Equal(10, union.Maximum);
```

### Greater/Less Than

``` c#
var range = new Range<int>(4,6);

Assert.True(range.LessThan(7));
Assert.False(range.LessThan(5));
Assert.True(range.GreaterThan(3));
Assert.False(range.GreaterThan(4));
```

### Compare To

``` c#
var range1 = new Range<int>(4,6);

Assert.Equal(1, range1.CompareTo(1));
Assert.Equal(0, range1.CompareTo(4));
Assert.Equal(0, range1.CompareTo(5));
Assert.Equal(0, range1.CompareTo(6));
Assert.Equal(-1, range1.CompareTo(7));
```

## Advanced Usage


### Inclusivity/Exclusivity
A range may specify inclusivity/exclusivity of the minimum and maximum values using the `Inclusivity` property. `InclusiveMinInclusiveMax` the default used by `Range`. 

``` c#

var range1 = new Range<int>(1,10) { Inclusivity = RangeInclusivity.ExclusiveMinExclusiveMax };
var range2 = new Range<int>(1,10) { Inclusivity = RangeInclusivity.ExclusiveMinInclusiveMax };
var range3 = new Range<int>(1,10) { Inclusivity = RangeInclusivity.InclusiveMinExclusiveMax };
var range4 = new Range<int>(1,10) { Inclusivity = RangeInclusivity.InclusiveMinInclusiveMax };

Assert.False(range1.Contains(1));
Assert.False(range1.Contains(10));

Assert.False(range2.Contains(1));
Assert.True(range2.Contains(10));

Assert.True(range3.Contains(1));
Assert.False(range3.Contains(10));

Assert.True(range4.Contains(1));
Assert.True(range4.Contains(10));
```

### IQueryable filtering

The library includes extensions to `IQueryable` which can be used with entity framework to filter a property by a range of the same type. This will automatically handle inclusivity.

``` c#
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
```
