using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using netDumbster.smtp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultra.Email.BusinessObjects;
using Ultra.Email.Controllers;

namespace TestAllModules.Modules.Email
{
    [TestFixture]
    public class SmtpEmailAccountControllerTest : TestBase
    {
        private const string AccountNamePort25 = "Port25";

        private SimpleSmtpServer server;

        [SetUp]
        public void SetUp()
        {
            BaseSetup();
            //TEST smtp server https://github.com/cmendible/netDumbster
            server = SimpleSmtpServer.Start(25);

            CreateSampleData(WinApp);
            CreateSampleData(AspApp);
        }

        private void CreateSampleData(XafApplication App)
        {
            var Os = App.CreateObjectSpace();
            var Account = Os.CreateObject<SmtpEmailAccount>();
            Account.SmtpPort = 25;
            Account.Name = AccountNamePort25;
            Account.Description = "Account using port 25 and not auth";
            Account.EnableSSL = false;
            Account.SmtpServer = "localhost";
            Account.UseUsernameAndPassword = false;
            Os.CommitChanges();
        }

        [Test]
        public void When_SendEmailWithoutSslOrAuth_Expect_EmailSent_Win()
        {

            server.ClearReceivedEmail();
            var os = WinApp.CreateObjectSpace();
            var Parameters = os.CreateObject<TestEmailParameters>();
            Parameters.To = "TestAddress@Test.com";
            Parameters.Body = "Hello";
            Parameters.From = "admin@admin.com";
            Parameters.SmtpEmailAccount = os.FindObject<SmtpEmailAccount>(new BinaryOperator(nameof(SmtpEmailAccount.Name), AccountNamePort25));
           
            SmtpEmailAccountController.SendEmailSilently(Parameters);
            //System.Threading.Thread.Sleep(3000);
            //var test=SmtpEmailAccountController.LastException;
            Assert.AreEqual(server.ReceivedEmailCount, 1);
        }
        [Test]
        public void When_SendEmailWithoutSslOrAuth_Expect_EmailSent_Asp()
        {
            server.ClearReceivedEmail();
            var os = AspApp.CreateObjectSpace();
            var Parameters = os.CreateObject<TestEmailParameters>();
            Parameters.To = "TestAddress@Test.com";
            Parameters.Body = "Hello";
            Parameters.From = "admin@admin.com";
            Parameters.SmtpEmailAccount = os.FindObject<SmtpEmailAccount>(new BinaryOperator(nameof(SmtpEmailAccount.Name), AccountNamePort25));

            SmtpEmailAccountController.SendEmailSilently(Parameters);

            Assert.AreEqual(server.ReceivedEmailCount, 1);
        }

        [Test]
        public void When_SendEmailWithWrongToAddress_Expect_LastExceptionNotNull_Win()
        {
            server.ClearReceivedEmail();
            var os = WinApp.CreateObjectSpace();
            var Parameters = os.CreateObject<TestEmailParameters>();
            Parameters.To = "BadEmailAddress";
            Parameters.Body = "Hello";
            Parameters.From = "admin@admin.com";
            Parameters.SmtpEmailAccount = os.FindObject<SmtpEmailAccount>(new BinaryOperator(nameof(SmtpEmailAccount.Name), AccountNamePort25));

            SmtpEmailAccountController.SendEmailSilently(Parameters);

            Assert.NotNull(SmtpEmailAccountController.LastException);
        }

        [Test]
        public void When_SendEmailWithWrongToAddress_Expect_LastExceptionNotNull_Asp()
        {
            server.ClearReceivedEmail();
            var os = AspApp.CreateObjectSpace();
            var Parameters = os.CreateObject<TestEmailParameters>();
            Parameters.To = "BadEmailAddress";
            Parameters.Body = "Hello";
            Parameters.From = "admin@admin.com";
            Parameters.SmtpEmailAccount = os.FindObject<SmtpEmailAccount>(new BinaryOperator(nameof(SmtpEmailAccount.Name), AccountNamePort25));

            SmtpEmailAccountController.SendEmailSilently(Parameters);

            Assert.NotNull(SmtpEmailAccountController.LastException);
        }


    }
}