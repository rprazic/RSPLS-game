using System.ComponentModel;

namespace GameService.Domain.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Retrieves the description attribute of an enumeration value.
    /// </summary>
    /// <param name="value">The enumeration value to get the description for.</param>
    /// <returns>The description string if available; otherwise, an empty string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the enum value is null.</exception>
    public static string ToDescriptionString(this Enum value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "Enum value cannot be null.");
        }

        if (!Enum.TryParse(value.GetType(), value.ToString(), out _))
        {
            return string.Empty;
        }

        var attributes = (DescriptionAttribute[])value
            .GetType()
            .GetField(value.ToString())
            ?.GetCustomAttributes(typeof(DescriptionAttribute), false)!;
        return attributes is { Length: > 0 } ? attributes[0].Description : string.Empty;
    }

    /// <summary>
    /// Converts an enumeration value to its integer representation.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enumeration value to convert.</param>
    /// <returns>The integer representation of the enumeration value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the enum value is null.</exception>
    public static int ToInt<T>(this T value) where T : Enum
    {
        return value is null
            ? throw new ArgumentNullException(nameof(value), "Enum value cannot be null.")
            : Convert.ToInt32(value);
    }
}