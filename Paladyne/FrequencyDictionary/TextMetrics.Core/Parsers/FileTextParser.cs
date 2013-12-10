using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextMetrics.Core.Parsers
{
    public class FileTextParser: ITextParser
    {
        private string myInputFile;
        private Encoding myEncoding;

        public FileTextParser(string filename,Encoding encoding)
        {
            this.myInputFile = filename;
            this.myEncoding = encoding;
            if (!File.Exists(myInputFile))
                throw new System.IO.FileNotFoundException("Input file not found", myInputFile);
            VerifyFile();
        }

        public IEnumerable<string> Parse()
        {
            
            using(var fStream = new StreamReader(myInputFile,myEncoding))
            {
                string line = null;
                while ((line = fStream.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        private void VerifyFile()
        {
            var buffer = new char[10240];
            string sampleContent;

            using (var sr = new StreamReader(myInputFile,true))
            {
                sr.Peek();
                if ((sr.CurrentEncoding != Encoding.UTF8 && sr.CurrentEncoding.IsSingleByte != myEncoding.IsSingleByte) ||
                    !sr.CurrentEncoding.IsSingleByte && myEncoding.WebName == sr.CurrentEncoding.WebName)
                {
                    throw new ApplicationException("Wrong input file encoding");
                }
                int length = sr.Read(buffer, 0, buffer.Length);
                sampleContent = new string(buffer, 0, length);
            }

            //Look for 4 consecutive binary zeroes, if they are exists file is supposed to be binary
            //or it is length >1000 but does not contain any endline symbol
            if (sampleContent.Contains("\0\0\0\0") ||
                (sampleContent.Length>500 && !sampleContent.Contains(Environment.NewLine)))
                throw new ApplicationException("Wrong input file, looks like it is binary.");
        }
    }
}
