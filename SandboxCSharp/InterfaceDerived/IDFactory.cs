using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.InterfaceDerived
{
    class IDFactory : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {
            
        }

        public void Run(IUnityContainer Container)
        {
            NewMethod("Class1", new Class1(), 22);

            NewMethod("Class2", new Class2(), 44);

            NewMethod("Class3", new Class3(), 66);            
        }

        private static void NewMethod(string name, TheInterface obj, int parameter)
        {
            Console.WriteLine($"==========================================================");
            Console.WriteLine($"Calling {name}.DoIt");
            string className = obj.DoIt(parameter);

             if (className==name)
                Console.WriteLine($"OK ------------- this is the same class name { className}");
            else
                Console.WriteLine($"FAILED !!!!!!!!! {name} != { className}");


        }
    }
}
