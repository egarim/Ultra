using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;

using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Ultra.MainDemo.Module.BusinessObjects;
using DevExpress.Persistent.BaseImpl;
using Country = Ultra.MainDemo.Module.BusinessObjects.Country;

namespace Ultra.MainDemo.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }

        private Country CreateCountry(string Name, string Code)
        {
            Country Country = ObjectSpace.FindObject<Country>(CriteriaOperator.Parse("Name=?", Name));
            if (Country == null)
            {
                Country = ObjectSpace.CreateObject<Country>();
                Country.Name = Name;
                Country.Code = Code;
            }
            return Country;
        }

        private Customer CreateCustomer(string Name, string Code, Country Country)
        {
            Customer Customer = ObjectSpace.FindObject<Customer>(CriteriaOperator.Parse("Name=?", Name));
            if (Customer == null)
            {
                Customer = ObjectSpace.CreateObject<Customer>();
                Customer.Name = Name;
                Customer.Code = Code;
                Customer.Country = Country;
            }
            return Customer;
        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            var ElSalvador = CreateCountry("El Salvador", "503");
            var Russia = CreateCountry("Russia", "7");
            var Estonia = CreateCountry("Estonia", "372");
            var Chile = CreateCountry("Chile", "56");
            var DominicanRepublic = CreateCountry("Dominican Republic", "56");
            var Cuba = CreateCountry("Cuba", "53");

            CreateCustomer("Jaime Ricardo Macias", "000", ElSalvador);
            CreateCustomer("Douglas Coto", "001", ElSalvador);
            CreateCustomer("Walter Omar Gavarrete", "002", ElSalvador);

            CreateCustomer("Alexander Pushkin", "003", Russia);
            CreateCustomer("Peter Alexeyevich", "004", Russia);
            CreateCustomer("Anna Karenina", "005", Russia);

            CreateCustomer("Sander Tallinn", "006", Estonia);
            CreateCustomer("Voldermar", "007", Estonia);
            CreateCustomer("Peter", "008", Estonia);

            CreateCustomer("Luis Alarcon Ponce", "009", Chile);
            CreateCustomer("Cristobal Perez", "010", Chile);
            CreateCustomer("Jaime Pascal", "011", Chile);

            CreateCustomer("Pedro Hernandez", "012", DominicanRepublic);
            CreateCustomer("Yordi", "013", DominicanRepublic);

            CreateCustomer("Jose Javier Columbie", "014", Cuba);
            CreateCustomer("Yasmani", "015", Cuba);
            CreateCustomer("Yosbel", "016", Cuba);

            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
            PermissionPolicyUser sampleUser = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "User"));
            if (sampleUser == null)
            {
                sampleUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                sampleUser.UserName = "User";
                sampleUser.SetPassword("");
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            PermissionPolicyUser userAdmin = ObjectSpace.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", "Admin"));
            if (userAdmin == null)
            {
                userAdmin = ObjectSpace.CreateObject<PermissionPolicyUser>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
            }
            // If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
            userAdmin.Roles.Add(adminRole);
            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }

        private PermissionPolicyRole CreateDefaultRole()
        {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }
    }
}