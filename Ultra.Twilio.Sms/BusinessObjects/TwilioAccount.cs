using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Ultra.Twilio.Sms.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Twilio Sms")]
    [ImageName("Actions_EnvelopeClose")]
    [DefaultProperty("Name")]
    public class TwilioAccount : BaseObject
    {
        /// <summary>
        /// <para>Used to initialize a new instance of a <see cref="TwilioAccount"/> descendant, in a particular Session.</para>
        /// </summary>
        /// <param name="session">A DevExpress.Xpo.Session object which represents a persistent object&#39;s cache where the business object will be instantiated.</param>
        public TwilioAccount(Session session) : base(session)
        {
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Size(300)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="TwilioAccount"/> class.</para>
        /// </summary>
        public TwilioAccount()
        {
        }

        private string description;
        private string name;
        private string accountToken;
        private string accountSid;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string AccountSid
        {
            get => accountSid;
            set => SetPropertyValue(nameof(AccountSid), ref accountSid, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string AccountToken
        {
            get => accountToken;
            set => SetPropertyValue(nameof(AccountToken), ref accountToken, value);
        }

        [Association("TwilioAccount-TwilioPhoneNumbers"), DevExpress.Xpo.Aggregated()]
        public XPCollection<TwilioPhoneNumber> TwilioPhoneNumbers
        {
            get
            {
                return GetCollection<TwilioPhoneNumber>(nameof(TwilioPhoneNumbers));
            }
        }
    }
}