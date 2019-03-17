using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultra.UniversalSearch.BusinessObjects;

namespace Ultra.MainDemo.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Name")]
    [UniversalSearchAttribute("Code;Name")]
    public class Customer : BaseObject
    {
        /// <summary>
        /// <para>Used to initialize a new instance of a <see cref="Customer"/> descendant, in a particular Session.</para>
        /// </summary>
        /// <param name="session">A DevExpress.Xpo.Session object which represents a persistent object&#39;s cache where the business object will be instantiated.</param>
        public Customer(Session session) : base(session)
        {
        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="Customer"/> class.</para>
        /// </summary>
        public Customer()
        {
        }

        private Country country;

        private string name;
        private string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Association("Country-Customers")]
        public Country Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }
    }

    [DefaultClassOptions]
    [DefaultProperty("Name")]
    [UniversalSearchAttribute("Code;Name")]
    public class Country : BaseObject
    {
        /// <summary>
        /// <para>Used to initialize a new instance of a <see cref="Customer"/> descendant, in a particular Session.</para>
        /// </summary>
        /// <param name="session">A DevExpress.Xpo.Session object which represents a persistent object&#39;s cache where the business object will be instantiated.</param>
        public Country(Session session) : base(session)
        {
        }

        /// <summary>
        /// <para>Creates a new instance of the <see cref="Customer"/> class.</para>
        /// </summary>
        public Country()
        {
        }

        private string name;
        private string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Association("Country-Customers")]
        public XPCollection<Customer> Customers
        {
            get
            {
                return GetCollection<Customer>(nameof(Customers));
            }
        }
    }
}