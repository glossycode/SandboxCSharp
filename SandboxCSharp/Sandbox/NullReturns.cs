using SandboxCSharp.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.Sandbox
{
    class NullReturns : IFactory
    {
        private static readonly ILogger _logger = LogManager.Instance().GetLogger(typeof(NullReturns));
        public void DoRegister(IUnityContainer Container)
        {
        }

        public void Run(IUnityContainer Container)
        {
            var obj = GetAnObject(true);
            LogObject(obj);

            obj = GetAnObject(false, false);
            LogObject(obj);

            obj = GetAnObject(false, true);
            LogObject(obj);

            obj = GetAnObject(true) ?? new NullReturns() { Name = "The left operator object is null"};
            LogObject(obj);

            obj = GetAnObject(false) ?? new NullReturns() { Name = "The left operator object is NOT null" };
            LogObject(obj);
        }

        private static void LogObject(NullReturns obj)
        {
            _logger.Debug($"This is an null object, so name is:>{obj?.Name}< and its first child:>{obj?.Children?[0].Name}<");
        }

        NullReturns GetAnObject(bool isNull, bool hasChildren = false)
        {
            if (isNull)
                return null;
            if (!hasChildren)
                return new NullReturns() { Name = "Hello" };
            else return new NullReturns() { Name = "Hello", Children = new NullReturns[] { new NullReturns() { Name = "Child1" } } };

        }

        public String Name { get; set; }
        public IList<NullReturns> Children { get; set; }
    }
}
