using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCSharp.InterfaceDerived
{
    class Class1 : TheInterface
    {
        public virtual string DoIt(int parameter)
        {
            Console.WriteLine("This is Class1:" + parameter);

            Console.WriteLine("GetType().Name:" + GetType().Name);
            return GetType().Name;
        }
    }
}
