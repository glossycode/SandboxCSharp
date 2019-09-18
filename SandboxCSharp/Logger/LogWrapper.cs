using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SandboxCSharp.Logger
{
    /// <summary>
    /// Implementation of <see cref="ILogger" /> wrapper interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class forwards all the method calls to the <see cref="log4net.ILog" /> interface.
    /// </para>
    /// </remarks>
    /// <seealso cref="ILogger" />
    /// <seealso cref="LogManager" />
    /// <seealso cref="LogManager.GetLogger(System.Type)" />
    /// <author>Stefan Frutiger</author>
    public class LogWrapper : ILogger
    {
        // Internal Remarks: The only reason for building this wrapper class is to avoid 
        // adding a reference to the log4net assembly from "every" project in the solution.

        private readonly log4net.ILog _logger;

        public LogWrapper(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            _logger = log4net.LogManager.GetLogger(type);
        }

        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        public void Debug(string format, params object[] args)
        {
            Debug(Activity.Current, format, args);
        }
        public void Debug(Activity activity, string format, params object[] args)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            SetActivity(activity);
            if (args != null && args.Length > 0)
            {
                _logger.DebugFormat(CultureInfo.InvariantCulture, format, args);
            }
            else
            {
                _logger.Debug(format);
            }
        }

        public bool IsInfoEnabled
        {
            get { return _logger.IsInfoEnabled; }
        }

        public void Info(string format, params object[] args)
        {
            Info(Activity.Current, format, args);
        }

        public void Info(Activity activity, string format, params object[] args)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            SetActivity(activity);
            if (args != null && args.Length > 0)
            {
                _logger.InfoFormat(CultureInfo.InvariantCulture, format, args);
            }
            else
            {
                _logger.Info(format);
            }
        }

        public bool IsWarnEnabled
        {
            get { return _logger.IsWarnEnabled; }
        }

        public void Warn(string format, params object[] args)
        {
            Warn(Activity.Current, format, args);
        }

        public void Warn(Activity activity, string format, params object[] args)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            SetActivity(activity);

            if (args != null && args.Length > 0)
            {
                _logger.WarnFormat(CultureInfo.InvariantCulture, format, args);
            }
            else
            {
                _logger.Warn(format);
            }
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            Warn(exception, Activity.Current, format, args);
        }

        public void Warn(Exception exception, Activity activity, string format, params object[] args)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            if (exception == null) throw new ArgumentNullException("exception");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            SetActivity(activity);

            // Use log4net.Util.SystemStringFormat to defer string "building" until ToString() is called:
            // If Warn is disabled, string.Format() is never called...
            _logger.Warn(new log4net.Util.SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
        }

        public bool IsErrorEnabled
        {
            get { return _logger.IsErrorEnabled; }
        }

        public void Error(string format, params object[] args)
        {
            Error(Activity.Current, format, args);
        }

        public void Error(Activity activity, string format, params object[] args)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            if (args != null && args.Length > 0)
            {
                _logger.ErrorFormat(CultureInfo.InvariantCulture, format, args);
            }
            else
            {
                _logger.Error(format);
            }
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            Error(exception, Activity.Current, format, args);
        }

        public void Error(Exception exception, Activity activity, string format, params object[] args)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            if (activity == null) throw new ArgumentNullException("activity");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            //SetActivity(activity);

            // Use log4net.Util.SystemStringFormat to defer string "building" until ToString() is called:
            // If Error is disabled, string.Format() is never called...
            _logger.Error(new log4net.Util.SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
        }

        public bool IsFatalEnabled
        {
            get { return _logger.IsFatalEnabled; }
        }

        public void Fatal(string format, params object[] args)
        {
            Fatal(Activity.Current, format, args);
        }

        public void Fatal(Activity activity, string format, params object[] args)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            SetActivity(activity);

            if (args != null && args.Length > 0)
            {
                _logger.FatalFormat(CultureInfo.InvariantCulture, format, args);
            }
            else
            {
                _logger.Fatal(format);
            }

        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            Fatal(exception, Activity.Current, format, args);
        }

        public void Fatal(Exception exception, Activity activity, string format, params object[] args)
        {
            if (exception == null) throw new ArgumentNullException("exception");

            if (activity == null) throw new ArgumentNullException("activity");

            if (string.IsNullOrWhiteSpace(format)) throw new ArgumentNullException("format");

            SetActivity(activity);

            // Use log4net.Util.SystemStringFormat to defer string "building" until ToString() is called:
            // If Fatal is disabled, string.Format() is never called...
            _logger.Fatal(new log4net.Util.SystemStringFormat(CultureInfo.InvariantCulture, format, args), exception);
        }

        private void SetActivity(Activity activity)
        {
            if (activity == null) throw new ArgumentNullException("activity");

            // HACK: For major bug in log4net v1.2.12.0 --> https://issues.apache.org/jira/browse/LOG4NET-398
            if (!IsRunningInUnitTest)
            {
                // Due to the log4net bug 398: Do not use LogicalThreadContext when running in a unit test
                log4net.LogicalThreadContext.Properties["Activity"] = activity;
            }
            // END HACK
        }

        private bool? _isRunningInUnitTest;

        private bool IsRunningInUnitTest
        {
            get
            {
                if (!_isRunningInUnitTest.HasValue)
                {
                    // Most simple condition to detect if the program is running in a unit test...
                    // Proposal from Eric Bole-Feysot
                    // See: http://stackoverflow.com/questions/3167617/determine-if-code-is-running-as-part-of-a-unit-test

                    const string testAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";

                    _isRunningInUnitTest = Assembly.GetEntryAssembly() == null &&
                                           AppDomain.CurrentDomain.GetAssemblies()
                                               .Any(a => a.FullName.StartsWith(testAssemblyName));

                    if (_isRunningInUnitTest.Value)
                    {
                        // Log the message just once
                        _logger.InfoFormat(CultureInfo.InvariantCulture, "We're running in a Unit Test and cannot log the current Activity.");
                    }
                }
                return _isRunningInUnitTest.Value;
            }
        }
    }

}
