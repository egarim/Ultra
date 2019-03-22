using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.Scripting.Core.Interfaces
{
    public interface IViewControllerScript
    {
        void Execute(object sender, View view, XafApplication application);
    }

    public class FunctionResult<T>
    {
        public FunctionResult()
        {
        }

        public T Result { get; set; }
    }

    public interface IExecuteProcess
    {
        void Execute();
    }

    public interface IExecuteFunction
    {
        FunctionResult<TResult> Execute<T, TResult>(T Param1);
    }

    public interface IExecuteFunction2
    {
        FunctionResult<TResult> Execute<T, T1, TResult>(T Param1, T Param2);
    }
}