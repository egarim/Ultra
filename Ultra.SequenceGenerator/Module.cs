﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Ultra.SequenceGenerator.Updaters;

namespace Ultra.SequenceGenerator
{
    //TODO uncomment the filter attribute depending on the platform implementation
    //[ToolboxItemFilter("Xaf.Platform.Win")]
    //[ToolboxItemFilter("Xaf.Platform.Web")]
    [DevExpress.Utils.ToolboxTabName(XafAssemblyInfo.DXTabXafModules)]
    //TODO module description
    [Description("Ultra Modules for XAF:Sequence Generator ")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(UltraSequenceGenerator), "Resources.Gear.ico")]
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class UltraSequenceGenerator : ModuleBase
    {
        public UltraSequenceGenerator()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new Ultra.SequenceGenerator.DatabaseUpdate.Updater(objectSpace, versionFromDB);
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
            //TODO return an array of types to improve performance
            return new Type[] {
                typeof(Sequencer),typeof(XpoServerId)

            };
           
        }

        protected override IEnumerable<Type> GetDeclaredControllerTypes()
        {
            return base.GetDeclaredControllerTypes();
        }
    }
}