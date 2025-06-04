# Custom property name provider

Sometimes it is desirable to omit certain properties and these can be filtered as shown below:

!!! note
    The `propertyInfo` parameter passed into the delegate provides full information
    about the property in order to make an informed decision about what the property name
    should be.

    You could get information from custom attributes on the property to further
    drive your choice of property name.


```csharp { data-fiddle="mcY2LW" }
var dictionary = new MyObject()
    .ToDictionary(c => c.
        WithPropertyNameProvider(propertyInfo => 
            $"MyPrefix_{propertyInfo.Name}"));

// This will result in a dictionary containing:
// ["MyPrefix_AValue"] = 0
```