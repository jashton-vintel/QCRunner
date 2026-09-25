using System.ComponentModel;
using System.Reflection;

namespace QCRunner.Common;

public static class EnumExtensions
{
    /// <summary>
    /// Returns the text of the <see cref="DescriptionAttribute"/> applied to an enum member,
    /// falling back to the member name when no description has been declared.
    /// </summary>
    public static string GetDescription(this Enum value)
    {
        string name = value.ToString();
        FieldInfo? field = value.GetType().GetField(name);

        return field?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name;
    }
}
