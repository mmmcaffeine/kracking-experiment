using System;
using System.Linq.Expressions;

namespace AsIfByMagic.Extensions.Validation
{
    public class Rule<T> : IRule<T>
    {
        private Expression<Func<T, bool>> PredicateExpression { get; }
        private Func<T, bool> CompiledPredicate { get; }

        public Rule(Expression<Func<T, bool>> expression)
        {
            PredicateExpression = expression;
            CompiledPredicate = PredicateExpression.Compile();
        }

        public Exception CreateException(T value)
        {
            throw new Exception($"The passed value does not meet the criteria of: '{ PredicateExpression }'.");
        }

        public bool SatisfiedBy(T value)
        {
            return CompiledPredicate(value);
        }

        public static Rule<T> operator &(Rule<T> a, Rule<T> b)
        {
            var parameterA = a.PredicateExpression.Parameters[0];
            var parameterB = b.PredicateExpression.Parameters[0];
            var visitor = new SubstituteExpressionVisitor(parameterB, parameterA);
            var body = Expression.AndAlso(a.PredicateExpression.Body, visitor.Visit(b.PredicateExpression.Body));
            var lambda = Expression.Lambda<Func<T, bool>>(body, parameterA);

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
