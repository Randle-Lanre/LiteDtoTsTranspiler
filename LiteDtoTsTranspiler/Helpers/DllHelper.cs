namespace LiteDtoTsTranspiler.Helpers;

public static class DllHelper
{
   internal static string FindDll(string dir, string dllName)
    {
        string[] files = Directory.GetFiles(dir);

        // check if the dll is among them
        foreach (string file in files)
        {
            if (Path.GetFileName(file) == dllName)
            {
                // Return the full path of the dll
                return file;
            }
        }

        var subdirs = Directory.GetDirectories(dir);

        foreach (string subdir in subdirs)
        {
            string dllPath = FindDll(subdir, dllName);

            if (dllPath != null)
            {
                return dllPath;
            }
        }

        return null;
    }
}