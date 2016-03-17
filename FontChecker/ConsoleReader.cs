using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontChecker
{
    public static class TextLineReader
    {
        public static IEnumerable<string> ReadLines(TextReader tr)
        {
            var s = tr.ReadLine();
            while (s != null)
            {
                yield return s;
                s = tr.ReadLine();
            }
        }
    }
}
