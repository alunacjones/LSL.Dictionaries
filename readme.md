[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-dictionaries.svg)](https://ci.appveyor.com/project/alunacjones/lsl-dictionaries)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.Dictionaries)](https://coveralls.io/github/alunacjones/LSL.Dictionaries)
[![NuGet](https://img.shields.io/nuget/v/LSL.Dictionaries.svg)](https://www.nuget.org/packages/LSL.Dictionaries/)

# LSL.Dictionaries

Helpers for dictionaries

The following quick start examples assume the following class definition has been defined:

```csharp
public class MyObject
{
    public int AValue { get; set; }
}
```
## Object Extensions

Convert an object to a dictionary using the `ToDictionary()` object extensions method.

```csharp
using LSL.Dictionaries.Extensions;
...
var theDictionary = new MyObject().ToDictionary();

/*
    theDictionary will contain:

    ["AValue"] = 0
*/
```

## Dictionary Extensions

Convert an `IDictionary<string, object>` to an object:

```csharp
using LSL.Dictionaries.Extensions;
...

var theObject = new Dictionary<string, object>
{
    ["AValue"] = 12
}.ToObject<MyObject>();

/*
    theObject will have AValue set to 12
*/
```
<!-- HIDE -->

## Further Documentation

More in-depth documentation can be found [here](https://alunacjones.github.io/LSL.Dictionaries/)

<!-- END:HIDE -->
