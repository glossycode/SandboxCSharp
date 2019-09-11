using SandboxCSharp.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace SandboxCSharp.Unity
{
    sealed class UnityBuilder
    {
        private static readonly UnityBuilder _instance = new UnityBuilder();

        // Type-safe generic list of servers
        public IUnityContainer Container { get; } = new UnityContainer();
        
        public List<IFactory> Factories { get; private set; } = new List<IFactory>();

        // Note: constructor is 'private'
        private UnityBuilder()
        {
            Factories = GetAllFactories();
            Factories.ForEach(x => DoRegisterFactory(x));
        }

        public static UnityBuilder Instance()
        {
            return _instance;
        }

        public void DoRegisterFactory(IFactory factory)
        {
            if (factory!=null)
            {
                factory.DoRegister(Container);
            }            
        }
        
        public  List<IFactory> GetAllFactories()
        {
            List<String> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IFactory).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(x => x.FullName).ToList();

            var list = new List<IFactory>();
            foreach (var t in types)
            {
                list.Add((IFactory)Activator.CreateInstance(Type.GetType(t)));
            }
            return list;
        }
    }
}
