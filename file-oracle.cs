using System;
using System.IO;
using System.Reflection;
using CitizenFX.Core;

namespace MyResource.Client
{
    public class FileOraclePoc : BaseScript
    {
        public FileOraclePoc()
        {
            string path = @"C:\test.txt";
            bool exists;

            try
            {
                Assembly.LoadFrom(path);
                exists = true;
            }
            catch (FileNotFoundException)
            {
                exists = false;
            }
            catch (BadImageFormatException)
            {
                exists = true;
            }
            catch (FileLoadException)
            {
                exists = true;
            }

            Debug.WriteLine(string.Format("[kinobau] {0} exists: {1}", path, exists));
        }
    }
}
