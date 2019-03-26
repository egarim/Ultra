using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Ultra.Scripting.Core.BusinessObjects;

namespace Ultra.Scripting.Core.Controllers
{
    public class ScriptHookController : ViewController
    {
        public ScriptHookController()
        {
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            var CurrentScripts = this.View.ObjectSpace.GetObjects<ViewScript>(new BinaryOperator("ViewId", this.View.Id));
            foreach (var item in CurrentScripts)
            {
                item.ExecuteAssemblyCode(this.View);
            }

            // Perform various tasks depending on the target View.
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}