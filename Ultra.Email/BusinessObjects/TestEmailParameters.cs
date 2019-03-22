﻿using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.Email.BusinessObjects
{
    [NonPersistent()]
    public class TestEmailParameters : BaseObject
    {
        private SmtpEmailAccount smtpEmailAccount;
        private string body;
        private string subject;
        private string to;

        /// <summary>
        /// <para>Used to initialize a new instance of a <see cref="TestEmailParameters"/> descendant, in a particular Session.</para>
        /// </summary>
        /// <param name="session">A DevExpress.Xpo.Session object which represents a persistent object&#39;s cache where the business object will be instantiated.</param>
        public TestEmailParameters(Session session) : base(session)
        {
        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="TestEmailParameters"/> class.</para>
        /// </summary>
        public TestEmailParameters()
        {
        }

        [RuleRequiredField("TestEmailParameters SmtpEmailAccount RuleRequiredField",
         DefaultContexts.Save)]
        public SmtpEmailAccount SmtpEmailAccount
        {
            get => smtpEmailAccount;
            set => SetPropertyValue(nameof(SmtpEmailAccount), ref smtpEmailAccount, value);
        }

        [RuleRequiredField("TestEmailParameters SmtpEmailAccount To",
        DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string To
        {
            get => to;
            set => SetPropertyValue(nameof(To), ref to, value);
        }

        [RuleRequiredField("TestEmailParameters SmtpEmailAccount Subject",
          DefaultContexts.Save)]
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Subject
        {
            get => subject;
            set => SetPropertyValue(nameof(Subject), ref subject, value);
        }

        [RuleRequiredField("TestEmailParameters SmtpEmailAccount Body",
         DefaultContexts.Save)]
        [Size(300)]
        public string Body
        {
            get => body;
            set => SetPropertyValue(nameof(Body), ref body, value);
        }
    }
}