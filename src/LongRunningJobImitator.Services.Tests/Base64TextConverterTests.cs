using FluentAssertions;
using LongRunningJobImitator.Services.Tests.AutoFixtureConfiguration;

namespace LongRunningJobImitator.Services.Tests
{
    public class Base64TextConverterTests
    {
        
        [Theory]
        [AutoMoqData]
        public void Base64Encode_SimpleText_ExpectedResult(Base64TextConverter sut)
        {
            // Arrange
            var input = "Some simple text.";
            var expected = "U29tZSBzaW1wbGUgdGV4dC4=";
            
            // Act
            var actual = sut.Base64Encode(input);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [AutoMoqData]
        public void Base64Encode_SpecialCharactersAndUnicode_ExpectedResult(Base64TextConverter sut)
        {
            // Arrange
            var input = "!@#$%^&*())_+פûגא";
            var expected = "IUAjJCVeJiooKSlfK9GE0YvQstCw";

            // Act
            var actual = sut.Base64Encode(input);

            // Assert
            actual.Should().Be(expected);
        }


        [Theory]
        [AutoMoqData]
        public void Base64Encode_Decode_SameValue(Base64TextConverter sut)
        {
            // Arrange
            var input = "!@#$%^&*())_+פûגא";

            // Act
            var actual = sut.Base64Encode(input);
            var decoded = sut.Base64Decode(actual);

            // Assert
            decoded.Should().Be(input);
        }

        [Theory]
        [AutoMoqData]
        public void Base64Encode_MultilineText_ExpectedResult(Base64TextConverter sut)
        {
            // Arrange
            var input = """
                Some
                multiline
                text
                """;
            var expected = "U29tZQ0KbXVsdGlsaW5lDQp0ZXh0";

            // Act
            var actual = sut.Base64Encode(input);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [AutoMoqData]
        public void Base64Encode_LongText_ExpectedResult(Base64TextConverter sut)
        {
            // Arrange
            var input = "Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string ";
            var expected = "TG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcg";

            // Act
            var actual = sut.Base64Encode(input);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [AutoMoqData]
        public void Base64Encode_EmptyText_ArgumentException(Base64TextConverter sut)
        {
            // Arrange
            var input = string.Empty;

            // Act
            var action = () => sut.Base64Encode(input);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [AutoMoqData]
        public void Base64Encode_NullText_ArgumentException(Base64TextConverter sut)
        {
            // Act
            var action = () => sut.Base64Encode(null!);

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }
}