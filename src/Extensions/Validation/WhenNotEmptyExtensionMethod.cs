using System;

namespace AsIfByMagic.Extensions.Validation
{
    public static class WhenNotEmptyExtensionMethod
    {
        public static string WhenNotEmpty(this string value, string paramName)
        {
            if(value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if(value == string.Empty)
            {
                throw new ArgumentException("Value cannot be an empty string.", paramName);
            }

            return value;
        }
    }
}