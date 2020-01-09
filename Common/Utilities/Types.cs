using System;

namespace Common
{
    public static partial class GlobalUtilities
    {
        public static TDestination Extend<TSource, TDestination>(TSource source, TDestination destination) where TDestination : TSource
        {
            var sourceType = source.GetType();
            foreach (var property in sourceType.GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(destination, property.GetValue(source));
                }
            }

            return destination;
        }

        /// <summary>
        /// Attempts to parse the provided object as the provided type. If the parsing is unsuccessful, it will reutrn the provided default value.
        /// </summary>
        /// <typeparam name="T">The type to parse your string to.</typeparam>
        /// <param name="s">The object to parse.</param>
        /// <param name="defaultValue">The value that will be returned if parsing is unsuccessful.</param>
        /// <returns></returns>
        public static T TryParse<T>(object s, object defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(s, typeof(T));
            }
            catch
            {
                return (T)defaultValue;
            }
        }
    }
}