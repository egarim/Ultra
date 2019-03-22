using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultra.Email.BusinessObjects;

namespace Ultra.Email.Controllers
{
    public class SmtpEmailAccountController : ViewController
    {
        /// <summary>
        /// <para>Creates an instance of the <see cref="SmtpEmailAccountController"/> class.</para>
        /// </summary>
        public SmtpEmailAccountController()
        {
            this.TargetObjectType = typeof(SmtpEmailAccount);
            PopupWindowShowAction SendTestEmail = new PopupWindowShowAction(this, "SendTestEmail", PredefinedCategory.View);
            SendTestEmail.Caption = "Send Test Email";
            SendTestEmail.CustomizePopupWindowParams += SendTestEmail_CustomizePopupWindowParams;
            SendTestEmail.Execute += SendTestEmail_Execute;
        }

        private IObjectSpace os;
        private TestEmailParameters obj;

        protected virtual void SendTestEmail_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            os = this.Application.CreateObjectSpace();
            obj = os.CreateObject<TestEmailParameters>();
            obj.SmtpEmailAccount = os.GetObject<SmtpEmailAccount>((SmtpEmailAccount)this.View.CurrentObject);
            e.View = Application.CreateDetailView(os, obj);
        }

        protected virtual void SendTestEmail_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Validator.RuleSet.Validate(os, obj, "Save");

            try
            {
                obj.SmtpEmailAccount.SendEmail(obj.To, obj.Subject, obj.Body);
                ShowMessage(InformationType.Success, "Success", "Success");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(string.Format("{0}:{1}", "exception.Message", exception.Message));
                if (exception.InnerException != null)
                {
                    Debug.WriteLine(string.Format("{0}:{1}", "exception.InnerException.Message", exception.InnerException.Message));
                }
                Debug.WriteLine(string.Format("{0}:{1}", " exception.StackTrace", exception.StackTrace));
                ShowMessage(InformationType.Error, exception.Message, "Error");
            }
        }

        protected virtual void ShowMessage(InformationType Type, string Message, string Caption)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 5000;
            options.Message = Message;
            options.Type = Type;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = Caption;
            options.Win.Type = WinMessageType.Alert;

            Application.ShowViewStrategy.ShowMessage(options);
        }
    }
}