using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.Generic
{
    class GenericFatory : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {           
        }

        public void Run(IUnityContainer Container)
        {            
            AGeneric.ExecuteWithType();
            AGeneric.ExecuteWithNoType();
        }
    }
}
