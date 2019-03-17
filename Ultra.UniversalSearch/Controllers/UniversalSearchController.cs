using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Ultra.UniversalSearch;
using Ultra.UniversalSearch.BusinessObjects;

namespace Template.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class UniversalSearchController : ViewController<ListView>
    {
        public UniversalSearchController()
        {
            detailView = new SimpleAction(this, "ShowDetailView", "Hidden");
            detailView.Caption = "ShowDetailView";
            detailView.Execute += ShowDetailView_Execute;
            // Target required Views (via the TargetXXX properties) and create their Actions.
            parametrizedAction = new ParametrizedAction(this, "Universal Search", DevExpress.Persistent.Base.PredefinedCategory.View, typeof(string));
            parametrizedAction.Execute += Search_Execute;
        }

        private void ShowDetailView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            UniversalSearchResult CurrentSearchResult = (UniversalSearchResult)e.CurrentObject;
            IObjectSpace objectSpace = Application.CreateObjectSpace(CurrentSearchResult.ObjectType);

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, objectSpace.GetObjectByKey(CurrentSearchResult.ObjectType, Guid.Parse(CurrentSearchResult.ObjectKey)), true);
        }

        private List<Type> SearchableTypes = new List<Type>();

        protected override void OnActivated()
        {
            base.OnActivated();
            processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (processCurrentObjectController != null)
            {
                processCurrentObjectController.CustomProcessSelectedItem +=
                    processCurrentObjectController_CustomProcessSelectedItem;
            }

            SearchableTypes = new List<Type>();

            IEnumerable<ITypeInfo> SearchTypes = this.Application.TypesInfo.PersistentTypes.Where(ti => ti.FindAttribute<UniversalSearchAttributeAttribute>() != null);

            foreach (ITypeInfo typeInfo in SearchTypes)
            {
                SearchableTypes.Add(typeInfo.Type);
            }

            NonPersistentObjectSpace objectSpace = ((NonPersistentObjectSpace)this.View.CollectionSource.ObjectSpace);
            objectSpace.ObjectsGetting += ObjectSpace_ObjectsGetting;
        }

        private void processCurrentObjectController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            e.Handled = true;
            detailView.DoExecute();
        }

        private BindingList<UniversalSearchResult> objects = new BindingList<UniversalSearchResult>();
        private ListViewProcessCurrentObjectController processCurrentObjectController;
        private SimpleAction detailView;
        private ParametrizedAction parametrizedAction;

        private void ObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e)
        {
            e.Objects = objects;
        }

        private CriteriaOperator GetCaseInsensitiveCriteria(object searchValue, Type TargetType)
        {
            var result = this.Application.Model.BOModel.GetClass(TargetType);

            //we get a list of all the properties that can be involved in the filter
            //ICollection<string> SearchProperties = standardFilterController.GetFullTextSearchProperties();

            Collection<string> SearchProperties = new Collection<string>();
            foreach (IModelMember modelMember in result.OwnMembers)
            {
                SearchProperties.Add(modelMember.Name);
            }

            //we declare a model class and a property name,the values on this variables will change if we property involve is a navigation property (another persistent object)
            IModelClass ModelClass = null;
            string PropertyName = string.Empty;

            //we declare a list of operators to contains new operators we are going to create
            List<CriteriaOperator> Operator = new List<CriteriaOperator>();
            //we iterate all the properties
            foreach (var CurrentProperty in SearchProperties)
            {
                //here we split the name with a dot, if length is greater than 1 it means its a navigation property, beware that this may fail with a deep tree of properties like category.subcategory.name
                var Split = CurrentProperty.Split('.');
                if (Split.Length > 1)
                {
                    Debug.WriteLine(string.Format("{0}", "its a complex property"));
                    var CurrentClass = result;
                    for (int i = 0; i < Split.Length; i++)
                    {
                        //if its a navigation property we locate the type in the BOModel
                        //IModelMember member = CurrentClass.OwnMembers.Where(m => m.Name == Split[i]).FirstOrDefault();
                        //var member = CurrentClass.AllMembers.Where(m => m.Name == Split[i]).FirstOrDefault();
                        IModelMember member = CurrentClass.AllMembers.Where(m => m.Name == Split[i]).FirstOrDefault();

                        //then we set the model class and property name to the values of the navigation property like category.name where category is the model class and name is the property
                        CurrentClass = this.Application.Model.BOModel.GetClass(member.Type);
                        if (CurrentClass == null)
                            continue;

                        ModelClass = CurrentClass;
                        PropertyName = Split[i + 1];
                    }
                    Debug.WriteLine(string.Format("{0}:{1}", "ModelClass", ModelClass.Name));
                    Debug.WriteLine(string.Format("{0}:{1}", "PropertyName", PropertyName));
                }
                else
                {
                    //else the model class will be the current class where the filter is executing, and the property will be the current property we are evaluating
                    ModelClass = result;
                    PropertyName = CurrentProperty;
                }

                //we look for the property on the class model own member
                var Property = ModelClass.OwnMembers.Where(m => m.Name == PropertyName).FirstOrDefault();
                if (Property != null)
                {
                    //if the property is a string it means that we can set it to upper case
                    if (Property.Type == typeof(string))
                    {
                        searchValue = searchValue.ToString().ToUpper();
                        //we create an operator where we set the value of the property to upper before we compare it, also we change the comparison value to upper
                        CriteriaOperator Operand = CriteriaOperator.Parse("Contains(Upper(" + CurrentProperty + "), ?)", searchValue);
                        //we added to the list of operators that will concatenate with OR
                        Operator.Add(Operand);
                    }
                    else
                    {
                        //if the property is not a string we need to try to cast the value to the correct type so we do a catch try, if we manage to cast the value it will be added to the operators list
                        try
                        {
                            var ConvertedType = Convert.ChangeType(searchValue, Property.Type);
                            CriteriaOperator operand = new BinaryOperator(CurrentProperty, ConvertedType, BinaryOperatorType.Equal);
                            Operator.Add(operand);
                        }
                        catch (Exception)
                        {
                            //silent exception, this will happen if the casting was not successfully so we won't add the operand on this case
                        }
                    }
                }
            }

            //we concatenate everything with an OR
            var alloperators = CriteriaOperator.Or(Operator.ToArray());
            Debug.WriteLine(string.Format("{0}:{1}", "all operators", alloperators));
            return alloperators;
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
            if (processCurrentObjectController != null)
            {
                processCurrentObjectController.CustomProcessSelectedItem -=
                    processCurrentObjectController_CustomProcessSelectedItem;
            }
        }

        private void Search_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            var os = this.Application.CreateObjectSpace();
            objects.Clear();
            foreach (Type type in SearchableTypes)
            {
                UniversalSearchAttributeAttribute attrs = (UniversalSearchAttributeAttribute)type.GetCustomAttributes(true).Cast<Attribute>().Where(att => att.GetType().IsAssignableFrom(typeof(UniversalSearchAttributeAttribute))).FirstOrDefault();

                CriteriaOperator criteria = GetCaseInsensitiveCriteria(e.ParameterCurrentValue, type);
                string[] DisplayProperties = attrs.DisplayProperties.Split(';');
                string QueryColumns = this.Application.Model.BOModel.GetClass(type).KeyProperty + ";" + attrs.DisplayProperties;
                var Dv = os.CreateDataView(type, QueryColumns, criteria, null);
                string FullDisplay = string.Empty;
                for (int i = 0; i < Dv.Count; i++)
                {
                    var SearchResult = this.View.ObjectSpace.CreateObject<UniversalSearchResult>();
                    SearchResult.ObjectDisplayName = type.Name;
                    XafDataViewRecord xafDataViewRecord = (XafDataViewRecord)Dv[i];
                    SearchResult.ObjectType = type;
                    SearchResult.ObjectKey = xafDataViewRecord[0].ToString();
                    foreach (var displayProperty in DisplayProperties)
                    {
                        FullDisplay = FullDisplay + " " + xafDataViewRecord[displayProperty]?.ToString();
                    }
                    SearchResult.Display = FullDisplay;
                    objects.Add(SearchResult);
                }
            }

            this.View.ObjectSpace.Refresh();
        }
    }
}