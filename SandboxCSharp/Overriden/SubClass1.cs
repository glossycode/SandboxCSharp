using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCSharp.Overriden
{
    public class SubClass1 : BasicClass
    {
        public override bool IsOption1Implemented => true;
    }
}
