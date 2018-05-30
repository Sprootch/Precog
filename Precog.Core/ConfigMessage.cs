namespace Precog.Core
{
    public class ConfigMessage
    {
        private ConfigMessage(string message, Severity severity)
        {
            Message = message;
            Severity = severity;
        }

        public string Message { get; }
        public Severity Severity { get; }

        internal static ConfigMessage Error(string error)
        {
            return new ConfigMessage(error, Severity.Error);
        }

        internal static ConfigMessage Info(string message)
        {
            return new ConfigMessage(message, Severity.Info);
        }

        internal static ConfigMessage Success(string message)
        {
            return new ConfigMessage(message, Severity.Success);
        }
    }
}
