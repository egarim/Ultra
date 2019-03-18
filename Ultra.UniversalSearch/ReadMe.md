### Universal Search

This module helps you search across all your persistent classes and present a unified search result


### Setup

- Add to your agnostic module project the nuget package that matches your current version of XAF
- Add the module to the require modules (in Module.Designer.cs or Module.Designer.vb) as shown below

### C#
```
private void InitializeComponent()
{		
	this.RequiredModuleTypes.Add(typeof(Ultra.UniversalSearch.UniversalSearchModule));
}
```
### Vb.Net
```
Private Sub InitializeComponent()
	Me.RequiredModuleTypes.Add(GetType(Ultra.UniversalSearch.UniversalSearchModule))
End Sub
```

### Usage
### C#
```
[UniversalSearchAttribute("Code;Name", "Country Code:{0} - Country Name:{1}")]
public class Customer:BaseObject
{
	public string Name { get; set; }
	public int CustomerID { get; set; }
	public bool Active { get; set; }
}
```
### Vb.Net
```
<UniversalSearchAttribute("Code;Name", "Country Code:{0} - Country Name:{1}")>
Public Class Customer
	Inherits BaseObject

	Public Property Name As String
	Public Property CustomerID As Integer
	Public Property Active As Boolean
End Class
```

### Demo
![Ultra Universal Search](UltraUniversalSearch.gif)
