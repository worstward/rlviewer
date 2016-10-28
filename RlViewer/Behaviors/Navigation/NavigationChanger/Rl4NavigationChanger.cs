using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Behaviors.Converters;


namespace RlViewer.Behaviors.Navigation.NavigationChanger
{
    class Rl4NavigationChanger : NavigationChanger.Abstract.NavigationChanger
    {
        public Rl4NavigationChanger(Files.LocatorFile fileToChange, Files.LocatorFile sourceFile)
            : base(fileToChange, sourceFile)
        {
            _rl4File = (Files.Rli.Concrete.Rl4)fileToChange;
            _header = ((Headers.Concrete.Rl4.Rl4Header)fileToChange.Header).HeaderStruct;
        }

        private Files.Rli.Concrete.Rl4 _rl4File;
        private Headers.Concrete.Rl4.Rl4RliFileHeader _header;



        protected Headers.Concrete.Rl4.Rl4StrHeaderStruct ConvertToStrHeader(Headers.Abstract.IStrHeader sourceHeader)
        {
            switch (SourceFile.Properties.Type)
            {
                case FileType.k:
                    return ((Headers.Concrete.K.KStrHeaderStruct)sourceHeader).ToRl4StrHeader();
                case FileType.ba:
                    return ((Headers.Concrete.Ba.BaStrHeader)sourceHeader).ToRl4StrHeader();
                default:
                    throw new ArgumentException("SourceFile type");
            }
        }



        /// <summary>
        /// Changes selected rl4 rli navigation based on source rhg file
        /// </summary>
        /// <param name="baFilename">Path to the source rhg</param>
        public override void ChangeNavigation()
        {   
            if (_rl4File != null)
            {
                var strDataLength = SourceFile.Width * SourceFile.Header.BytesPerSample;

                var sourceCreationTime = new FileInfo(SourceFile.Properties.FilePath).LastWriteTime;

                using (var sourceReader = File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var rl4Changer = File.Open(_rl4File.Properties.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var rl4Head = Converters.StructIO.ReadStruct<Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Changer);
                        var currentAzimuthDecompositionStep = rl4Head.rlParams.dy;
                        var initialAzimuthDecompositionStep = rl4Head.rhgParams.dy == 0 ? rl4Head.synthParams.VH : rl4Head.rhgParams.dy;

                        var rhgLinesInRliLine = (int)Math.Ceiling(currentAzimuthDecompositionStep / initialAzimuthDecompositionStep);

                        rl4Head = rl4Head.ChangeFlightTime(sourceCreationTime);

                        rl4Changer.Position = 0;
                        rl4Changer.Write(StructIO.WriteStruct<Headers.Concrete.Rl4.Rl4RliFileHeader>(rl4Head), 0, _rl4File.Header.FileHeaderLength);


                        sourceReader.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);
                        for (int i = 0; i < _header.rlParams.sy; i++)
                        {
                            sourceReader.Seek((SourceFile.Header.StrHeaderLength + strDataLength) * rhgLinesInRliLine, SeekOrigin.Current);
                        }

                        for (int i = 0; i < _rl4File.Height; i++)
                        {
                            var sourceStrHeader = GetSourceNavigationHeader(sourceReader);

                            var rl4StrHeader = ConvertToStrHeader(sourceStrHeader);
                            sourceReader.Seek(strDataLength, SeekOrigin.Current);

                            sourceReader.Seek((SourceFile.Header.StrHeaderLength + strDataLength) * rhgLinesInRliLine, SeekOrigin.Current); 

                            var rl4HeaderBytes = StructIO.WriteStruct<Headers.Concrete.Rl4.Rl4StrHeaderStruct>(rl4StrHeader);
                            rl4Changer.Write(rl4HeaderBytes, 0, rl4HeaderBytes.Length);
                            rl4Changer.Seek(_rl4File.Width * _rl4File.Header.BytesPerSample, SeekOrigin.Current);
                        }                
                    }
                }


            }
        }
    }
}
