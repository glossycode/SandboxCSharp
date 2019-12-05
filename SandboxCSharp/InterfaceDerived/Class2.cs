using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCSharp.InterfaceDerived
{
    class Class2 : Class1 
    {
        public override string DoIt(int parameter)
        {
            Console.WriteLine("This is Class2:" + parameter);

            Console.WriteLine("GetType().Name:" + GetType().Name);

            return GetType().Name;
        }
    }
}
