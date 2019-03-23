using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.Email.Controllers
{
    public class SendEmailController : ViewController
    {
        private SimpleAction sendEmail;

        public SendEmailController()
        {
            this.TargetObjectType = typeof(IBoToEmail);
            sendEmail = new SimpleAction(this, "SendEmail", PredefinedCategory.View);
            sendEmail.Caption = "Send Email";
            sendEmail.ImageName = @"Glyph_Mail";
            sendEmail.Execute += SendEmail_Execute;
        }

        public SimpleAction SendEmail { get => sendEmail; set => sendEmail = value; }

        protected virtual void SendEmail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SmtpEmailAccountController.XafSendEmail(this.View.CurrentObject as IBoToEmail, this.Application);
        }
    }
}