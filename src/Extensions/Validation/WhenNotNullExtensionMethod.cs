using System;

namespace AsIfByMagic.Extensions.Validation
{
    public static class WhenNotNullExtensionMethod
    {
        public static T WhenNotNull<T>(this T value, string paramName)
        {
            // TODO Do we wish to validate there is a paramName? What happens if we pass null or string.Empty to the ctor?

            if(!Equals(value, null))
            {
                return value;
            }

            throw new ArgumentNullException(paramName);
        }
    }
}