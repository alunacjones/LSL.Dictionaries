# Custom property filter

Sometimes it is desirable to omit certain properties and these can be filtered as shown below:

!!! note
    The `propertyInfo` parameter passed into the delegate provides full information
    about the property that is potentially being added to the dictionary

    The `value` parameter is the value of the property.

```csharp { data-fiddle="Yk2nOY" }
var dictionary = new MyObject()
    .ToDictionary(c => c.
        WithPropertyFilter((propertyInfo, value) => 
            propertyInfo.Name != "AValue"));

// dictionary will be empty as MyObject only has AValue as a property
```