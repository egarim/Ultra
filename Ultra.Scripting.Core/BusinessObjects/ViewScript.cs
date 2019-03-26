using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace Ultra.Scripting.Core.BusinessObjects
{
    [NavigationItem("Scripting")]
    [DefaultClassOptions]
    [ImageName("Action_ShowScript")]
    public class ViewScript : Script
    {
        public ViewScript(Session session) : base(session)
        {
        }

        private string entryMethod;
        private string entryType;
        private string viewId;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ViewId
        {
            get => viewId;
            set => SetPropertyValue(nameof(ViewId), ref viewId, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EntryType
        {
            get => entryType;
            set => SetPropertyValue(nameof(EntryType), ref entryType, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EntryMethod
        {
            get => entryMethod;
            set => SetPropertyValue(nameof(EntryMethod), ref entryMethod, value);
        }

        public void ExecuteAssemblyCode(View XafView)
        {
            Type type = this.GetAssembly().GetType(this.EntryType);
            object obj = Activator.CreateInstance(type);
            type.InvokeMember(this.EntryMethod,
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                obj,
                new object[] { XafView });
        }
    }
}