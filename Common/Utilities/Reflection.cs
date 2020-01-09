using System;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Gets the default value for the provided type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static object GetDefault(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Gets the default value for the provided type
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns></returns>
        public static T GetDefault<T>()
        {
            var t = typeof(T);
            return (T)GetDefault(t);
        }
    }
}