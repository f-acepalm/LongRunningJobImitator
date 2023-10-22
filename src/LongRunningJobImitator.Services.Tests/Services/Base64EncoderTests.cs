using AutoFixture.Xunit2;
using FluentAssertions;
using LongRunningJobImitator.Services.Interfaces;
using LongRunningJobImitator.Services.Services;
using LongRunningJobImitator.Services.Tests.AutoFixtureConfiguration;
using Moq;
using System.Text;

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
        var actual = sut.Encode(input);

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
        var actual = sut.Encode(input);

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
        var actual = sut.Encode(input);
        var decoded = sut.Decode(actual);

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
        var actual = sut.Encode(input);

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
        var actual = sut.Encode(input);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [AutoMoqData]
    public void Encode_EmptyText_ArgumentException(Base64Encoder sut)
    {
        // Arrange
        var input = string.Empty;

        // Act
        var action = () => sut.Encode(input);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [AutoMoqData]
    public void Encode_NullText_ArgumentException(Base64Encoder sut)
    {
        // Act
        var action = () => sut.Encode(null!);

        // Assert
        action.Should().Throw<ArgumentException>();
    }
}