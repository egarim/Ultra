using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Microsoft.CodeAnalysis;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;

using DevExpress.Persistent.BaseImpl;
using Ultra.Scripting.Core.BusinessObjects;

namespace Ultra.Scripting.Core.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class ScriptController : ViewController
    {
        private DevExpress.ExpressApp.Actions.SimpleAction saCompile;
        private DevExpress.ExpressApp.Actions.SimpleAction saDetectLocalReferences;
        private DevExpress.ExpressApp.Actions.SimpleAction saRunScript;
        private System.ComponentModel.IContainer components = null;

        public ScriptController()
        {
            this.TargetObjectType = typeof(Script);
            this.TargetViewType = ViewType.DetailView;

            this.components = new System.ComponentModel.Container();
            this.saCompile = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.saDetectLocalReferences = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.saRunScript = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //
            // saCompile
            //
            this.saCompile.Caption = "Compile";
            this.saCompile.ConfirmationMessage = null;
            this.saCompile.Id = "d71a4da4-e86e-4688-94f7-d0acf0591b1d";
            this.saCompile.ToolTip = null;
            this.saCompile.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.saCompile_Execute);
            this.saCompile.ImageName = "Properties";
            //
            // saDetectLocalReferences
            //
            this.saDetectLocalReferences.Caption = "Detect Local Assembly References";
            this.saDetectLocalReferences.ConfirmationMessage = null;
            this.saDetectLocalReferences.Id = "3f2bdf2c-a7c3-448c-b1aa-c44fc7041558";
            this.saDetectLocalReferences.ToolTip = null;
            this.saDetectLocalReferences.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.scDetectLocalAssemblyReferences_Execute);
            this.saDetectLocalReferences.ImageName = "ArrangeGroups";
            //
            // saRunScript
            //
            this.saRunScript.Caption = "Run Script";
            this.saRunScript.ConfirmationMessage = null;
            this.saRunScript.Id = "9f0e8713-0411-44d1-85b9-540203b5dcee";
            this.saRunScript.ToolTip = null;
            this.saRunScript.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.saRunScript_Execute);
            this.saRunScript.ImageName = "Action_Debug_Start";
            //
            // ScriptViewController
            //
            this.Actions.Add(this.saCompile);
            this.Actions.Add(this.saDetectLocalReferences);
            this.Actions.Add(this.saRunScript);

            SimpleAction TestAction = new SimpleAction(this, "TestAction", PredefinedCategory.View);
            TestAction.Caption = "Test Action";
            TestAction.Execute += TestAction_Execute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
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
        }

        private static void OpenDetailView(XafApplication app)
        {
            IObjectSpace os = app.CreateObjectSpace();
            //Find an existing object.
            //Contact obj = os.FindObject<Contact>(CriteriaOperator.Parse("FirstName=?", "My Contact"));
            //Or create a new object.
            Script obj = os.CreateObject<Script>();
            obj.Name = "My Contact";
            //Save the changes if necessary.
            //os.CommitChanges();
            //Configure how our View will be displayed (all parameters except for the CreatedView are optional).
            ShowViewParameters svp = new ShowViewParameters();
            DetailView dv = app.CreateDetailView(os, obj);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            svp.CreatedView = dv;
            //svp.TargetWindow = TargetWindow.NewModalWindow;
            //svp.Context = TemplateContext.PopupWindow;
            //svp.CreateAllControllers = true;
            //You can pass custom Controllers for intercommunication or to provide a standard functionality (e.g., functionality of a dialog window).
            //DialogController dc = Application.CreateController<DialogController>();
            //svp.Controllers.Add(dc);
            // Show our View once the ShowViewParameters object is initialized.
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        private static void CompilationResult(Script CurrentCode, EmitResult result)
        {
            IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                                    diagnostic.IsWarningAsError ||
                                    diagnostic.Severity == DiagnosticSeverity.Error);

            StringBuilder Builder = new StringBuilder();
            foreach (Diagnostic diagnostic in failures)
            {
                Builder.AppendLine(string.Format("{0}: {1}", diagnostic.Id, diagnostic.GetMessage()));
                //Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
            }
            CurrentCode.CompilationResult = Builder.ToString();
        }

        private void ExecuteAssemblyCode(Assembly assembly)
        {
            Type type = assembly.GetType("RoslynCompileSample.Writer");
            object obj = Activator.CreateInstance(type);
            type.InvokeMember("OpenDetailView",
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                obj,
                new object[] { this.Application });
        }

        private void saCompile_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var CurrentCode = (Script)e.CurrentObject;

            SyntaxTree syntaxTree;
            if (CurrentCode.Language == ScriptLanguage.CSharp)
            {
                syntaxTree = CSharpSyntaxTree.ParseText(CurrentCode.ScriptCode);
            }
            else
            {
                syntaxTree = Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxTree.ParseText(CurrentCode.ScriptCode);
            }

            string assemblyName = Path.GetRandomFileName();

            List<MetadataReference> Ref = new List<MetadataReference>();

            Script CurrentScript = (Script)e.CurrentObject;

            foreach (ScriptAssemblyReference item in CurrentScript.ScriptAssemblyReferences)
            {
                Ref.Add(MetadataReference.CreateFromFile(item.AssemblyPath));
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: Ref,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    CompilationResult(CurrentCode, result);
                    MessageOptions options = new MessageOptions();
                    options.Duration = 5000;
                    options.Message = string.Format("There are compilation errors, please check the compilation result");
                    options.Type = InformationType.Error;
                    options.Web.Position = InformationPosition.Bottom;
                    options.Win.Caption = "Error";
                    options.Win.Type = WinMessageType.Flyout;
                    Application.ShowViewStrategy.ShowMessage(options);
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    if (CurrentScript.Assembly == null)
                    {
                        CurrentScript.Assembly = this.View.ObjectSpace.CreateObject<FileData>();
                    }
                    ms.Seek(0, SeekOrigin.Begin);
                    CurrentScript.Assembly.LoadFromStream(assemblyName, ms);

                    if (this.View.ObjectSpace.IsModified)
                        this.View.ObjectSpace.CommitChanges();

                    MessageOptions options = new MessageOptions();
                    options.Duration = 5000;
                    options.Message = string.Format("Compilation is completed!");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    options.Win.Caption = "Success";
                    options.Win.Type = WinMessageType.Flyout;
                    Application.ShowViewStrategy.ShowMessage(options);

                    //ExecuteAssemblyCode(assembly);
                }
            }
        }

        private void scDetectLocalAssemblyReferences_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            AssemblyName[] CallingAssemblyReferences = Assembly.GetCallingAssembly().GetReferencedAssemblies();
            AssemblyName[] ThisAssemblyReferences = this.GetType().Assembly.GetReferencedAssemblies();

            List<AssemblyName> AllAssemblies = new List<AssemblyName>();
            AllAssemblies.AddRange(CallingAssemblyReferences);
            AllAssemblies.AddRange(Assembly.GetExecutingAssembly().GetReferencedAssemblies());
            AllAssemblies.Add(Assembly.GetCallingAssembly().GetName());
            AllAssemblies.Add(this.GetType().Assembly.GetName());

            AllAssemblies.AddRange(ThisAssemblyReferences);

            AllAssemblies.Add(typeof(object).Assembly.GetName());
            AllAssemblies.Add(typeof(Enumerable).Assembly.GetName());

            List<AssemblyName> FilteredAssemblies = new List<AssemblyName>();
            foreach (AssemblyName assembly in AllAssemblies)
            {
                var Exist = FilteredAssemblies.Where(f => f.FullName == assembly.FullName).FirstOrDefault();
                if (Exist == null)
                {
                    FilteredAssemblies.Add(assembly);
                }
            }

            foreach (AssemblyName filteredAssembly in FilteredAssemblies)
            {
                var Loc = Assembly.ReflectionOnlyLoad(filteredAssembly.FullName).Location;
                var AssemblyRefernce = this.View.ObjectSpace.CreateObject<ScriptAssemblyReference>();
                AssemblyRefernce.AssemblyName = filteredAssembly.Name;
                AssemblyRefernce.AssemblyFullName = filteredAssembly.FullName;
                AssemblyRefernce.AssemblyPath = Loc;
                AssemblyRefernce.Script = (Script)e.CurrentObject;
            }

            this.View.ObjectSpace.CommitChanges();
        }

        private void saRunScript_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var CurrentCode = (Script)e.CurrentObject;
            Assembly assembly = CurrentCode.GetAssembly();
            ExecuteAssemblyCode(assembly);
        }

        private void TestAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var CurrentCode = (Script)e.CurrentObject;

            #region SampleCode

            string SampleText = @"
                using System;

                namespace RoslynCompileSample
                {
                    public class Writer
                    {
                        public void Write(string message)
                        {
                            Console.WriteLine(message);
                        }
                    }
                }";

            #endregion SampleCode

            //SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(SampleText);

            SyntaxTree syntaxTree;
            if (CurrentCode.Language == ScriptLanguage.CSharp)
            {
                syntaxTree = CSharpSyntaxTree.ParseText(CurrentCode.ScriptCode);
            }
            else
            {
                syntaxTree = Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxTree.ParseText(CurrentCode.ScriptCode);
            }

            string assemblyName = Path.GetRandomFileName();

            List<MetadataReference> Ref = new List<MetadataReference>();

            Script CurrentScript = (Script)e.CurrentObject;

            foreach (ScriptAssemblyReference item in CurrentScript.ScriptAssemblyReferences)
            {
                Ref.Add(MetadataReference.CreateFromFile(item.AssemblyPath));
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: Ref,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    CompilationResult(CurrentCode, result);
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    if (CurrentScript.Assembly == null)
                    {
                        CurrentScript.Assembly = this.View.ObjectSpace.CreateObject<FileData>();
                    }
                    ms.Seek(0, SeekOrigin.Begin);
                    CurrentScript.Assembly.LoadFromStream(assemblyName, ms);

                    if (this.View.ObjectSpace.IsModified)
                        this.View.ObjectSpace.CommitChanges();

                    //ExecuteAssemblyCode(assembly);
                }
            }
        }
    }
}