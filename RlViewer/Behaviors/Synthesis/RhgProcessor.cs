using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Behaviors.Synthesis
{
    public class RhgProcessor<T> where T : Headers.Abstract.IStrHeader, new()
    {

        public RhgProcessor(Files.Rhg.Abstract.RhgFile[] rhgSeries, int hologramShortStringSize, int blockAzimuthSize, int memoryParts)
        {
            _rhgSeries = rhgSeries;
            _hologramShortStringSize = hologramShortStringSize;
            _emptyString = new byte[hologramShortStringSize * 2];
            _blockAzimuthSize = blockAzimuthSize;
        }

        private int _hologramShortStringSize;
        private int _blockAzimuthSize;
        private byte[] _emptyString;
        private Files.Rhg.Abstract.RhgFile[] _rhgSeries;

        private int GetCurrentRhgIndex(long readerPosition)
        {
            var fileSizes = _rhgSeries.Select(x => x.Properties.Length).ToArray();
            var headerLength = _rhgSeries[0].Header.FileHeaderLength;
            int index = 0;

            readerPosition += headerLength * _rhgSeries.Length;

            for (int i = 0; i < _rhgSeries.Length; i++)
            {
                readerPosition = readerPosition - fileSizes[i];
                index = i;

                if (readerPosition < 0)
                {
                    break;
                }
            }

            return index;
        }

        private long GetCurrentRhgPosition(long readerPosition, int currentIndex)
        {
            var fileSizes = _rhgSeries.Where((x, i) => i < currentIndex).Sum(x => x.Properties.Length);
            var headerLength = _rhgSeries[0].Header.FileHeaderLength;

            var position = (readerPosition + headerLength * (currentIndex + 1)) - fileSizes;
            return position;
        }


        public T[] ReadRhgToStream(long blockReaderPosition, Stream hologramWriter, int skippedLines = 0)
        {

            var currentRhgIndex = GetCurrentRhgIndex(blockReaderPosition);
            var currentRhgPosition = GetCurrentRhgPosition(blockReaderPosition, currentRhgIndex);
            var currentRhg = _rhgSeries[currentRhgIndex];


            var hologramReader = File.OpenRead(currentRhg.Properties.FilePath);

            hologramReader.Position = currentRhgPosition;
            hologramWriter.Position = 0;
            T[] headers = new T[_blockAzimuthSize];


            //for (int i = 0; i < (_matrixExtensionCoef - 1) / 2 * _blockAzimuthSize; i++)
            //{
            //    hologramWriter.Write(_emptyString, 0, _emptyString.Length);
            //}

            for (int i = 0; i < skippedLines; i++)
            {
                hologramWriter.Write(_emptyString, 0, _emptyString.Length);
                headers[i] = new T();
            }

            int j = 0;
            for (j = 0; j < _blockAzimuthSize - skippedLines; j++)
            {
                if (hologramReader.Position >= hologramReader.Length)
                {
                    if ((currentRhgIndex + 1) < _rhgSeries.Length)
                    {
                        currentRhgIndex++;
                        hologramReader = File.OpenRead(_rhgSeries[currentRhgIndex].Properties.FilePath);
                        hologramReader.Seek(_rhgSeries[currentRhgIndex].Header.FileHeaderLength, SeekOrigin.Begin);
                    }
                    else
                    {
                        break;
                    }
                }

                var naviHeader = Converters.StructIO.ReadStruct<T>(hologramReader);
                headers[j + skippedLines] = naviHeader;
                var hologramStringToPass = GetFloatRhgSamples(hologramReader);
                hologramWriter.Write(hologramStringToPass, 0, hologramStringToPass.Length);
            }


            if (hologramReader.Position >= hologramReader.Length)
            {
                for (; j < _blockAzimuthSize - skippedLines; j++)
                {
                    hologramWriter.Write(_emptyString, 0, _emptyString.Length);
                }
            }


            //// fill borders with 0
            //for (int i = 0; i < (_matrixExtensionCoef - 1) / 2 * _blockAzimuthSize; i++)
            //{
            //    hologramWriter.Write(_emptyString, 0, _emptyString.Length);
            //}

            return headers;
        }


        private byte[] GetFloatRhgSamples(Stream hologramReader)
        {
            var hologramShortBuffer = new byte[_hologramShortStringSize];
            var hologramFloatBuffer = new byte[_hologramShortStringSize * 2];

            var hologramSampleStringSize = _hologramShortStringSize / sizeof(short);

            var floats = new float[hologramSampleStringSize];
            var shorts = new short[hologramSampleStringSize];

            hologramReader.Read(hologramShortBuffer, 0, hologramShortBuffer.Length);

            Buffer.BlockCopy(hologramShortBuffer, 0, shorts, 0, hologramShortBuffer.Length);

            for (int j = 0; j < hologramSampleStringSize; j++)
            {
                floats[j] = shorts[j];
            }
            Buffer.BlockCopy(floats, 0, hologramFloatBuffer, 0, hologramFloatBuffer.Length);

            return hologramFloatBuffer;
        }
    }
}
