[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-dictionaries.svg)](https://ci.appveyor.com/project/alunacjones/lsl-dictionaries)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.Dictionaries)](https://coveralls.io/github/alunacjones/LSL.Dictionaries)
[![NuGet](https://img.shields.io/nuget/v/LSL.Dictionaries.svg)](https://www.nuget.org/packages/LSL.Dictionaries/)

# LSL.Dictionaries

Helpers for dictionaries. Currently supports mapping an object to a dictionary and a dictionary into an object.

<!-- HIDE -->

## Further Documentation

More in-depth documentation can be found [here](https://alunacjones.github.io/LSL.Dictionaries/)

<!-- END:HIDE -->

## Assumed class definitions

The following quick start examples assume the following class definitions have been defined:

```csharp
public class MyObject
{
    public int AValue { get; set; }
    public Inner Inner { get; set; } = new Inner();
}

public class Inner
{
    public string Name { get; set; }
}
```

## Object Extensions

Convert an object to a dictionary using the `ToDictionary()` object extensions method.

```csharp { data-fiddle="nkAdvw" }
using LSL.Dictionaries.Extensions;
...
var theDictionary = new MyObject()
    {
        AValue = 12,
        Inner = new Inner
        {
            Name = "Als"
        }
    }
    .ToDictionary();

/*
    theDictionary will contain:

    ["AValue"] = 12
    ["Inner"] = new Dictionary<string, object>
    {
        ["Name"] = "Als"
    }
*/
```

## Dictionary Extensions

Convert an `IDictionary<string, object>` to an object:

```csharp { data-fiddle="jQIAC7" } 
using LSL.Dictionaries.Extensions;
...

var theObject = new Dictionary<string, object>
{
    ["AValue"] = 12,
    ["Inner"] = new Dictionary<string, object>
    {
        ["Name"] = "Als"
    }
}.ToObject<MyObject>();

/*
    theObject will be:
    {
        AValue = 12,
        Inner = {
            Name = "Als"
        }
    }
*/
```
