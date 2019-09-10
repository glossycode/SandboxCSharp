using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCSharp.Overriden
{
  public  class BasicClass : IClass
    {
        public virtual bool IsOption1Implemented { get; } = false;
    }
}
