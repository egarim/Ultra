using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultra.Email.BusinessObjects;
using Ultra.Email.Updaters;

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
            SendTestEmail.ImageName = "ActionGroup_EasyTestRecorder";
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

            XafSendEmail(obj, this.Application);
        }

        public static void SendEmailSilently(IBoToEmail IBoToEmail)
        {
            XafSendEmail(IBoToEmail, null);
        }

        public static void XafSendEmail(IBoToEmail IBoToEmail, XafApplication App)
        {
            try
            {
                Tracing.Tracer.LogSeparator("Sending email");
                Tracing.Tracer.LogValue("Name", IBoToEmail.GetEmailAccount().Name);
                Tracing.Tracer.LogValue("Description", IBoToEmail.GetEmailAccount().Description);
                Tracing.Tracer.LogValue("DisableSSLCertificateCheck", IBoToEmail.GetEmailAccount().DisableSSLCertificateCheck);
                Tracing.Tracer.LogValue("EnableSSL", IBoToEmail.GetEmailAccount().EnableSSL);
                Tracing.Tracer.LogValue("SmtpPort", IBoToEmail.GetEmailAccount().SmtpPort);
                Tracing.Tracer.LogValue("SmtpServer", IBoToEmail.GetEmailAccount().SmtpServer);
                Tracing.Tracer.LogValue("UserName", IBoToEmail.GetEmailAccount().UserName);
                Tracing.Tracer.LogValue("UseUsernameAndPassword", IBoToEmail.GetEmailAccount().UseUsernameAndPassword);

                IBoToEmail.GetEmailAccount().SendEmail(IBoToEmail);

                if (App != null)
                {
                    ShowMessage(App, InformationType.Success, CaptionHelper.GetLocalizedText(ModelLocalizationNodesGeneratorUpdater.ModuleName, ModelLocalizationGroupGeneratorUpdater.SuccessMessage)
                , CaptionHelper.GetLocalizedText(ModelLocalizationNodesGeneratorUpdater.ModuleName, ModelLocalizationGroupGeneratorUpdater.SuccessCaption));
                }
                Tracing.Tracer.LogSeparator("Email sent");
            }
            catch (Exception exception)
            {
                Tracing.Tracer.LogError(exception);
                if (App != null)
                {
                    ShowMessage(App, InformationType.Error, CaptionHelper.GetLocalizedText(ModelLocalizationNodesGeneratorUpdater.ModuleName, ModelLocalizationGroupGeneratorUpdater.ErrorMessage)
                  , CaptionHelper.GetLocalizedText(ModelLocalizationNodesGeneratorUpdater.ModuleName, ModelLocalizationGroupGeneratorUpdater.ErrorCaption));
                }
            }
        }

        protected static void ShowMessage(XafApplication App, InformationType Type, string Message, string Caption)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 5000;
            options.Message = Message;
            options.Type = Type;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = Caption;
            options.Win.Type = WinMessageType.Alert;

            App.ShowViewStrategy.ShowMessage(options);
        }
    }
}