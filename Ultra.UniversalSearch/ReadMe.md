
This module helps you search across all your persistent classes and present a unified search result


### Setup

- Add to your agnostic module project the [nuget package](https://nuget.bitframeworks.com/feeds/main/Ultra.UniversalSearch/19.1.5.1) from https://nuget.bitframeworks.com/nuget/main/ that matches your current version of XAF

	With the Nuget.exe Client: From the command line, run the following command:
	```
	nuget install Ultra.UniversalSearch -Version 19.1.5.1 -Source https://nuget.bitframeworks.com/nuget/main/
	```

	To install Ultra.UniversalSearch from the Package Manager Console within Visual Studio, run the following command:
	```
	Install-Package Ultra.UniversalSearch -Version 19.1.5.1 -Source https://nuget.bitframeworks.com/nuget/main/
	```
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



### Note

As we can see in the UniversalSearchAttribute definition bellow the DisplayProperties, and DisplayPropertiesStringFormat parameters are just for presentation purposes once the search has successfully found results. The actual search function will iterate over every property of your class and match your search term accordingly.

```
public UniversalSearchAttribute(string DisplayProperties, string DisplayPropertiesStringFormat)
		{
			Guard.ArgumentNotNullOrEmpty(DisplayProperties, "DisplayProperties");
			Guard.ArgumentNotNullOrEmpty(DisplayPropertiesStringFormat, "DisplayPropertiesStringFormat");
			this.DisplayProperties = DisplayProperties;
			this.DisplayPropertiesStringFormat = DisplayPropertiesStringFormat;
		}
```

If you want to explore more take a peek at the method ICollection<String> GetFullTextSearchProperties(ITypeInfo TypeInfo) where all the magic happens.

### Tip 

When using composition in your DisplayProperties make sure to also include the property or field you want to actually be displayed. This will avoid showing in your search results an ugly looking Oid string.  Example: Customer.Name instead of Customer will suffice.

```
[UniversalSearchAttribute("InvoiceNumber;Customer.Name", "Invoice Number:{0} - Customer:{1}")]
    public class Invoice : BaseObject
    {  
        
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }
        
        public string InvoiceNumber
        {
            get => invoiceNumber;
            set => SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value);
        }
    }

```
