using System;
using System.Collections;
using System.Linq;

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

        public static T WhenNotEmpty<T>(this T value, string paramName) where T : IEnumerable
        {
            _ = value.WhenNotNull(nameof(value));

            if(!value.Cast<object>().Any())
            {
                throw new ArgumentException("Value cannot be an empty enumerable.", paramName);
            }

            return value;
        }
    }
}