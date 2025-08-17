# Custom instance factory

You may wish to provide a custom object instance factory. The example below shows how to achieve this.

!!! note
    In this example we are just using what the default configuration uses: `Activator.CreateInstance`

```csharp { data-fiddle="eZF4ly" }
var theObject = new Dictionary<string, object>
{
    ["AValue"] = 12
}
.ToObject<MyObject>(c => c
    .WithInstanceFactory(Activator.CreateInstance));

// This will result in an instance of MyObject with AValue set to 12
```