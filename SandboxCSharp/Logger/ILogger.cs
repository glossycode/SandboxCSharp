using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCSharp.Logger
{
    /// <summary>
    /// The ILogger interface is used by the Quickline application to write log messages 
    /// into a log repository (file, database, ...) depending on the log configuration.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use the <see cref="LogManager" /> to obtain logger instances
    /// that implement this interface. The <see cref="LogManager.GetLogger(System.Type)" />
    /// static method is used to get logger instances.
    /// </para>
    /// <para>
    /// This class contains methods for logging at different levels and also
    /// has properties for determining if those logging levels are
    /// enabled in the current configuration.
    /// </para>
    /// </remarks>
    /// <example>Simple example of logging messages
    /// <code lang="C#">
    /// private static readonly ILogger _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    ///
    /// public void LogMethod()
    /// {
    ///     _logger.Debug("This is a debug message");
    ///
    ///     if (_logger.IsDebugEnabled)
    ///     {
    ///         _logger.Debug("This is another debug message");
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="LogManager" />
    /// <seealso cref="LogManager.GetLogger(System.Type)" />
    /// <author>Stefan Frutiger</author>
    public interface ILogger
    {
        // Internal Remarks: The only reason for building this ILogger interface is to avoid 
        // adding a reference to the log4net assembly from "every" project in the solution.
        // This ILogger interface is somewhat simplified and "reduced to the max" compared to
        // the "full-fledged" log4net.ILog interface.

        bool IsDebugEnabled { get; }
        void Debug(string format, params object[] args);
        //void Debug(Activity activity, string format, params object[] args);

        bool IsInfoEnabled { get; }
        void Info(string format, params object[] args);
        //void Info(Activity activity, string format, params object[] args);

        bool IsWarnEnabled { get; }
        void Warn(string format, params object[] args);
        //void Warn(Activity activity, string format, params object[] args);
        void Warn(Exception exception, string format, params object[] args);
        //void Warn(Exception exception, Activity activity, string format, params object[] args);

        bool IsErrorEnabled { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error",
            Justification = "The Error method has been naturalised and is also part of the log4net.ILog interface.")]
        void Error(string format, params object[] args);
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error",
        //    Justification = "The Error method has been naturalised and is also part of the log4net.ILog interface.")]
        //void Error(Activity activity, string format, params object[] args);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error",
            Justification = "The Error method has been naturalised and is also part of the log4net.ILog interface.")]
        void Error(Exception exception, string format, params object[] args);
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error",
        //    Justification = "The Error method has been naturalised and is also part of the log4net.ILog interface.")]
        //void Error(Exception exception, Activity activity, string format, params object[] args);

        bool IsFatalEnabled { get; }
        void Fatal(string format, params object[] args);
        //void Fatal(Activity activity, string format, params object[] args);
        void Fatal(Exception exception, string format, params object[] args);
        //void Fatal(Exception exception, Activity activity, string format, params object[] args);
    }
}
