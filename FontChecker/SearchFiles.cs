using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FontChecker
{
    public static class SearchFiles
    {
        public static IEnumerable<string> Search(string directory, string[] patterns)
        {
            // サブディレクトリの検索
            foreach (var filePath in Directory.EnumerateDirectories(directory).
                    SelectMany(dirPath => Search(dirPath, patterns)))
            {
                yield return filePath;
            }

            // ファイルの検索
            foreach (var filePath in patterns.SelectMany(pattern => 
                    Directory.GetFiles(directory, pattern)))
            {
                yield return filePath;
            }
        }
    }
}
