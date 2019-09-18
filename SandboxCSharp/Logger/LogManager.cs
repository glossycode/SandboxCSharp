using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SandboxCSharp.Logger
{
    sealed class LogManager
    {
        // Static members are 'eagerly initialized', that is, 
        // immediately when class is loaded for the first time.
        // .NET guarantees thread safety for static initialization
        private static readonly LogManager _instance = new LogManager();

        private LogManager()
        {
            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            GetLogger(typeof(Program)).Info("Start Application");
        }

        public static LogManager Instance()
        {
            return _instance;
        }

        public ILogger GetLogger(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return new LogWrapper(type);
        }
    }
}
