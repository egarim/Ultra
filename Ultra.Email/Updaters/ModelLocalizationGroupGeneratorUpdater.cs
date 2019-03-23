using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Model.Core;

namespace Ultra.Email.Updaters
{
    public class ModelLocalizationGroupGeneratorUpdater :
    ModelNodesGeneratorUpdater<ModelLocalizationGroupGenerator>
    {
        public const string SuccessCaption = "Success Caption";
        public const string ErrorCaption = "Error Caption";
        public const string SuccessMessage = "Success Message";
        public const string ErrorMessage = "Error Message";

        public static string GetMainLocalizationNode()
        {
            return ModelLocalizationNodesGeneratorUpdater.ModuleName;
        }

        public override void UpdateNode(ModelNode node)
        {
            // Cast the 'node' parameter to IModelLocalizationGroup
            // to access the LocalizationGroup node.

            IModelLocalizationGroup Group = (IModelLocalizationGroup)node;

            if (!(Group.Name == ModelLocalizationNodesGeneratorUpdater.ModuleName))
                return;

            CreateNodes(Group);
        }

        #region Helpers

        private static void AddGroupItem(IModelLocalizationGroup Group, string Name, string Value)
        {
            var Item = Group.AddNode<IModelLocalizationItem>(Name);
            Item.Name = Name;
            Item.Value = Value;
        }

        #endregion Helpers

        private static void CreateNodes(IModelLocalizationGroup Group)
        {
            //TODO create nodes for your text
            AddGroupItem(Group, SuccessCaption, "Success");
            AddGroupItem(Group, ErrorCaption, "Error");
            AddGroupItem(Group, SuccessMessage, "Mail successfully sent");
            AddGroupItem(Group, ErrorMessage, "An error has occurred sending the mail");
        }
    }
}