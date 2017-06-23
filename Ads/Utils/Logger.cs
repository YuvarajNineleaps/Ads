using System;
using System.Diagnostics;
using Ads.Controllers;
using log4net;

public static class Logger
{

    private static ILog logger = LogManager.GetLogger(typeof(Logger));

    public static void Error(string message, string module)
    {
        WriteEntry(message, "error", module);
        logger.Error(message, new Exception(module));
    }

    public static void Error(Exception ex, string module)
    {
        WriteEntry(ex.Message, "error", module);
        logger.Error(ex.Message + "in module " + module, ex);

    }

    public static void Warning(string message, string module)
    {
        WriteEntry(message, "warning", module);
        logger.Warn(message, new Exception(module));

    }

    public static void Info(string message, string module)
    {
        WriteEntry(message, "info", module);
        logger.Info(message, new Exception(module));

    }

    private static void WriteEntry(string message, string type, string module)
    {
        Trace.WriteLine(
                string.Format("{0}:{1}:{2}:{3}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              type,
                              module,
                              message));
    }

    internal static void Info(string v, AdController adController)
    {
        throw new NotImplementedException();
    }
}