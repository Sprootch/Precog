using System.Collections.Generic;
using System.Linq;

namespace Precog.Core
{
    public class DirectoryParser
    {
        private readonly IFileEnumerator fileEnumerator;

        public DirectoryParser(IFileEnumerator fileEnumerator)
        {
            this.fileEnumerator = fileEnumerator;
            Excludes = new List<string>();
        }

        public List<string> Excludes { get; set; }

        public IEnumerable<string> Parse(string path, string searchPattern)
        {
            var result = new List<string>();
            foreach (var file in fileEnumerator.EnumerateFiles(path, searchPattern))
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
