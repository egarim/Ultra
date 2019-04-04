namespace Ultra.MainDemo.Module
{
    partial class MainDemoModule
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // UltraModuleTemplateModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(Ultra.UniversalSearch.UniversalSearchModule));
            this.RequiredModuleTypes.Add(typeof(Ultra.Scripting.Core.ScriptingCoreModule));
            this.RequiredModuleTypes.Add(typeof(Ultra.Email.EmailModule));
            this.AdditionalExportedTypes.Add(typeof(Ultra.Scripting.Core.BusinessObjects.Script));

        }

        #endregion
    }
}
