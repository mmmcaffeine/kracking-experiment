using System.Linq.Expressions;

namespace AsIfByMagic.Extensions.Validation
{
    public class SubstituteExpressionVisitor : ExpressionVisitor
    {
        private Expression ToReplace { get; }
        private Expression ReplaceWith { get; }

        public SubstituteExpressionVisitor(Expression toReplace, Expression replaceWith)
        {
            ToReplace = toReplace.WhenNotNull(nameof(toReplace));
            ReplaceWith = replaceWith.WhenNotNull(nameof(replaceWith));
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return ReferenceEquals(ToReplace, node) ? ReplaceWith : node;
        }
    }
}
