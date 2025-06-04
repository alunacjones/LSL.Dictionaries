# With no configuration

When no configuration is provided the defaults are as follows:

| Option                 | Behaviour                                                                                                                                                    |
| ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Property Filter        | All `public` properties are added to the dictionary                                                                                                          |
| Value Mapping          | The value of the property is added to the dictionary with no transformation                                                                                  |
| Property Name Provider | The default property name provider sets the dictionary key to the name of the property                                                                       |
| Complex Type Checker   | The default complex type checker does the following: `type.IsClass && !type.IsAssignableFrom(typeof(string)) && !typeof(IEnumerable).IsAssignableFrom(type)` |

The following code would will just use the default configuration when converting an object to a
dictionary.

```csharp
var dictionary = new Thingy().ToDictionary();
```
