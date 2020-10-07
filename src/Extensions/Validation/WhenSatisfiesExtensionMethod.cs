namespace AsIfByMagic.Extensions.Validation
{
    public static class WhenSatisfiesExtensionMethod
    {
        public static T WhenSatisfies<T>(this T value, IRule<T> rule)
        {
            _ = value.WhenNotNull(nameof(value));
            _ = rule.WhenNotNull(nameof(rule));

            if(!rule.SatisfiedBy(value))
            {
                throw rule.CreateException(value);
            }

            return value;
        }
    }
}