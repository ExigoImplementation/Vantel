using Common;
using System;

/// <summary>
/// Provides functionality to enhance enumerations.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Returns the description of the specified enum.
    /// </summary>
    /// <param name="value">The value of the enum for which to return the description.</param>
    /// <returns>A description of the enum, or the enum name if no description exists.</returns>
    public static string GetDescription(this Enum value)
    {
        return GlobalUtilities.GetEnumDescription(value);
    }
}