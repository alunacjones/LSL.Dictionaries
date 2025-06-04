# Custom value mapping

When mapping a value from dictionary's value it may be desirable to format the value prior to it being set on the object. The following example shows a simple value mapper that will double any integer values that go into an object's `int` properties

!!! note
    The `propertyInfo` parameter passed into the delegate provides full information
    about the property whose value will be added to the dictionary.

    The `value` parameter is the value of the property.

```csharp  { data-fiddle="Hx4GBH" }
var theObject = new Dictionary<string, object>
{
    ["AValue"] = 12
}
.ToObject<MyObject>(c => c
    .WithValueMapper((propertyInfo, value) => value switch
    {
        int intValue => intValue * 2,
        _ => value						
    }));

// theObject's AValue property will be 24
```