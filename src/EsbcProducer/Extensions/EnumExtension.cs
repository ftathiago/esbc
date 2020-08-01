using System;

namespace EsbcProducer.Extensions
{
    public static class EnumExtension
    {
        public static T Parse<T>(this string enumString)
            where T : Enum
        {
            var parsed = Enum.TryParse(typeof(T), enumString, ignoreCase: true, out var result);
            if (!parsed)
            {
                throw new ArgumentException($"{enumString} is not a valid {typeof(T).Name} enum.");
            }

            return (T)result;
        }
    }
}