# Quick Start

When no configuration is provided the defaults are as follows:

| Option                                                 | Behaviour                                                                              |
| ------------------------------------------------------ | -------------------------------------------------------------------------------------- |
| [Property Filter]                                      | All `public` properties are set on the object if found in the dictionary               |
| [Value Mapping]                                        | The value from the dictionary sets the property on the object with no transformation   |
| [Property Name Provider] | The default property name provider sets the dictionary key to the name of the property |

The following code would will just use the default configuration when converting an object to a
dictionary.

```csharp { data-fiddle="zNzuYm" }
var theObject = new Dictionary<string, object>()
    .ToObject();

// This will result in a dictionary containing:
// ["AValue"] = 0
```

[Property Filter]: ./02-custom-property-filter.md
[Value Mapping]: ./03-custom-value-mapping.md
[Property Name Provider]: ./04-custom-property-name.md