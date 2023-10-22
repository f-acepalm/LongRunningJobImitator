using System.Text;
using LongRunningJobImitator.Services.Interfaces;

namespace LongRunningJobImitator.Services.Services;
public class Base64Encoder : ITextEncoder
{
    public string Encode(string value)
    {
        ValidateInput(value);
        var textBytes = Encoding.UTF8.GetBytes(value);

        return Convert.ToBase64String(textBytes);
    }

    public string Decode(string value)
    {
        ValidateInput(value);
        var base64EncodedBytes = Convert.FromBase64String(value);

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
