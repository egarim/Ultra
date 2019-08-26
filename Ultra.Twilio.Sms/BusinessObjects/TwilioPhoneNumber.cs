using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Ultra.Twilio.Sms.BusinessObjects
{
    public class TwilioPhoneNumber : XPObject
    {
        public TwilioPhoneNumber(Session session) : base(session)
        { }

        private string sid;
        private string friendlyName;
        private TwilioAccount twilioAccount;
        private string phoneNumber;

        [ModelDefault("AllowEdit", "false")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value);
        }

        [Association("TwilioAccount-TwilioPhoneNumbers")]
        public TwilioAccount TwilioAccount
        {
            get => twilioAccount;
            set => SetPropertyValue(nameof(TwilioAccount), ref twilioAccount, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FriendlyName
        {
            get => friendlyName;
            set => SetPropertyValue(nameof(FriendlyName), ref friendlyName, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Sid
        {
            get => sid;
            set => SetPropertyValue(nameof(Sid), ref sid, value);
        }
    }

    public class TwilioPhoneNumberLog : BaseObject
    {
        public TwilioPhoneNumberLog(Session session) : base(session)
        { }

        private string log;
        private DateTime date;
        private TwilioPhoneNumber twilioPhoneNumber;
        private TwilioAccount twilioAccount;

        [Association("TwilioAccount-TwilioPhoneNumberLogs")]
        public TwilioAccount TwilioAccount
        {
            get => twilioAccount;
            set => SetPropertyValue(nameof(TwilioAccount), ref twilioAccount, value);
        }

        [Association("TwilioPhoneNumber-TwilioPhoneNumberLogs")]
        public TwilioPhoneNumber TwilioPhoneNumber
        {
            get => twilioPhoneNumber;
            set => SetPropertyValue(nameof(TwilioPhoneNumber), ref twilioPhoneNumber, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetPropertyValue(nameof(Date), ref date, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Log
        {
            get => log;
            set => SetPropertyValue(nameof(Log), ref log, value);
        }
    }
}