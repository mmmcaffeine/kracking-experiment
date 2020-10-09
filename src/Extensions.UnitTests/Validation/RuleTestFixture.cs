using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AsIfByMagic.Extensions.Validation
{
    [TestFixture]
    public class RuleTestFixture
    {
        private class Entity
        {
            public string Name { get; set; }
        }

        [Test]
        public void Foo()
        {
            Func<Entity, bool> predicate = x => !string.IsNullOrEmpty(x.Name);
            Rule<Entity> rule = predicate;
            var unnamedEntity = new Entity {Name = null};
            var namedEntity = new Entity {Name = "Name"};

            Console.WriteLine(predicate.Method.ToString());

            rule.SatisfiedBy(unnamedEntity).ShouldBeFalse();
            rule.SatisfiedBy(namedEntity).ShouldBeTrue();
        }

        [Test]
        public void Bar()
        {
            //Expression<Func<Entity, bool>> expression = x => !string.IsNullOrEmpty(x.Name);
            Expression<Func<Entity, bool>> expression = x => x.Name.StartsWith("H");

            Func<Entity, bool> predicate = x => char.IsUpper(x.Name[0]);
            Expression<Func<Entity, bool>> foo = x => predicate(x);

            Console.WriteLine(foo.ToString());
        }

        [Test]
        public void FooBar()
        {
            Rule<Entity> rule = new Rule<Entity>(x => char.IsUpper(x.Name[0]));
            Func<Entity, bool> predicate = (Func<Entity, bool>)rule;
            var entity = new Entity {Name = "Homer"};

            predicate(entity).ShouldBeTrue();

        }

        [Test]
        public void Queryable()
        {
            var list = new List<Entity>
            {
                new Entity {Name = "Homer"},
                new Entity {Name = "Marge"},
                new Entity {Name = "Bart"},
                new Entity {Name = "Lisa"},
                new Entity {Name = "Maggie"}
            };
            _ = list.AsQueryable();
        }
    }
}
