using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.XtraRichEdit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevExpress.CodeParser;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraRichEdit;

using DevExpress.Office;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Fields;
using System.Drawing;
using System.Diagnostics;

using System.Windows.Forms;
using Ultra.Scripting.Core.BusinessObjects;

namespace Ultra.Scripting.Core.Win
{
    [PropertyEditor(typeof(string), false)]
    public class CodeEditor : PropertyEditor//, IInplaceEditSupport
    {
        private MySyntaxHighlightService _SyntaxHigLighter;
        private DevExpress.XtraRichEdit.RichEditControl control = null;

        protected override void ReadValueCore()
        {
            if (control != null)
            {
                if (CurrentObject != null)
                {
                    control.ReadOnly = false;
                    control.Text = (string)PropertyValue;
                }
                else
                {
                    control.ReadOnly = true;
                    control.Text = string.Empty;
                }
            }
        }

        private void control_ValueChanged(object sender, EventArgs e)
        {
            if (!IsValueReading)
            {
                OnControlValueChanged();
                WriteValueCore();
            }
        }

        protected override object CreateControlCore()
        {
            control = new DevExpress.XtraRichEdit.RichEditControl();
            Script CurrentObject = (Script)this.CurrentObject;
            this.CurrentObjectChanged += CodeEditor_CurrentObjectChanged;
            CurrentObject.Changed += CurrentObject_Changed;
            _SyntaxHigLighter = new MySyntaxHighlightService(control, (Script)this.CurrentObject);
            control.ReplaceService<ISyntaxHighlightService>(_SyntaxHigLighter);
            control.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            control.Dock = DockStyle.Fill;
            //control.Minimum = 0;
            //control.Maximum = 5;
            control.TextChanged += control_ValueChanged;
            return control;
        }

        private void CurrentObject_Changed(object sender, DevExpress.Xpo.ObjectChangeEventArgs e)
        {
            _SyntaxHigLighter.Execute();
        }

        private void CodeEditor_CurrentObjectChanged(object sender, EventArgs e)
        {
            _SyntaxHigLighter.Execute();
        }

        protected override void OnControlCreated()
        {
            base.OnControlCreated();
            ReadValue();
        }

        public CodeEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (control != null)
            {
                control.TextChanged -= control_ValueChanged;
                control = null;
            }
            base.Dispose(disposing);
        }

        //RepositoryItem IInplaceEditSupport.CreateRepositoryItem()
        //{
        //    RepositoryItemSpinEdit item = new RepositoryItemSpinEdit();
        //    item.MinValue = 0;
        //    item.MaxValue = 5;
        //    item.Mask.EditMask = "0";
        //    return item;
        //}
        protected override object GetControlValueCore()
        {
            if (control != null)
            {
                return (string)control.Text;
            }
            return null;
        }
    }
}