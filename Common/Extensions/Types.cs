using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class TypeExtensions
{
    /// <summary>
    /// Cast an object to the provided type
    /// </summary>
    /// <typeparam name="T">The type to convert the source to</typeparam>
    /// <param name="source">The object to convert</param>
    /// <returns>The typed source</returns>
    public static T As<T>(this object source) where T : class
    {
        return (source as T);
    }

    /// <summary>
    /// Parses an object into an Enum. This method will attempt to convert the provided object to a string.
    /// </summary>
    /// <typeparam name="T">The type of the Enum</typeparam>
    /// <param name="value">String value to parse</param>
    /// <returns>The typed Enum value</returns>
    public static T ToEnum<T>(this object value)
    {
        return TypeExtensions.ToEnum<T>(value, true);
    }

    /// <summary>
    /// Parses an object into an Enum. This method will attempt to convert the provided object to a string.
    /// </summary>
    /// <typeparam name="T">The type of the Enum</typeparam>
    /// <param name="value">String value to parse</param>
    /// <param name="ignorecase">Whether the casing should be ignored when parsing the provided value</param>
    /// <returns>The typed Enum value</returns>
    public static T ToEnum<T>(this object value, bool ignorecase)
    {

        if (value == null)
        {
            throw new ArgumentNullException("value");
        }

        var valueAsString = value.ToString().Trim();

        if (valueAsString.Length == 0)
        {
            throw new ArgumentException("Must specify valid information for parsing in the string.", "value");
        }

        Type t = typeof(T);

        if (!t.IsEnum)
        {
            throw new ArgumentException("Type provided must be an Enum.", "T");
        }

        return (T)Enum.Parse(t, valueAsString, ignorecase);
    }

    /// <summary>
    ///  Determines if the provided object can be parsed as the provided type. Essentially a condensed TryParse.
    /// </summary>
    /// <typeparam name="T">The type of object to attempt to parse the provided object as.</typeparam>
    /// <param name="objectToBeParsed">The object to attempt to parse.</param>
    /// <returns>Whether or not the object can be parsed as the provided type.</returns>
    public static bool CanBeParsedAs<T>(this object objectToBeParsed)
    {
        try
        {
            var castedObject = Convert.ChangeType(objectToBeParsed, typeof(T));
            return castedObject != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Copies properties from one object to another of the same type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="dest"></param>
    public static void CopyPropertiesTo<T>(this T source, T dest)
    {
        var plist = from prop in typeof(T).GetProperties() where prop.CanRead && prop.CanWrite select prop;

        foreach (PropertyInfo prop in plist)
        {
            prop.SetValue(dest, prop.GetValue(source, null), null);
        }
    }


    /// <summary>
    /// Deep-clone two objects of the same type
    /// </summary>
    /// <typeparam name="T">The type of objects</typeparam>
    /// <param name="source">The object to clone</param>
    /// <returns></returns>
    public static T DeepClone<T>(this T source)
    {
        using (var stream = new MemoryStream())
        {
            var dcs = new DataContractSerializer(typeof(T));
            dcs.WriteObject(stream, source);
            stream.Position = 0;
            return (T)dcs.ReadObject(stream);
        }
    }

    // NOTE: THE FOLLOWING METHOD WAS NOT WORKING FOR ME WHEN I TRIED TO USE IT - Mike M.
    /// <summary>
    /// Deep-clone two objects of the same type
    /// </summary>
    /// <typeparam name="T">The type of objects</typeparam>
    /// <param name="source">The object to clone</param>
    /// <returns></returns>
    //public static T DeepClone<T>(this T source)
    //{
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        formatter.Serialize(stream, source);
    //        stream.Position = 0;
    //        return (T)formatter.Deserialize(stream);
    //    }
    //}
}