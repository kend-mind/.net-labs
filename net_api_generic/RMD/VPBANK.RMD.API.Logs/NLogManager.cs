using NLog;

namespace VPBANK.RMD.API.Logs
{
    public interface ILoggerManager
    {
        void Debug(string message);
        void Error(string message);
        void Info(string message);
        void Warn(string message);
    }

    public class NLogManager : ILoggerManager
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public void Debug(string message) => logger.Debug(message);
        public void Error(string message) => logger.Error(message);
        public void Info(string message) => logger.Info(message);
        public void Warn(string message) => logger.Warn(message);
    }
}
