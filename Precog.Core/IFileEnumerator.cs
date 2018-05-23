using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Precog.Core
{
    public interface IFileEnumerator
    {
        IEnumerable<string> EnumerateFiles(string path, string searchPattern);
    }
}
