#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.Nullable
{
    class NullableFactorry : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {
            
        }

        public void Run(IUnityContainer Container)
        {
            string[] args = { };
            string s = (args.Length > 0) ? args[0] : null;
            M(s);
        }

         void M(string s)
        {
            Console.WriteLine(s.Length);
        }
    }
}
