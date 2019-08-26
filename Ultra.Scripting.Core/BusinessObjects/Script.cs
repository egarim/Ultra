﻿using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Reflection;
using System.IO;
using DevExpress.Xpo.Metadata;

namespace Ultra.Scripting.Core.BusinessObjects
{
    [NavigationItem("Scripting")]
    [DefaultClassOptions]
    [ImageName("Action_ShowScript")]
    public class ScriptTemplate : Script
    {
        public ScriptTemplate(Session session) : base(session)
        {
        }
    }

    [NavigationItem("Scripting")]
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Script : BaseObject
    {
        public Assembly GetAssembly()
        {
            MemoryStream ms = new MemoryStream();
            Assembly.SaveToStream(ms);
            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = System.Reflection.Assembly.Load(ms.ToArray());
            return assembly;
        }

        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Script(Session session)
            : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Language = ScriptLanguage.CSharp;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        // Fields...

        DateTime lastCompiled;
        private ScriptLanguage _Language;
        private string _CompilationResult;
        private string _Description;
        private string _Name;
        private FileData _Assembly;
        private string _ScriptCode;

        [ModelDefault("PropertyEditorType", "Ultra.Scripting.Core.Win.CodeEditor")]
        [Size(SizeAttribute.Unlimited)]
        public string ScriptCode
        {
            get
            {
                return _ScriptCode;
            }
            set
            {
                SetPropertyValue("ScriptCode", ref _ScriptCode, value);
            }
        }

        public ScriptLanguage Language
        {
            get { return _Language; }
            set
            {
                _Language = value;
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetPropertyValue("Name", ref _Name, value);
            }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetPropertyValue("Description", ref _Description, value);
            }
        }

        public FileData Assembly
        {
            get
            {
                return _Assembly;
            }
            set
            {
                SetPropertyValue("Assembly", ref _Assembly, value);
                LastCompiled = DateTime.Now;
            }
        }
        [ModelDefault("AllowEdit","false")]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime LastCompiled
        {
            get => lastCompiled;
            set => SetPropertyValue(nameof(LastCompiled), ref lastCompiled, value);
        }
        [Association("Script-ScriptAssemblyReferences")]
        public XPCollection<ScriptAssemblyReference> ScriptAssemblyReferences
        {
            get
            {
                return GetCollection<ScriptAssemblyReference>("ScriptAssemblyReferences");
            }
        }

        [NonPersistent()]
        [Size(SizeAttribute.Unlimited)]
        public string CompilationResult
        {
            get
            {
                return _CompilationResult;
            }
            set
            {
                SetPropertyValue("CompilationResult", ref _CompilationResult, value);
            }
        }
    }
}