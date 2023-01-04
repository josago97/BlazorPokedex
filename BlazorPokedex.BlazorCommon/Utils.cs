namespace BlazorPokedex.BlazorCommon;

public static class Utils
{
    private static readonly string AssemblyName = typeof(Utils).Assembly.GetName().Name;

    public static string GetStaticFileUrl(string wwwrootPath)
    {
        return $"_content/{AssemblyName}/{wwwrootPath}";
    }
}
