using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

public static class Logger
{
    public static ILog Log;

    public static void Init()
    {
        Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

        TraceAppender tracer = new TraceAppender();
        PatternLayout patternLayout = new PatternLayout();
        patternLayout.ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss::ffff} %level [%thread] %class %method – %message %newline";
        patternLayout.ActivateOptions();
        tracer.Layout = patternLayout;
        tracer.ActivateOptions();
        hierarchy.Root.AddAppender(tracer);

        RollingFileAppender roller = new RollingFileAppender();
        roller.Layout = patternLayout;
        roller.AppendToFile = true;
        roller.RollingStyle = RollingFileAppender.RollingMode.Size;
        roller.MaxSizeRollBackups = 21;
        roller.MaximumFileSize = "500KB";
        roller.StaticLogFileName = true;
        roller.File = "Logger/Logger.log";
        roller.LockingModel = new FileAppender.MinimalLock();
        roller.ActivateOptions();
        hierarchy.Root.AddAppender(roller);

        hierarchy.Root.Level = Level.All;
        hierarchy.Configured = true;

        Log = LogManager.GetLogger(typeof(Logger));
    }
}
