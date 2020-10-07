namespace AsIfByMagic.Extensions.Validation
{
    public static class WhenSatisfiesExtensionMethod
    {
        public static T WhenSatisfies<T>(this T Value, Rule<T> rule)
        {
            return Value;
        }
    }
}