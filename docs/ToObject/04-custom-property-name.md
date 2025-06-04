# Custom property name provider

You may have a dictionary whose key names do not match properties on the object. This method allows you to provide a custom dictionary key.

!!! note
    The `propertyInfo` parameter passed into the delegate provides full information
    about the property in order to make an informed decision about what the dictionary key 
    to lookup should be (for a specific property of the target object) 

    You could get information from custom attributes on the property to further
    drive your choice of dictionary key.

```csharp { data-fiddle="greH3K" }
var theObject = new Dictionary<string, object>
{
    ["Prefix_AValue"] = 12
}
.ToObject<MyObject>(c => c.
    WithPropertyNameProvider(propertyInfo => 
        $"Prefix_{propertyInfo.Name}"));

// This will result in an instance of MyObject with AValue set to 12
```