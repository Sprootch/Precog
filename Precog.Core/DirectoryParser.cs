using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Precog.Core
{
    public class DirectoryParser
    {
        public DirectoryParser()
        {
            Excludes = new List<string>();
        }

        public List<string> Excludes { get; set; }

        public IEnumerable<string> Parse(string path, string searchPattern)
        {
            var result = new List<string>();
            foreach (var file in Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories))
            {
                if (!Excludes.Any(e => file.ToUpperInvariant().Contains(e.ToUpperInvariant())))
                {
                    result.Add(file);
                }
            }
            return result;
        }
    }
}
