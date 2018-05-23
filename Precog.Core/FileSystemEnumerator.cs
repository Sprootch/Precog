using System.Collections.Generic;
using System.IO;

namespace Precog.Core
{
    public class FileSystemEnumerator : IFileEnumerator
    {
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories);
        }
    }
}
