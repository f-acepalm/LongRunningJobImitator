using LongRunningJobImitator.Services.Interfaces;
using System.Text;

namespace LongRunningJobImitator.Services
{
    public class Base64TextConverter : ITextConverter
    {
        public string Convert(string text)
        {
            ValidateInput(text);

            return Base64Encode(text);
        }

        public string Base64Encode(string text)
        {
            ValidateInput(text);
            var textBytes = Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(textBytes);
        }

        public string Base64Decode(string base64String)
        {
            ValidateInput(base64String);
            var base64EncodedBytes = System.Convert.FromBase64String(base64String);
            
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static void ValidateInput(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text can not be empty");
            }
        }
    }
}
