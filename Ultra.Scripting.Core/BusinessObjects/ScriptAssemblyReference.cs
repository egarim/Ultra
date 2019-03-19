using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Ultra.Scripting.Core.BusinessObjects
{
    //[ImageName("BO_Contact")]
    [DefaultProperty("AssemblyName")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ScriptAssemblyReference : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ScriptAssemblyReference(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        // Fields...
        private string assemblyName;

        private string _AssemblyFullName;

        private string _AssemblyPath;

        private Script _Script;

        [Association("Script-ScriptAssemblyReferences")]
        public Script Script
        {
            get
            {
                return _Script;
            }
            set
            {
                SetPropertyValue("Script", ref _Script, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string AssemblyName
        {
            get => assemblyName;
            set => SetPropertyValue(nameof(AssemblyName), ref assemblyName, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string AssemblyFullName
        {
            get
            {
                return _AssemblyFullName;
            }
            set
            {
                SetPropertyValue("AssemblyFullName", ref _AssemblyFullName, value);
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string AssemblyPath
        {
            get
            {
                return _AssemblyPath;
            }
            set
            {
                SetPropertyValue("AssemblyPath", ref _AssemblyPath, value);
            }
        }
    }
}