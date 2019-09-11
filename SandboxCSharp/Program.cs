using SandboxCSharp.Factory;
using SandboxCSharp.Overriden;
using SandboxCSharp.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity;

namespace SandboxCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            UnityBuilder.Instance().Factories.ForEach(x => x.Run(UnityBuilder.Instance().Container));
            Console.Write("\r\nHit a key to terminate ... ");
            Console.ReadKey();
        }
    }
}
