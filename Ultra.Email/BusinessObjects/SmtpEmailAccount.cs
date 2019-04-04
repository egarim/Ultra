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
using Ultra.Email;

[DefaultClassOptions]
[NavigationItem("Email")]
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
            return GetPasswordField();
        }
        set
        {
            SetPasswordField(value);
        }
    }

    protected virtual void SetPasswordField(string value)
    {
        SetPropertyValue(nameof(Password), ref password, value);
    }

    protected virtual string GetPasswordField()
    {
        return password;
    }

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
    /// this method will send an email using the parameters from IBoToEmail
    /// </summary>
    /// <param name="Instance">An object that implements IBoToEmail</param>
    public void SendEmail(IBoToEmail Instance)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient(this.SmtpServer);

        mail.From = new MailAddress(Instance.GetFrom());

        mail.To.Add(Instance.GetTo());

        if (Instance.GetCC() != null && Instance.GetCC() != string.Empty)
            mail.CC.Add(Instance.GetCC());
        if (Instance.GetBCC() != null && Instance.GetBCC() != string.Empty)
            mail.Bcc.Add(Instance.GetBCC());
        mail.Subject = Instance.GetSubject();
        mail.Body = Instance.GetBody();

        List<Tuple<string, MemoryStream, ContentType>> Attachments = Instance.GetAttachments();
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
        if (this.UseUsernameAndPassword)
        {
            SmtpServer.Credentials = new System.Net.NetworkCredential(this.UserName, this.Password);
        }

        SmtpServer.EnableSsl = this.EnableSSL;
        if (DisableSSLCertificateCheck)
            DisableCertificateCheck();

        SmtpServer.Send(mail);
    }
}