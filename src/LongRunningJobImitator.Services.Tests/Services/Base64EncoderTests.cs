using FluentAssertions;
using LongRunningJobImitator.Services.Services;
using LongRunningJobImitator.Services.Tests.AutoFixtureConfiguration;

namespace LongRunningJobImitator.Services.Tests.Services;

public class Base64EncoderTests
{

    [Theory]
    [AutoMoqData]
    public void Encode_SimpleText_ExpectedResult(Base64Encoder sut)
    {
        // Arrange
        var input = "Some simple text.";
        var expected = "U29tZSBzaW1wbGUgdGV4dC4=";

        // Act
        var actual = sut.Encode(new(input));

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [AutoMoqData]
    public void Encode_SpecialCharactersAndUnicode_ExpectedResult(Base64Encoder sut)
    {
        // Arrange
        var input = "!@#$%^&*())_+פûגא";
        var expected = "IUAjJCVeJiooKSlfK9GE0YvQstCw";

        // Act
        var actual = sut.Encode(new(input));

        // Assert
        actual.Should().Be(expected);
    }


    [Theory]
    [AutoMoqData]
    public void Encode_Decode_SameValue(Base64Encoder sut)
    {
        // Arrange
        var input = "!@#$%^&*())_+פûגא";

        // Act
        var actual = sut.Encode(new(input));
        var decoded = sut.Decode(new(actual));

        // Assert
        decoded.Should().Be(input);
    }

    [Theory]
    [AutoMoqData]
    public void Encode_MultilineText_ExpectedResult(Base64Encoder sut)
    {
        // Arrange
        var input = """
                Some
                multiline
                text
                """;
        var expected = "U29tZQ0KbXVsdGlsaW5lDQp0ZXh0";

        // Act
        var actual = sut.Encode(new(input));

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [AutoMoqData]
    public void Encode_LongText_ExpectedResult(Base64Encoder sut)
    {
        // Arrange
        var input = "Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string ";
        var expected = "TG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcg";

        // Act
        var actual = sut.Encode(new(input));

        // Assert
        actual.Should().Be(expected);
    }
}