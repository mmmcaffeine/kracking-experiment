using NUnit.Framework;
using Shouldly;
using System;

namespace AsIfByMagic.Extensions.Validation
{
    [TestFixture]
    public class WhenNotEmptyExtensionMethodTestFixture
    {
        [Test]
        public void WhenNotEmpty_String_Given_ValueIsNull_Then_ExceptionThrown()
        {
            Should.Throw<ArgumentNullException>(() => ((string)null).WhenNotEmpty("parameter")).ParamName.ShouldBe("parameter");
        }

        [Test]
        public void WhenNotEmpty_String_Given_ValueIsEmpty_Then_ExceptionThrown()
        {
            var thrown = Should.Throw<ArgumentException>(() => string.Empty.WhenNotEmpty("parameter"));

            thrown.Message.ShouldStartWith("Value cannot be an empty string.");
            thrown.ParamName.ShouldBe("parameter");
        }

        [Test]
        public void WhenNotEmpty_String_Given_ValueIsNotNullOrEmpty_Then_Value()
        {
            "value".WhenNotEmpty("parameter").ShouldBe("value");
        }

        [Test]
        public void WhenNotEmpty_Enumerable_Given_ValueIsNull_Then_ExceptionThrown()
        {
            Should.Throw<ArgumentNullException>(() => ((string[])null).WhenNotEmpty("nullArray")).ParamName.ShouldBe("value");
        }

        [Test]
        public void WhenNotEmpty_Enumerable_Given_EmptyEnumerable_Then_ExceptionThrown()
        {
            var thrown = Should.Throw<ArgumentException>(() => new string[]{}.WhenNotEmpty("emptyArray"));

            thrown.Message.ShouldStartWith("Value cannot be an empty enumerable.");
            thrown.ParamName.ShouldBe("emptyArray");
        }

        [Test]
        public void WhenNotEmpty_Enumerable_GivenNotEmptyEnumerable_Then_Value()
        {
            // Arrange
            var value = new[]{"These", "Are", "Some", "Strings"};

            // Act
            var actual = value.WhenNotEmpty("array");

            // Assert
            actual.ShouldBeSameAs(value);
        }
    }
}