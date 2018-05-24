using System.Collections.Generic;

namespace Precog.Core
{
    public interface IFileEnumerator
    {
        IEnumerable<string> EnumerateFiles(string path, string searchPattern);
    }
}
