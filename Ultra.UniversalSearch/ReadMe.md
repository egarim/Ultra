### Universal Search

This module helps you search across all your persistent classes and present a unified search result

### Usage

```csharp

[UniversalSearchAttribute("Name;CustomerID")]
public class Customer:BaseObject
{
	public string Name { get; set; }
	public int CustomerID { get; set; }
	public bool Active { get; set; }
}
```

```vb.net
<UniversalSearchAttribute("Name;CustomerID")>
Public Class Customer
    Inherits BaseObject

    Public Property Name As String
    Public Property CustomerID As Integer
    Public Property Active As Boolean
End Class
```

