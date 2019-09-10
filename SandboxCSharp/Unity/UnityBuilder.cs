using SandboxCSharp.Overriden;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.Unity
{
    sealed class UnityBuilder
    {
        private static readonly UnityBuilder _instance =
          new UnityBuilder();


        //private IUnityContainer _container = new UnityContainer();
        // Type-safe generic list of servers
        public IUnityContainer Container { get; } = new UnityContainer();//  { return _container; } }

        // Note: constructor is 'private'

        private UnityBuilder()
        {
            //_container = new UnityContainer();
            Container.RegisterType<IClass, BasicClass>("BasicClass");
            Container.RegisterType<IClass, SubClass1>("SubClass1");


            IClass battery = Container.Resolve<IClass>("SubClass1");
        }

        public static UnityBuilder Instance()
        {
            return _instance;
        }
    }

}
