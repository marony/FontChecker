using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace FontChecker
{
    public class PdfDocument : IDisposable
    {
        private string _outputFileName;
        private Document _doc;
        private FileStream _stream;

        private Font _defaultFont;
        private Font _redFont;

        public PdfDocument(string outputFileName)
        {
            // 出力ファイル名
            _outputFileName = outputFileName;

            // システムフォントを使用できるように登録
            FontFactory.RegisterDirectory(Environment.SystemDirectory.Replace("system32", "fonts"));

            // A4横でドキュメントを作成
            _doc = new Document(PageSize.A4.Rotate());
            _stream = new FileStream(_outputFileName, FileMode.Create);
            //ファイルの出力先を設定
            var pw = PdfWriter.GetInstance(_doc, _stream);

            // フォントセットアップ
            _defaultFont = FontFactory.GetFont("MS-Gothic", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL);
            _redFont = FontFactory.GetFont("MS-Gothic", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 9, Font.NORMAL);
            _redFont.SetColor(255, 0, 0);

            //ドキュメントを開く
            _doc.Open();
        }

        public void Process(List<string> fonts, string text)
        {
            _doc.Add(new Paragraph("フォントの個数:" + fonts.Count, _defaultFont));

            // 描画
            var line = new string('-', 80);
            var n = 1;
            foreach (var fontName in fonts)
            {
                Console.WriteLine($"{n}/{fonts.Count} : {Path.GetFileName(fontName)}");
                // ファイル名を出力
                _doc.Add(new Paragraph(line, _defaultFont));
                _doc.Add(new Paragraph(fontName, _defaultFont));
                // フォントを出力
                try
                {
                    if (IsFontCollection(fontName))
                    {
                        // フォントコレクション
                        var fontNames = BaseFont.EnumerateTTCNames(fontName);
                        for (var i = 0; i < fontNames.Length; ++i)
                            DrawText(fontName, text, i);
                    }
                    else // 単体のフォント
                        DrawText(fontName, text);
                }
                catch (Exception ex)
                {
                    string message = $"{fontName} : {ex.Message}";
                    Console.WriteLine(message);
                    _doc.Add(new Paragraph(message));
                }
                ++n;
            }
        }

        private bool IsFontCollection(string fontName)
        {
            return fontName.ToLower().EndsWith(".ttc");
        }

        private void DrawText(string fontName, string text, int i = -1)
        {
            BaseFont bf = null;
            if (i < 0)
                bf = BaseFont.CreateFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            else
                bf = BaseFont.CreateFont($"{fontName},{i}", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            // FIXME: フォントサイズの指定ができるように
            var font = new Font(bf, 12);
            // フォント名
            string s = string.Empty;
            foreach (var names in /* bf.FullFontName */ bf.FamilyFontName)
                s += names.Last() + ", ";

            _doc.Add(new Paragraph(s, _redFont));
            // サンプル文書
            _doc.Add(new Paragraph(text, font));
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _doc.Close();
                    _doc = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
