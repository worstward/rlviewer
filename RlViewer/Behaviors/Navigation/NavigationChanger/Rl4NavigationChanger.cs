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
            _header = ((Headers.Concrete.Rl4.Rl4Header)fileToChange.Header).HeaderStruct;
        }

        private Headers.Concrete.Rl4.Rl4RliFileHeader _header;


        protected override byte[] ConvertToDestStrHeaderBytes(Headers.Abstract.IStrHeader sourceHeader)
        {
            switch (SourceFile.Properties.Type)
            {
                case FileType.k:
                    return StructIO.WriteStruct<Headers.Concrete.Rl4.Rl4StrHeaderStruct>(((Headers.Concrete.K.KStrHeaderStruct)sourceHeader).ToRl4StrHeader());
                case FileType.ba:
                    return StructIO.WriteStruct<Headers.Concrete.Rl4.Rl4StrHeaderStruct>(((Headers.Concrete.Ba.BaStrHeader)sourceHeader).ToRl4StrHeader());
                default:
                    throw new ArgumentException("SourceFile type");
            }
        }


        public override void ChangeFlightTime()
        {
            if (DestinationFile != null)
            {
                var rhgCreationTime = new FileInfo(SourceFile.Properties.FilePath).LastWriteTime;

                using (var rhgStream = File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var rlstream = File.Open(DestinationFile.Properties.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {

                        _header = _header.ChangeFlightTime(rhgCreationTime);
                        rlstream.Write(StructIO.WriteStruct<Headers.Concrete.Rl4.Rl4RliFileHeader>(_header), 0, DestinationFile.Header.FileHeaderLength);

                    }
                }
            }
        }


        protected override float OffsetStrings
        {
            get
            {
                return _header.rlParams.dy;
            }
        }

        private int _azimuthCompressionCoef = 0;
        protected override int AzimuthCompressionCoef
        {
            get
            {
                if (_azimuthCompressionCoef != 0)
                {
                    return _azimuthCompressionCoef;
                }
                else
                {
                    var currentAzimuthDecompositionStep = _header.rlParams.dy;
                    var initialAzimuthDecompositionStep = _header.rhgParams.dy == 0 ? _header.synthParams.VH : _header.rhgParams.dy;

                    _azimuthCompressionCoef = (int)Math.Ceiling(currentAzimuthDecompositionStep / initialAzimuthDecompositionStep);

                    return _azimuthCompressionCoef;
                }
            }
        }


    }
}
