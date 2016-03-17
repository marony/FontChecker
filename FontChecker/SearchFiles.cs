using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontChecker
{
    public static class SearchFiles
    {
        public static IEnumerable<string> Search(string directory, string[] patterns)
        {
            // FIXME: 検索しながらyieldする
            var files = new List<string>();
            InternalSearch(directory, patterns, files);
            foreach (var file in files)
                yield return file;
        }

        private static void InternalSearch(string directory, string[] patterns, List<string> files)
        {
            // サブディレクトリの検索
            foreach (var path in Directory.EnumerateDirectories(directory))
                InternalSearch(path, patterns, files);
            // ファイルの検索
            foreach (var pattern in patterns)
            {
                foreach (var path in Directory.GetFiles(directory, pattern))
                    files.Add(path);
            }
        }
    }
}
