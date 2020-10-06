using NUnit.Framework;
using Shouldly;
using System;

namespace AsIfByMagic.Extensions.Validation
{
    [TestFixture]
    public class WhenNotEmptyExtensionMethodTestFixture
    {
        [Test]
        public void WhenNotEmpty_Given_ValueIsNull_Then_ExceptionThrown()
        {
            Should.Throw<ArgumentNullException>(() => ((string)null).WhenNotEmpty("parameter")).ParamName.ShouldBe("parameter");
        }

        [Test]
        public void WhenNotEmpty_Given_ValueIsEmpty_Then_ExceptionThrown()
        {
            var thrown = Should.Throw<ArgumentException>(() => string.Empty.WhenNotEmpty("parameter"));

            thrown.Message.ShouldStartWith("Value cannot be an empty string.");
            thrown.ParamName.ShouldBe("parameter");
        }

        [Test]
        public void WhenNotEmpty_Given_ValueIsNotNullOrEmpty_Then_Value()
        {
            "value".WhenNotEmpty("parameter").ShouldBe("value");
        }
    }
}