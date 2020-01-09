using System.Web.Mvc;

public static class TempDataExtensions
{
    public static void Put<T>(this TempDataDictionary tempData, T value) where T : class
    {
        tempData[typeof(T).FullName] = value;
    }
    public static void Put<T>(this TempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[typeof(T).FullName + key] = value;
    }
    public static T Get<T>(this TempDataDictionary tempData) where T : class
    {
        object o;
        tempData.TryGetValue(typeof(T).FullName, out o);
        return o == null ? null : (T)o;
    }
    public static T Get<T>(this TempDataDictionary tempData, string key) where T : class
    {
        object o;
        tempData.TryGetValue(typeof(T).FullName + key, out o);
        return o == null ? null : (T)o;
    }
}