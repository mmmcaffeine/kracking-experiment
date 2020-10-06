using NUnit;
using NUnit.Framework;
using Shouldly;
using System;

namespace AsIfByMagic.Extensions.Validation
{
    [TestFixture]
    public class WhenNotNullExtensionMethodTestFixture
    {
        public class Service
        {
            private Dependency Dependency { get; }
            public Service(Dependency dependency)
            {
                Dependency = dependency.WhenNotNull(nameof(dependency));
            }
        }

        public class Dependency { }

        [Test]
        public void WhenNotNull_Given_ParamName_Then_ExceptionThrownWithParamName()
        {
            Should.Throw<ArgumentNullException>(() => new Service(null)).ParamName.ShouldBe("dependency");
        }
    }
}