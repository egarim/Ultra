using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using Ultra.UniversalSearch.Updaters;
using Template.Module.Controllers;
using System.Drawing;

namespace Ultra.UniversalSearch
{
    //TODO uncomment the filter attribute depending on the platform implementation
    //[ToolboxItemFilter("Xaf.Platform.Win")]
    //[ToolboxItemFilter("Xaf.Platform.Web")]
    [DevExpress.Utils.ToolboxTabName(XafAssemblyInfo.DXTabXafModules)]
    //TODO module description
    [Description("Ultra Modules for XAF: Universal Search")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(UniversalSearchModule), "Resources.User_Search02_WF.ico")]
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class UniversalSearchModule : ModuleBase
    {
        public UniversalSearchModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }

        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }

        public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters)
        {
            base.AddGeneratorUpdaters(updaters);
            updaters.Add(new ModelLocalizationGroupGeneratorUpdater());
            updaters.Add(new ModelLocalizationNodesGeneratorUpdater());
        }

        protected override IEnumerable<Type> GetDeclaredExportedTypes()
        {
            return new Type[] {
                typeof(UniversalSearchResult),
            };
        }

        protected override IEnumerable<Type> GetDeclaredControllerTypes()
        {
            return new Type[] {
                typeof(UniversalSearchController)
            };
        }
    }
}