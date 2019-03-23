using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Ultra.Email;

namespace Ultra.MainDemo.Module.BusinessObjects.EmailModule
{
    [DefaultClassOptions]
    [NavigationItem("Email Module Demo")]
    public class EmailObject : BaseObject, IBoToEmail
    {
        /// <summary>
        /// <para>Used to initialize a new instance of a <see cref="EmailObject"/> descendant, in a particular Session.</para>
        /// </summary>
        /// <param name="session">A DevExpress.Xpo.Session object which represents a persistent object&#39;s cache where the business object will be instantiated.</param>
        public EmailObject(Session session) : base(session)
        {
        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="EmailObject"/> class.</para>
        /// </summary>
        public EmailObject()
        {
        }

        private string bCC;
        private string cC;
        private string to;
        private string body;
        private string subject;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Subject
        {
            get => subject;
            set => SetPropertyValue(nameof(Subject), ref subject, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Body
        {
            get => body;
            set => SetPropertyValue(nameof(Body), ref body, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string To
        {
            get => to;
            set => SetPropertyValue(nameof(To), ref to, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CC
        {
            get => cC;
            set => SetPropertyValue(nameof(CC), ref cC, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BCC
        {
            get => bCC;
            set => SetPropertyValue(nameof(BCC), ref bCC, value);
        }

        public string GetSubject()
        {
            return this.Subject;
        }

        public string GetBody()
        {
            return this.Body;
        }

        public string GetTo()
        {
            return this.To;
        }

        public List<Tuple<string, MemoryStream, ContentType>> GetAttachments()
        {
            return null;
        }

        public SmtpEmailAccount GetEmailAccount()
        {
            throw new NotImplementedException();
        }

        public string GetCC()
        {
            return this.CC;
        }

        public string GetBCC()
        {
            return this.BCC;
        }
    }
}