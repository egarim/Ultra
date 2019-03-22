using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

[DefaultClassOptions]
[NavigationItem("System")]
[ImageName("Actions_EnvelopeClose")]
[DefaultProperty("Name")]
public class SmtpEmailAccount : BaseObject
{ // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
    public SmtpEmailAccount(Session session)
        : base(session)
    {
    }

    /// <summary>
    /// <para>Creates a new instance of the <see cref="SmtpEmailAccount"/> class.</para>
    /// </summary>
    public SmtpEmailAccount()
    {
    }

    public override void AfterConstruction()
    {
        base.AfterConstruction();
        // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
    }

    [RuleRequiredField("SmtpEmailAccount Name RuleRequiredField",
     DefaultContexts.Save,
     CustomMessageTemplate = "Name is required")]
    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Name
    {
        get => name;
        set => SetPropertyValue(nameof(Name), ref name, value);
    }

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Description
    {
        get => description;
        set => SetPropertyValue(nameof(Description), ref description, value);
    }

    private bool useUsernameAndPassword;
    private int smtpPort;
    private bool disableSSLCertificateCheck;
    private string description;
    private string name;
    private string smtpServer;

    [RuleRequiredField("SmtpEmailAccount SmtpServer RuleRequiredField",
     DefaultContexts.Save,
     CustomMessageTemplate = "Smtp server is required")]
    public string SmtpServer
    {
        get { return smtpServer; }
        set
        {
            if (smtpServer == value)
                return;
            smtpServer = value;
            RaisePropertyChangedEvent(nameof(SmtpServer));
        }
    }

    [RuleRequiredField("SmtpEmailAccount SmtpPort RuleRequiredField",
     DefaultContexts.Save,
     CustomMessageTemplate = "Smtp port is required")]
    public int SmtpPort
    {
        get => smtpPort;
        set => SetPropertyValue(nameof(SmtpPort), ref smtpPort, value);
    }

    private bool enableSSL;

    public bool EnableSSL
    {
        get
        {
            return enableSSL;
        }
        set
        {
            SetPropertyValue("EnableSSL", ref enableSSL, value);
        }
    }

    public bool DisableSSLCertificateCheck
    {
        get => disableSSLCertificateCheck;
        set => SetPropertyValue(nameof(DisableSSLCertificateCheck), ref disableSSLCertificateCheck, value);
    }

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    private string userName;

    public bool UseUsernameAndPassword
    {
        get => useUsernameAndPassword;
        set => SetPropertyValue(nameof(UseUsernameAndPassword), ref useUsernameAndPassword, value);
    }

    [Appearance("SmtpEmailAccount Username", Criteria = "UseUsernameAndPassword = false", Enabled = false)]
    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string UserName
    {
        get
        {
            return userName;
        }
        set
        {
            SetPropertyValue("UserName", ref userName, value);
        }
    }

    private string password;

    [Appearance("SmtpEmailAccount Password", Criteria = "UseUsernameAndPassword = false", Enabled = false)]
    [ModelDefault("IsPassword", "true")]
    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Password
    {
        get
        {
            return password;
        }
        set
        {
            SetPropertyValue("Password", ref password, value);
        }
    }

    [Obsolete("Do not use this in Production code!!!", false)]
    private static void DisableCertificateCheck()
    {
        // Disabling certificate validation can expose you to a man-in-the-middle attack
        // which may allow your encrypted message to be read by an attacker
        // https://stackoverflow.com/a/14907718/740639
        //https://stackoverflow.com/questions/777607/the-remote-certificate-is-invalid-according-to-the-validation-procedure-using
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (
                object s,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
            {
                return true;
            };
    }

    /// <summary>
    /// Send an email to the specified addresses
    /// </summary>
    /// <param name="AllAddress">A list of address separated by coma</param>
    /// <param name="Subject">The subject of the email</param>
    /// <param name="Body">the body of the email</param>
    public void SendEmail(string AllAddress, string Subject, string Body)
    {
        var Addresses = AllAddress.Split(',').ToList();
        SendEmail(Addresses, Subject, Body, null);
    }

    /// <summary>
    /// Send an email to the specified addresses
    /// </summary>
    /// <param name="AllAddress">A list of address</param>
    /// <param name="Subject">The subject of the email</param>
    /// <param name="Body">the body of the email</param>
    /// <param name="Attachments">A list of tuple with 2 items first the file name, second the memory stream with the content and third the content type</param>
    public void SendEmail(List<string> AllAddress, string Subject, string Body, List<Tuple<string, MemoryStream, ContentType>> Attachments)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient(this.SmtpServer);

        mail.From = new MailAddress(this.UserName);

        var Addresses = string.Join(",", AllAddress);
        mail.To.Add(Addresses);

        mail.Subject = Subject;
        mail.Body = Body;

        if (Attachments != null)
        {
            foreach (Tuple<string, MemoryStream, ContentType> CurrentObject in Attachments)
            {
                Attachment attach = new Attachment(CurrentObject.Item2, CurrentObject.Item3);
                attach.ContentDisposition.FileName = CurrentObject.Item1;
                mail.Attachments.Add(attach);
            }
        }

        SmtpServer.Port = this.SmtpPort;
        SmtpServer.Credentials = new System.Net.NetworkCredential(this.UserName, this.Password);
        SmtpServer.EnableSsl = this.EnableSSL;
        if (DisableSSLCertificateCheck)
            DisableCertificateCheck();

        SmtpServer.Send(mail);
    }
}