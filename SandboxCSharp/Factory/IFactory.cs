using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.Factory
{
    public interface IFactory
    {
        void DoRegister(IUnityContainer Container);
        void Run(IUnityContainer Container);
    }
}
