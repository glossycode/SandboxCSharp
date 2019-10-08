using SandboxCSharp.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.Sandbox
{
    enum ObjectEnum { NullObject, TrueObject }
    class NullReturns : IFactory
    {
        private static readonly ILogger _logger = LogManager.Instance().GetLogger(typeof(NullReturns));
        public void DoRegister(IUnityContainer Container)
        {
        }

        public void Run(IUnityContainer Container)
        {
            var obj = GetAnObject(ObjectEnum.NullObject);
            LogObject(obj);

            obj = GetAnObject(ObjectEnum.TrueObject, false);
            LogObject(obj);

            obj = GetAnObject(ObjectEnum.TrueObject, true);
            LogObject(obj);

            obj = GetAnObject(ObjectEnum.NullObject) ?? new NullReturns() { Name = "The left operator object is null" };
            LogObject(obj);

            obj = GetAnObject(ObjectEnum.TrueObject) ?? new NullReturns() { Name = "The left operator object is NOT null" };
            LogObject(obj);
        }

        private static void LogObject(NullReturns obj)
        {
            _logger.Debug($"This is an null object, so name is:>{obj?.Name}< and its first child:>{obj?.Children?[0].Name}<");
        }

        NullReturns GetAnObject(ObjectEnum type, bool hasChildren = false)
        {
            if (type == ObjectEnum.NullObject)
                return null;
            if (!hasChildren)
                return new NullReturns() { Name = "Hello with no children" };
            else return new NullReturns() { Name = "Hello from Family", Children = new NullReturns[] { new NullReturns() { Name = "Child1" } } };

        }

        public String Name { get; set; }
        public IList<NullReturns> Children { get; set; }
    }
}
