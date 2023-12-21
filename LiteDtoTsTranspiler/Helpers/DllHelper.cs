namespace LiteDtoTsTranspiler.Helpers;

public static class DllHelper
{
   internal static (bool, string )FindDll(string dir, string dllName)
    {
        string[] files = Directory.GetFiles(dir);

        // check if the dll is among them
        foreach (string file in files)
        {
            if (Path.GetFileName(file) == dllName)
            {
                // Return the full path of the dll
                return (true, file);
            }
        }

        var subdirs = Directory.GetDirectories(dir);

        foreach (string subdir in subdirs)
        {
           var  ( found,  location)  = FindDll(subdir, dllName);

            if (location != null)
            {
                return (found, location);
            }
        }

        return (false, null)!;
    }
}