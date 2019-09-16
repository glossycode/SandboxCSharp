using System;
using Unity;

namespace SandboxCSharp.Overriden
{
    class OverrideFactory : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {
            Container.RegisterType<IClass, BasicClass>("BasicClass");
            Container.RegisterType<IClass, SubClass1>("SubClass1");
        }

        public void Run(IUnityContainer Container)
        {
            var obj1 = (IClass)Container.Resolve<IClass>("BasicClass");
            Console.WriteLine("The basic type does not have the option implemented :\r\n=>\t" + obj1.IsOption1Implemented);
            Console.WriteLine("The sub class that does have the option implemented :\r\n=>\t" + ((IClass)Container.Resolve<IClass>("SubClass1")).IsOption1Implemented);
        }
    }
}
