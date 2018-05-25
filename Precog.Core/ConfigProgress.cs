namespace Precog.Core
{
    public class ConfigProgress
    {
        public ConfigProgress(int value, string message, bool error)
        {
            Value = value;
            Message = message;
            Error = error;
        }

        public int Value { get; }
        public string Message { get; }
        public bool Error { get; }
    }
}
