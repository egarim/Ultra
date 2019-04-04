using NUnit.Framework;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using Ultra.UniversalSearch.Controllers;
using Ultra.MainDemo.Module;
using Ultra.UniversalSearch;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Win.SystemModule;
using System.Linq;
using TestAllModules.Applications;

namespace TestAllModules.Modules
{
    public class TestBase
    {
        public TestApplicationWin WinApp;
        public TestApplicationAsp AspApp;

        public void BaseSetup()
        {
            SetupWinApp();
            SetupWebApp();
        }

        public virtual void SetupWebApp()
        {
            AspApp = new TestApplicationAsp();

            MainDemoModule MainDemoModule = new MainDemoModule();
            MainDemoModule.AdditionalExportedTypes.Add((typeof(PermissionPolicyUser)));
            MainDemoModule.Setup(WinApp);
            AspApp.Modules.Add(MainDemoModule);
            AspApp.Modules.Add(new SystemAspNetModule());

            AspApp.Setup();
        }

        public virtual void SetupWinApp()
        {
            WinApp = new TestApplicationWin();

            MainDemoModule MainDemoModule = new MainDemoModule();
            MainDemoModule.AdditionalExportedTypes.Add((typeof(PermissionPolicyUser)));
            MainDemoModule.Setup(WinApp);
            WinApp.Modules.Add(MainDemoModule);
            WinApp.Modules.Add(new SystemWindowsFormsModule());

            WinApp.Setup();
        }
    }
}