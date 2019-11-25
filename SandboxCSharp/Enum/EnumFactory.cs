using SandboxCSharp.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace SandboxCSharp.Enum2
{
    public enum Pomme 
    { 
        Golden, Braeburn, Topaz
    }
    class EnumFactory : IFactory
    {
        private readonly ILogger _logger = LogManager.Instance().GetLogger(typeof(EnumFactory));

        public void DoRegister(IUnityContainer Container)
        {

        }

        public void Run(IUnityContainer Container)
        {
            Object p;
            Enum.TryParse(typeof(Pomme), "Topaz", true, out p);
            Pomme myPomme = (Pomme)p;

            if (Enum.IsDefined(typeof(Pomme), "Topaz"))
            {
                _logger.Info("Topaz is in the enum");
            }

            if (!Enum.IsDefined(typeof(Pomme), "PommeRainette"))
            {
                _logger.Info("PommeRainette is NOT in the enum");
            }
        }
    }
}
