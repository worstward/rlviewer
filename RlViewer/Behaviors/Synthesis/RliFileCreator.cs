using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RlViewer.Behaviors.Synthesis
{
    class RliFileCreator
    {
        public RliFileCreator(Files.FileProperties properties)
        {
            _properties = properties;
        }

        private Files.FileProperties _properties;

        public Files.FileProperties Properties
        {
            get
            { 
                return _properties; 
            }
        }


        private Headers.Concrete.Rl4.Rl4RliSubHeaderStruct CreateRliSubHeader(ServerSarTaskParams sstp, int height = 0)
        {
            var rliFileSubHeader = new Headers.Concrete.Rl4.Rl4RliSubHeaderStruct();
            rliFileSubHeader.fileTime = new byte[16];
            rliFileSubHeader.fileName = new byte[256];
            rliFileSubHeader.synthTime = new byte[16];

            rliFileSubHeader.cadrWidth = (int)(sstp.Nshift / sstp.Nscale);
            rliFileSubHeader.cadrHeight = (int)(sstp.Mshift / sstp.Mshift);
            rliFileSubHeader.strSignalCount = (int)(sstp.Nshift / sstp.Nscale);
            rliFileSubHeader.width = (int)(sstp.Nshift / sstp.Nscale);
            rliFileSubHeader.height = height;
            rliFileSubHeader.dx = sstp.dR;

            return rliFileSubHeader;
        }

        private Headers.Concrete.Rl4.Rl4RhgSubHeaderStruct CreateRliFileRhgSubHeader(ServerSarTaskParams sstp)
        {
            var rliFileRhgSubHeader = new Headers.Concrete.Rl4.Rl4RhgSubHeaderStruct();
            rliFileRhgSubHeader.fileTime = new byte[16];
            rliFileRhgSubHeader.fileName = new byte[256];

            return rliFileRhgSubHeader;
        }

        private Headers.Concrete.Rl4.Rl4SynthesisSubHeaderStruct CreateRliSynthesisHeader(ServerSarTaskParams sstp)
        {
            var rliFileSynthesisHeader = new Headers.Concrete.Rl4.Rl4SynthesisSubHeaderStruct();
            rliFileSynthesisHeader.comments = new byte[512];
            rliFileSynthesisHeader.rhgName = new byte[128];

            rliFileSynthesisHeader.Fn = sstp.Fimpulses;
            rliFileSynthesisHeader.D0 = sstp.Rmin;
            rliFileSynthesisHeader.board = (byte)(sstp.Nav_LR_Side == -1 ? 0 : 1);
            rliFileSynthesisHeader.lambda = sstp.lambda;
            rliFileSynthesisHeader.dD = sstp.dR;

            return rliFileSynthesisHeader;
        }



        private Headers.Concrete.Rl4.Rl4RliFileHeader CreateRl4FileHeader(ServerSarTaskParams sstp, int height = 0)
        {
            var rliFileSubHeader = CreateRliSubHeader(sstp, height);
            var rliFileRhgHeader = CreateRliFileRhgSubHeader(sstp);
            var rliFileSynthHeader = CreateRliSynthesisHeader(sstp);


            var rl4Signature = new byte[] { 0x52, 0x4c, 0x49, 0x00 };
            int version = 1;
            var rliSubSize = System.Runtime.InteropServices.Marshal.SizeOf(rliFileSubHeader);
            var rhgSubSize = System.Runtime.InteropServices.Marshal.SizeOf(rliFileRhgHeader);
            var synthSubSize = System.Runtime.InteropServices.Marshal.SizeOf(rliFileSynthHeader);
            var reservedSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Headers.Concrete.Rl4.Rl4RliFileHeader))
                - rl4Signature.Length - sizeof(int) - rliSubSize - rhgSubSize - synthSubSize;


            var rliFileHeader = new Headers.Concrete.Rl4.Rl4RliFileHeader(rl4Signature,
                version, rliFileRhgHeader, rliFileSubHeader, rliFileSynthHeader, new byte[reservedSize]);

            return rliFileHeader;
        }

        public Files.LocatorFile CreateEmptyRli(ServerSarTaskParams sstp)
        {
            var headerStruct = CreateRl4FileHeader(sstp);
            var headerBytes = Behaviors.Converters.StructIO.WriteStruct<Headers.Concrete.Rl4.Rl4RliFileHeader>(headerStruct);

            using (var rliWriterStream = File.Open(_properties.FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                rliWriterStream.Write(headerBytes, 0, headerBytes.Length);
            }


            var header = Factories.Header.Abstract.HeaderFactory.GetFactory(_properties).Create(headerStruct);
            var rli = Factories.File.Abstract.FileFactory.GetFactory(_properties).Create(_properties, header, null);

            return rli;
        }

    }
}
