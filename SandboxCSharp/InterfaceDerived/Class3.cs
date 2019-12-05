using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCSharp.InterfaceDerived
{
    class Class3 : Class1, TheInterface
    {
        public string DoIt(int parameter)
        {
            Console.WriteLine("This is Class3:" + parameter);

            Console.WriteLine("GetType().Name:" + GetType().Name);
            return GetType().Name;
        }
    }
    
}
