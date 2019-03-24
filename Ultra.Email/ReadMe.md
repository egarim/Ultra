
### Ultra Email Module

This module allows you to create and use smtp email accounts inside of your XAF application

### Setup

- Add to your agnostic module project the [nuget package](https://www.nuget.org/packages/Ultra.Email/) that matches your current version of XAF
- Add the module to the require modules (in Module.Designer.cs or Module.Designer.vb) as shown below

### C#
```
private void InitializeComponent()
{		
	this.RequiredModuleTypes.Add(typeof(Ultra.Email.EmailModule));
}
```
### Vb.Net
```
Private Sub InitializeComponent()
	Me.RequiredModuleTypes.Add(GetType(Ultra.Email.EmailModule))
End Sub
```

### Usage
If you want to be able to send an email directly from a detalview or listview you need to implement the interface [IBoToEmail](https://github.com/egarim/Ultra/blob/master/Ultra.Email/IBoToEmail.cs) as shown below


### C#
```
[DefaultClassOptions]
[NavigationItem("Email Module Demo")]
public class EmailObject : BaseObject, IBoToEmail
{
    ...
    public string GetSubject()
    {
        return this.Subject;
    }

    public string GetBody()
    {
        return this.Body;
    }

    public string GetTo()
    {
        return this.To;
    }

    public List<Tuple<string, MemoryStream, ContentType>> GetAttachments()
    {
        return null;
    }

    public SmtpEmailAccount GetEmailAccount()
    {
        return this.Session.FindObject<SmtpEmailAccount>(new BinaryOperator("Name", "Gmail"));
    }

    public string GetCC()
    {
        return this.CC;
    }

    public string GetBCC()
    {
        return this.BCC;
    }
}
```
### Vb.Net
```
<DefaultClassOptions>
<NavigationItem("Email Module Demo")>
Public Class EmailObject
    Inherits BaseObject
    Implements IBoToEmail

    ...
    Public Function GetSubject() As String
        Return Me.Subject
    End Function

    Public Function GetBody() As String
        Return Me.Body
    End Function

    Public Function GetTo() As String
        Return Me.[To]
    End Function

    Public Function GetAttachments() As List(Of Tuple(Of String, MemoryStream, ContentType))
        Return Nothing
    End Function

    Public Function GetEmailAccount() As SmtpEmailAccount
        Return Me.Session.FindObject(Of SmtpEmailAccount)(New BinaryOperator("Name", "Gmail"))
    End Function

    Public Function GetCC() As String
        Return Me.CC
    End Function

    Public Function GetBCC() As String
        Return Me.BCC
    End Function
End Class

```

After you have implented the interface the "Send Email" action will appear in your views
![Email](Email.PNG)


If you want to use the SMTP email accounts from code, there are 2 ways of doing it

- If you want to send emails and display the result you can use SmtpEmailAccountController.XafSendEmail
- If you want to send emails and NOT display the result you can use SmtpEmailAccountController.SendEmailSilently

### Debuging the connection to the SMPT server


