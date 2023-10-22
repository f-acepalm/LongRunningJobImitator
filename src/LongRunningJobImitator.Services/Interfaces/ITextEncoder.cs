namespace LongRunningJobImitator.Services.Interfaces;

public interface ITextEncoder
{
    string Decode(string value);

    string Encode(string value);
}