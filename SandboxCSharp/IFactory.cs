using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp
{
    public interface IFactory
    {
        void DoRegister(IUnityContainer Container);
        void Run(IUnityContainer Container);
    }
}
