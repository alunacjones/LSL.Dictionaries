# Custom property filter

Sometimes it is desirable to omit certain properties and these can be filtered as shown below:

!!! note
    The `propertyInfo` parameter passed into the delegate provides full information
    about the property that is potentially being added to the dictionary

    The `value` parameter is the value of the property.

```csharp  { data-fiddle="LsiRsk" }
var theObject = new Dictionary<string, object>
{
    ["AValue"] = 12
}
.ToObject<MyObject>(c => c.
    WithPropertyFilter((propertyInfo, value) => 
        propertyInfo.Name != "AValue"));

// This will result in a MyObject instance with AValue as 0 
// because we have told it to ignore properties with the name AValue
```
