using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.UniversalSearch
{
    [ModelDefault("Caption", "Search")]
    [DomainComponent]
    [NavigationItem("Universal Search")]
    public class UniversalSearchResult
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public string ObjectDisplayName { get; set; }
        public string Display { get; set; }

        [Browsable(false)]
        public Type ObjectType { get; set; }

        [Browsable(false)]
        public string ObjectKey { get; set; }
    }
}