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
            Expression = expression;
            Predicate = Expression.Compile();
        }

        public Exception CreateException(T value)
        {
            throw new Exception($"The passed value does not meet the criteria of: '{ Expression }'.");
        }

        public bool SatisfiedBy(T value)
        {
            return Predicate(value);
        }

        public static Rule<T> operator &(Rule<T> a, Rule<T> b)
        {
            var parameterA = a.Expression.Parameters[0];
            var parameterB = b.Expression.Parameters[0];
            var visitor = new SubstituteExpressionVisitor(parameterB, parameterA);
            var body = System.Linq.Expressions.Expression.AndAlso(a.Expression.Body, visitor.Visit(b.Expression.Body));
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, parameterA);

            return new Rule<T>(lambda);
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
