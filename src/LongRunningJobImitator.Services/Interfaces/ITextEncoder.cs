using LongRunningJobImitator.Services.Models;

namespace LongRunningJobImitator.Services.Interfaces;

public interface ITextEncoder
{
    string Decode(DecodeModel model);

    string Encode(EncodeModel model);
}