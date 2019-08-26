using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Ultra.Twilio.Sms.BusinessObjects;

namespace Ultra.Twilio.Sms.Controllers
{
    public class TwilioAccountController : ViewController
    {
        private SimpleAction syncPhoneNumbers;

        public TwilioAccountController()
        {
            this.TargetObjectType = typeof(TwilioAccount);
            this.TargetViewType = ViewType.DetailView;
            this.syncPhoneNumbers = new SimpleAction(this, "SyncPhoneNumbers", PredefinedCategory.View);
            syncPhoneNumbers.Caption = "Sync Phone Numbers";
            //syncPhoneNumbers.ImageName = "BO_Unknow";
            syncPhoneNumbers.Execute += SyncPhoneNumbers_Execute;
        }

        public SimpleAction SyncPhoneNumbers { get => syncPhoneNumbers; set => syncPhoneNumbers = value; }

        private void SyncPhoneNumbers_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var CurrentAccount = (TwilioAccount)this.View.CurrentObject;
            TwilioClient.Init(CurrentAccount.AccountSid, CurrentAccount.AccountToken);

            var incomingPhoneNumbers = IncomingPhoneNumberResource.Read();
            foreach (IncomingPhoneNumberResource item in incomingPhoneNumbers)
            {
                var CurrentPhone = CurrentAccount.TwilioPhoneNumbers.Where(phone => phone.Sid == item.Sid).FirstOrDefault();
                if (CurrentPhone == null)
                {
                    CurrentPhone = this.View.ObjectSpace.CreateObject<TwilioPhoneNumber>();
                    CurrentPhone.Sid = item.Sid;

                    CurrentPhone.PhoneNumber = item.PhoneNumber.ToString();
                    CurrentPhone.TwilioAccount = CurrentAccount;
                }
                CurrentPhone.FriendlyName = item.FriendlyName;
            }
            this.View.ObjectSpace.CommitChanges();
        }
    }
}