using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.Services
{
    public class SameTextConverter : ITextConverter
    {
        public string Convert(string text)
        {
            return text;
        }
    }
}
