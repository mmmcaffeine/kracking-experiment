using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;

namespace AsIfByMagic.Extensions.Validation
{
    [TestFixture]
    public class WhenSatisfiesExtensionMethodTestFixture
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
            var exception = new Exception("I will not break the rules... I will not break the rules...");
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
            thrown.Message.ShouldStartWith("The passed value does not meet the criteria of:");
            thrown.Message.ShouldContain("x => x.Name.StartsWith(\"H\")");
            thrown.Message.ShouldEndWith(".");
        }

        [Test]
        public void WhenSatisfies_Rule_Given_CompositeRuleIsNotSatisfied_Then_ThrownExceptionHasFriendlyMessage()
        {
            // Arrange
            var startsWithRule = new Rule<Entity>(x => x.Name.StartsWith("B"));
            var endsWithRule = new Rule<Entity>(x => x.Name.EndsWith("t"));
            var combinedRule = startsWithRule & endsWithRule;
            var entity = new Entity {Name = "Homer"};

            // Act
            var actual = Should.Throw<Exception>(() => _ = entity.WhenSatisfies(combinedRule));

            // Assert
            actual.Message.ShouldStartWith("The passed value does not meet the criteria of:");
            actual.Message.ShouldContain("x => (x.Name.StartsWith(\"B\") AndAlso x.Name.EndsWith(\"t\"))");
            actual.Message.ShouldEndWith(".");
        }
    }
}