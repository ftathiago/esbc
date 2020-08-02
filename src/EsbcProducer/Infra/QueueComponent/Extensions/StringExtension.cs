using System;

namespace EsbcProducer.Infra.QueueComponent.Extensions
{
    public static class StringExtension
    {
        public static int ParseToInt(this string str, int defaultNumber = 0)
        {
            var hasNumber = !string.IsNullOrEmpty(str);
            if (!hasNumber)
            {
                return defaultNumber;
            }

            var parsed = int.TryParse(str, out var parsedNumber);
            if (parsed)
            {
                return parsedNumber;
            }

            return defaultNumber;
        }

        public static T ParseToEnum<T>(this string enumString)
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