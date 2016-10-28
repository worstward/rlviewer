using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Behaviors.Converters;

namespace RlViewer.Behaviors.Navigation.NavigationChanger
{
    public class Brl4NavigationChanger : NavigationChanger.Abstract.NavigationChanger
    {
        public Brl4NavigationChanger(Files.LocatorFile fileToChange, Files.LocatorFile sourceFile)
            : base(fileToChange, sourceFile)
        {
            _brl4File = (Files.Rli.Concrete.Brl4)fileToChange;
            _header = ((Headers.Concrete.Brl4.Brl4Header)fileToChange.Header).HeaderStruct;
        }

        private Files.Rli.Concrete.Brl4 _brl4File;
        private Headers.Concrete.Brl4.Brl4RliFileHeader _header;

        /// <summary>
        /// Checks if RLI has .ba RHG as its source
        /// </summary>
        /// <returns></returns>
        public bool CheckIsBaRhg()
        {
            if(_brl4File == null)
            {
                return false;
            }

            var rhgName = _brl4File.Header.HeaderInfo.Where(x => x.HeaderName == "РГГ")
                .FirstOrDefault().Params.Where(x => x.Item1 == "Имя файла").FirstOrDefault().Item2;

            return Path.GetExtension(rhgName) == ".ba";
        }


        protected Headers.Concrete.Brl4.Brl4StrHeaderStruct ConvertToStrHeader(Headers.Abstract.IStrHeader sourceHeader)
        {
            switch (SourceFile.Properties.Type)
            {
                case FileType.k:
                    return ((Headers.Concrete.K.KStrHeaderStruct)sourceHeader).ToBrl4StrHeader();
                case FileType.ba:
                    return ((Headers.Concrete.Ba.BaStrHeader)sourceHeader).ToBrl4StrHeader();
                default:
                    throw new ArgumentException("SourceFile type");
            }
        }

        /// <summary>
        /// Changes selected brl4 rli navigation based on source rhg file
        /// </summary>
        /// <param name="baFilename">Path to the source rhg</param>
        public override void ChangeNavigation()
        {   
            if (_brl4File != null)
            {
                var strDataLength = SourceFile.Width * SourceFile.Header.BytesPerSample;

                var baCreationTime = new FileInfo(SourceFile.Properties.FilePath).LastWriteTime;

                using (var sourceReader = File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var brl4Changer = File.Open(_brl4File.Properties.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var brl4Head = Converters.StructIO.ReadStruct<Headers.Concrete.Brl4.Brl4RliFileHeader>(brl4Changer);
                        var currentAzimuthDecompositionStep = brl4Head.rlParams.dy;
                        var initialAzimuthDecompositionStep = brl4Head.rhgParams.dy == 0 ? brl4Head.synthParams.VH : brl4Head.rhgParams.dy;

                        var rhgLinesInRliLine = (int)Math.Ceiling(currentAzimuthDecompositionStep / initialAzimuthDecompositionStep);


                        brl4Head = brl4Head.ChangeFlightTime(baCreationTime);

                        brl4Changer.Position = 0;
                        brl4Changer.Write(StructIO.WriteStruct<Headers.Concrete.Brl4.Brl4RliFileHeader>(brl4Head), 0, _brl4File.Header.FileHeaderLength);


                        sourceReader.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);
                        for (int i = 0; i < _header.rlParams.sy; i++)
                        {
                            sourceReader.Seek((SourceFile.Header.StrHeaderLength + strDataLength) * rhgLinesInRliLine, SeekOrigin.Current);
                        }

                        for (int i = 0; i < _brl4File.Height; i++)
                        {
                            var sourceStrHeader = GetSourceNavigationHeader(sourceReader);
                            var brl4StrHeader = ConvertToStrHeader(sourceStrHeader);
                            sourceReader.Seek(strDataLength, SeekOrigin.Current);

                            sourceReader.Seek((SourceFile.Header.StrHeaderLength + strDataLength) * rhgLinesInRliLine, SeekOrigin.Current); 

                            var brl4HeaderBytes = StructIO.WriteStruct<Headers.Concrete.Brl4.Brl4StrHeaderStruct>(brl4StrHeader);
                            brl4Changer.Write(brl4HeaderBytes, 0, brl4HeaderBytes.Length);
                            brl4Changer.Seek(_brl4File.Width * _brl4File.Header.BytesPerSample, SeekOrigin.Current);
                        }                
                    }
                }


            }
        }


    }
}
