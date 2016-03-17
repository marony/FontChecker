using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace FontChecker
{
    class Program
    {
        private static string[] patterns =
        {
            // フォントの拡張子
            "*.ttf", "*.otf", "*.ttc"
        };

        static string text = "abcdefghijklmnopqrstuvwxyz\n" + 
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ\n" +
            "あいうえおかきくけこさしすせそ\n" +
            "アイウエオカキクケコサシスセソ\n" +
            "魑魅魍魎四捨五入波阿弥陀仏般若心境";

        static void Main(string[] args)
        {
            // フォントを検索するディレクトリ
            var directory = @"C:\Windows\Fonts"; //Directory.GetCurrentDirectory();
            if (args.Length > 0)
                directory = args[0];
            // 出力ファイル(PDF)名
            var outputFileName = "test.pdf";
            if (args.Length > 1)
                outputFileName = args[1];
            // 標準入力
            if (Console.IsInputRedirected)
            {
                var sb = new StringBuilder();
                foreach (var s in TextLineReader.ReadLines(Console.In))
                    sb.AppendLine(s);
                text = sb.ToString();
            }
            // フォントを検索
            var fonts = SearchFiles.Search(directory, patterns).ToList();
            // PDF出力
            using (var pdf = new PdfDocument(outputFileName))
                pdf.Process(fonts, text);
        }
    }
}
