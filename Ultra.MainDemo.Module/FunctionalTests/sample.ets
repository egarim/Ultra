;#DropDB Ultra.MainDemoEasyTest

#Application Ultra.MainDemo.Win
#Application Ultra.MainDemo.Web

*FillForm
 User Name = Admin
*Action Log In

#IfDef Ultra.MainDemo.Win

*Action About...
 
*Action OK

#EndIf

#IfDef Ultra.MainDemo.Web
*Action Navigation(My Details)
#EndIf
