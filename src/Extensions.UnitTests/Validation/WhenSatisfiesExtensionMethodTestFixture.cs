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
        public void WhenSatisfies_Rule_Given_ValueIsNull_Then_Exception()
        {
            // Arrange
            var entity = (Entity) null;
            var rule = Substitute.For<IRule<Entity>>();

            // Act. Assert
            var actual = Should.Throw<ArgumentNullException>(() => entity.WhenSatisfies(rule));

            // Assert
            actual.ParamName.ShouldBe("value");
        }

        [Test]
        public void WhenSatisfies_Rule_Given_RuleIsNull_Then_Exception()
        {
            // Arrange
            var entity = new Entity { Name = "Maggie" };
            var rule = (IRule<Entity>) null;

            // Act. Assert
            var actual = Should.Throw<ArgumentNullException>(() => entity.WhenSatisfies(rule));

            // Assert
            actual.ParamName.ShouldBe("rule");
        }

        [Test]
        public void WhenSatisfies_Rule_Given_RuleIsSatisfied_Then_Value()
        {
            // Arrange
            var entity = new Entity { Name = "Homer" };
            var rule = Substitute.For<IRule<Entity>>();

            rule.SatisfiedBy(entity).Returns(true);

            // Act
            var actual = entity.WhenSatisfies(rule);

            // Assert
            actual.ShouldBeSameAs(entity);
        }

        [Test]
        public void WhenSatisfies_Rule_Given_RuleIsNotSatisfied_Then_ExceptionCreatedByRuleThrown()
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

        [Test]
        public void WhenSatisfies_Expression_Given_ExpressionIsSatisfied_Then_Value()
        {
            // Arrange
            var entity = new Entity {Name = "Bart"};

            // Act
            var actual = entity.WhenSatisfies(x => x.Name.StartsWith("B"));

            // Assert
            actual.ShouldBeSameAs(entity);
        }

        [Test]
        public void WhenSatisfies_Expression_Given_ExpressionIsNotSatisfied_Then_ExceptionThrown()
        {
            // Arrange
            var entity = new Entity {Name = "Bart"};

            // Act
            var thrown = Should.Throw<Exception>(() => _ = entity.WhenSatisfies(x => x.Name.StartsWith("H")));
            
            // Assert
            thrown.Message.ShouldStartWith("The value does not pass the specification defined by the expression");
            thrown.Message.ShouldContain("x => x.Name.StartsWith(\"H\")");
            thrown.Message.ShouldEndWith(".");
        }
    }
}