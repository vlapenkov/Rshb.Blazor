using System.Reflection;

namespace Suap.Triast.Scripts;

public static class SqlTextProvider
{

    public static string GetSqlText(string scriptName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        string scriptFullName = $"{assembly.GetName().Name}.SQL.{scriptName}";
        var resourceNames =
                    assembly.GetManifestResourceNames().
                    Where(str => str.Equals(scriptFullName, StringComparison.InvariantCultureIgnoreCase));

        if (resourceNames == null)
            throw new Exception("Не найдено скрипта sql");

        if (resourceNames.Count() > 1)
            throw new Exception("Найдено более одного скрипта sql");

        string resourceName = resourceNames.First();


        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        using StreamReader reader = new StreamReader(stream);

        string sql = reader.ReadToEnd();
        return sql;
    }

}
