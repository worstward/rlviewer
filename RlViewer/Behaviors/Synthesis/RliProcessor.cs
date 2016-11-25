using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Behaviors.Synthesis
{
    class RliProcessor<T> where T : Headers.Abstract.IStrHeader
    {
        public RliProcessor(string rliFileName, int totalLines)
        {
            _rliFileName = rliFileName;
            _totalLines = totalLines;
        }

        private string _rliFileName;
        private int _totalLines;
       
        public int ProcessRli(Stream rliStream, int rliWidth, int rliHeight, T[] navigationHeaders)
        {
            int processedLines = 0;
            var rliArr = new byte[(long)(rliWidth * sizeof(float))];
            using (var rliWriterStream = File.Open(_rliFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                for (int rliHeightIterator = 0; rliHeightIterator < rliHeight && _totalLines > 0 ; rliHeightIterator++)
                {
                    rliStream.Read(rliArr, 0, rliArr.Length);
                    var headerBytes = Converters.StructIO.WriteStruct<T>(navigationHeaders[rliHeightIterator]);
                    rliWriterStream.Write(headerBytes, 0, headerBytes.Length);
                    rliWriterStream.Write(rliArr, 0, rliArr.Length);
                    processedLines++;
                    _totalLines--;
                }
            }
            return processedLines;
        }
    }
}
