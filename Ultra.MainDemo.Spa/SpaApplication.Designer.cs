namespace Ultra.MainDemo.Spa
{
    partial class MainDemoSpaApplication
    {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Spa.SystemModule.SystemSpaModule module2;
        private Ultra.MainDemo.Module.MainDemoModule module3;
        private Ultra.MainDemo.Module.Spa.MainDemoSpaModule module4;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule;

        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Spa.SystemModule.SystemSpaModule();
            this.module3 = new Ultra.MainDemo.Module.MainDemoModule();
            this.module4 = new Ultra.MainDemo.Module.Spa.MainDemoSpaModule();
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();

            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();


            // 
            // MainDemoSpaApplication
            // 
            this.ApplicationName = "Ultra.MainDemo";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.validationModule);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.MainDemoSpaApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

    }
}