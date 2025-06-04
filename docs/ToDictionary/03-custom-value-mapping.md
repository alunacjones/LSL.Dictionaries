# Custom value mapping

When mapping a value from an object's property it may be desirable to format the value prior to it being added to the dictionary. The following example shows a simple value mapper that will format a date time as `dd-MMM-yyyy`

!!! note
    The `propertyInfo` parameter passed into the delegate provides full information
    about the property whose value will be added to the dictionary.

    The `value` parameter is the value of the property.

```csharp  { data-fiddle="6lJ5Xv" }
var dictionary = new MyObject()
    .ToDictionary(c => c
        .WithValueMapper((propertyInfo, value) => value switch
        {
            DateTime dateValue => dateValue.ToString("dd-MMM-yyyy"),
            _ => value						
        }));

// Any DateTime properties will be formatted as configured above.
// e.g. 12-Mar-2010 for DateTime(2010, 3, 12)
```