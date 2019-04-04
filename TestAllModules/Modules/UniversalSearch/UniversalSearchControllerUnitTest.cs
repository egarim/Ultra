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

namespace TestAllModules.Modules.UniversalSearch
{
    [TestFixture]
    public class UniversalSearchControllerUnitTest
    {
        private const string XValue = "x";
        private const string StringEmpty = "";
        private const object NullValue = null;
        private TestApplicationWin WinApp;
        private TestApplicationAsp AspApp;
        private UniversalSearchController WinController;
        private UniversalSearchController AspController;

        [SetUp]
        public void SetUp()
        {
            SetupWinApp();
            SetupWebApp();
            WinController = SetupWinController();
            AspController = SetupAspController();
        }

        private void SetupWebApp()
        {
            AspApp = new TestApplicationAsp();

            MainDemoModule MainDemoModule = new MainDemoModule();
            MainDemoModule.AdditionalExportedTypes.Add((typeof(PermissionPolicyUser)));
            MainDemoModule.Setup(WinApp);
            AspApp.Modules.Add(MainDemoModule);
            AspApp.Modules.Add(new SystemAspNetModule());

            AspApp.Setup();
        }

        private UniversalSearchController SetupAspController()
        {
            var controller = new UniversalSearchController();
            ListView UniversalSearchResultListView = AspApp.CreateListView(typeof(UniversalSearchResult), true);
            Frame frame = AspApp.CreateFrame(TemplateContext.ApplicationWindow);
            frame.SetView(UniversalSearchResultListView);
            frame.RegisterController(controller);
            return controller;
        }

        private UniversalSearchController SetupWinController()
        {
            var controller = new UniversalSearchController();
            ListView UniversalSearchResultListView = WinApp.CreateListView(typeof(UniversalSearchResult), true);
            Frame frame = WinApp.CreateFrame(TemplateContext.ApplicationWindow);
            frame.SetView(UniversalSearchResultListView);
            frame.RegisterController(controller);
            return controller;
        }

        private void SetupWinApp()
        {
            WinApp = new TestApplicationWin();

            MainDemoModule MainDemoModule = new MainDemoModule();
            MainDemoModule.AdditionalExportedTypes.Add((typeof(PermissionPolicyUser)));
            MainDemoModule.Setup(WinApp);
            WinApp.Modules.Add(MainDemoModule);
            WinApp.Modules.Add(new SystemWindowsFormsModule());

            WinApp.Setup();
        }

        #region X value test

        [Test]
        public void When_SearchActionDoExecuteWithX_Expect_ResultCountIs2_Win()
        {
            WinController.SearchAction.DoExecute(XValue);

            var Count = WinController.View.CollectionSource.GetCount();

            Assert.AreEqual(2, Count);
        }

        [Test]
        public void When_SearchActionDoExecuteWithX_Expect_ResultCountIs2_Asp()
        {
            AspController.SearchAction.DoExecute(XValue);

            var Count = AspController.View.CollectionSource.GetCount();

            Assert.AreEqual(2, Count);
        }

        #endregion X value test

        #region Null value test

        [Test]
        public void When_SearchActionDoExecuteWithNull_Expect_ResultCountIsZero_Win()
        {
            WinController.SearchAction.DoExecute(NullValue);

            var Count = WinController.View.CollectionSource.GetCount();

            Assert.AreEqual(0, Count);
        }

        [Test]
        public void When_SearchActionDoExecuteWithNull_Expect_ResultCountIsZero_Asp()
        {
            AspController.SearchAction.DoExecute(NullValue);

            var Count = AspController.View.CollectionSource.GetCount();

            Assert.AreEqual(0, Count);
        }

        #endregion Null value test

        #region Empty value test

        [Test]
        public void When_SearchActionDoExecuteWithStringEmpty_Expect_ResultCountIs23_Win()
        {
            WinController.SearchAction.DoExecute(StringEmpty);

            var Count = WinController.View.CollectionSource.GetCount();

            Assert.AreEqual(23, Count);
        }

        [Test]
        public void When_SearchActionDoExecuteWithStringEmpty_Expect_ResultCountIs23_Asp()
        {
            AspController.SearchAction.DoExecute(StringEmpty);

            var Count = AspController.View.CollectionSource.GetCount();

            Assert.AreEqual(23, Count);
        }

        #endregion Empty value test
    }
}