using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Suap.Identity.Logic.Implementations;
public static class EnumDescriptionProvider
{

    /// <summary>
    /// Получить словарь (значение, описание)
    /// </summary>      
    public static Dictionary<T, string> AsDictionary<T>(Type type, Func<Enum, T> func)
    {
        Dictionary<T, string> result = Enum.GetValues(type).Cast<Enum>()
            .ToDictionary(func, x => x.GetDescription());
        return result;
    }

    /// <summary>
    /// Получить значение enum из description
    /// </summary>
    public static T ParseDescription<T>(string source) where T : Enum
    {
        var dict = AsDictionary<T>(typeof(T), x => (T)x);

        foreach (var item in dict)
            if (item.Value == source) return (T)item.Key;

        throw new FormatException(nameof(source));
    }

    /// <summary>
    /// Из значения enum получить описание
    /// </summary>        
    public static string GetDescription(this Enum source)
    {
        FieldInfo fi = source.GetType().GetField(source.ToString());

        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Any())
        {
            return attributes.First().Description;
        }

        return source.ToString();
    }
}
