namespace LongRunningJobImitator.Models
{
    public class TextConverterResponse
    {
        public string Result { get; set; } = string.Empty;
        
        public int JobId { get; internal set; }
    }
}
