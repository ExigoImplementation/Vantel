using System;
using System.Reflection;

public static class ObjectExtensions
{
    /// <summary>
    /// Converts an anonymous type object into a specified type.
    /// </summary>
    /// <typeparam name="T">The type of object to convert to</typeparam>
    /// <param name="obj">The object to convert</param>
    /// <param name="type">The type of object to convert to</param>
    /// <returns></returns>
    public static object ToType<T>(this object obj, T type)
    {
        //create instance of T type object:
        var tmp = Activator.CreateInstance(Type.GetType(type.ToString()));

        //loop through the properties of the object you want to covert:          
        foreach (PropertyInfo pi in obj.GetType().GetProperties())
        {
            try
            {
                //get the value of property and try 
                //to assign it to the property of T type object:
                tmp.GetType().GetProperty(pi.Name).SetValue(tmp, pi.GetValue(obj, null), null);
            }
            catch { }
        }

        //return the T type object:         
        return tmp;
    }
}