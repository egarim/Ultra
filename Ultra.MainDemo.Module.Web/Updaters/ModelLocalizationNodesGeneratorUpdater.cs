using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Model.Core;

namespace Ultra.MainDemo.Module.Web.Updaters
{
    public class ModelLocalizationNodesGeneratorUpdater :
    ModelNodesGeneratorUpdater<ModelLocalizationNodesGenerator>
    {
        //TODO Change ModuleName Node
        public const string ModuleName = "ModuleNode";

        public override void UpdateNode(ModelNode node)
        {
            // Cast the 'node' parameter to IModelLocalization
            // to access the Localization node.

            IModelLocalization Localization = (IModelLocalization)node;
            IModelLocalizationGroup ThisModuleNode = (IModelLocalizationGroup)Localization.AddNode<IModelLocalizationGroup>();
            ThisModuleNode.Name = ModuleName;
            ThisModuleNode.Value = ModuleName;
        }
    }
}