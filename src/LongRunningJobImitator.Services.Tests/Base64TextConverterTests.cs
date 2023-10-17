using FluentAssertions;

namespace LongRunningJobImitator.Services.Tests
{
    public class Base64TextConverterTests
    {
        private Base64TextConverter _sut = new Base64TextConverter();
        
        [Fact]
        public void Convert_SimpleText_ExpectedResult()
        {
            // Arrange
            var input = "Some simple text.";
            var expected = "U29tZSBzaW1wbGUgdGV4dC4=";
            
            // Act
            var actual = _sut.Convert(input);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Convert_SpecialCharactersAndUnicode_ExpectedResult()
        {
            // Arrange
            var input = "!@#$%^&*())_+פûגא";
            var expected = "IUAjJCVeJiooKSlfK9GE0YvQstCw";

            // Act
            var actual = _sut.Convert(input);

            // Assert
            actual.Should().Be(expected);
        }


        [Fact]
        public void Convert_Decode_SameValue()
        {
            // Arrange
            var input = "!@#$%^&*())_+פûגא";

            // Act
            var actual = _sut.Convert(input);
            var decoded = _sut.Base64Decode(actual);

            // Assert
            decoded.Should().Be(input);
        }

        [Fact]
        public void Convert_MultilineText_ExpectedResult()
        {
            // Arrange
            var input = """
                Some
                multiline
                text
                """;
            var expected = "U29tZQ0KbXVsdGlsaW5lDQp0ZXh0";

            // Act
            var actual = _sut.Convert(input);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Convert_LongText_ExpectedResult()
        {
            // Arrange
            var input = "Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string Long string ";
            var expected = "TG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcgTG9uZyBzdHJpbmcg";

            // Act
            var actual = _sut.Convert(input);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Convert_EmptyText_ArgumentException()
        {
            // Arrange
            var input = string.Empty;

            // Act
            var action = () => _sut.Convert(input);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Convert_NullText_ArgumentException()
        {
            // Act
            var action = () => _sut.Convert(null!);

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }
}