namespace AsIfByMagic.Extensions.Validation
{
    public static class WhenSatisfiesExtensionMethod
    {
        public static T WhenSatisfies<T>(this T value, IRule<T> rule)
        {
            if(!rule.SatisfiedBy(value))
            {
                throw rule.CreateException(value);
            }

            return value;
        }
    }
}