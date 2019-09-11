using SandboxCSharp.Overriden;
using SandboxCSharp.Unity;
using System;
using Unity;

namespace SandboxCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var obj1 = (IClass)UnityBuilder.Instance().Container.Resolve<IClass>("BasicClass");
            Console.WriteLine("" + obj1.IsOption1Implemented);
            Console.WriteLine("" +((IClass)UnityBuilder.Instance().Container.Resolve<IClass>("SubClass1")).IsOption1Implemented);

            Console.ReadKey();


        }
    }
}
