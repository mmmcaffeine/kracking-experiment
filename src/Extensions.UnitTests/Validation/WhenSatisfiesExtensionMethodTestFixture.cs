using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;

namespace AsIfByMagic.Extensions.Validation
{
    [TestFixture]
    public class WhenSatisifiesExtensionMethodTestFixture
    {
        public class Entity
        {
            public string Name { get; set; }
        }

        [Test]
        public void WhenSatisfies_Given_RuleIsSatisfied_Then_Value()
        {
            // Arrange
            var entity = new Entity { Name = "Homer" };
            var rule = Substitute.For<IRule<Entity>>();

            // Act
            var actual = entity.WhenSatisfies(rule);

            // Assert
            actual.ShouldBeSameAs(entity);
        }

        [Test]
        public void WhenSatisifes_Given_RuleIsNotSatisfied_Then_ExceptionCreatedByRuleThrown()
        {
            // Arrange
            var entity = new Entity { Name = "Bart" };
            var exception = new Exception("I will not break the rules... I will not break the rulees...");
            var rule = Substitute.For<IRule<Entity>>();

            rule.SatisfiedBy(entity).Returns(false);
            rule.CreateException(entity).Returns(exception);

            // Act
            var actual = Should.Throw<Exception>(() => _ = entity.WhenSatisfies(rule));

            // Assert
            actual.ShouldBeSameAs(exception);
        }
    }
}