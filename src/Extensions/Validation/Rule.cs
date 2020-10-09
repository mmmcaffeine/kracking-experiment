using System;
using System.Linq.Expressions;

namespace AsIfByMagic.Extensions.Validation
{
    public class Rule<T> : IRule<T>
    {
        private Expression<Func<T, bool>> Expression { get; }
        private Func<T, bool> Predicate { get; }


        public Rule(Expression<Func<T, bool>> expression)
        {
            Expression = expression.WhenNotNull(nameof(expression));
            Predicate = Expression.Compile();
        }

        public bool SatisfiedBy(T value)
        {
            return Predicate(value);
        }

        public Exception CreateException(T value)
        {
            return new Exception($"The value does not pass the specification defined by the expression '{ Expression }'.");
        }

        public static explicit operator Func<T, bool>(Rule<T> rule)
        {
            return rule.SatisfiedBy;
        }

        public static implicit operator Rule<T>(Func<T, bool> predicate)
        {
            return new Rule<T>(x => predicate(x));
        }
        
        public static implicit operator Rule<T>(Expression<Func<T, bool>> expression)
        {
            return new Rule<T>(expression);
        }
    }
}
