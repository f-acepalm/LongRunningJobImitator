using System.Text;
using FluentValidation;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Models;
using LongRunningJobImitator.Services.Validation;

namespace LongRunningJobImitator.Services.Services;
public class Base64Encoder : ITextEncoder
{
    private readonly IValidator<DecodeModel> _decodeValidator;
    private readonly IValidator<EncodeModel> _encodeValidator;

    public Base64Encoder(DecodeModelValidator decodeValidator, IValidator<EncodeModel> encodeValidator)
    {
        _decodeValidator = decodeValidator;
        _encodeValidator = encodeValidator;
    }

    public string Encode(EncodeModel model)
    {
        _encodeValidator.ValidateAndThrow(model);
        var textBytes = Encoding.UTF8.GetBytes(model.Value);

        return Convert.ToBase64String(textBytes);
    }

    public string Decode(DecodeModel model)
    {
        _decodeValidator.ValidateAndThrow(model);
        var base64EncodedBytes = Convert.FromBase64String(model.Value);

        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
