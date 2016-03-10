using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Headers.Concrete.Brl4;

namespace RlViewer.Behaviors.Converters
{
    public static class Converters
    {
        public static RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader ToBrl4
            (this Rl4RliFileHeader rl4RliFileHeader, byte calibration, byte polarization, float angle_zond)
        {
            return new RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader(rl4RliFileHeader.fileSign, rl4RliFileHeader.fileVersion,
                rl4RliFileHeader.rhgParams.ToBrl4RhgSubHeader(),
                rl4RliFileHeader.rlParams.ToBrl4RliSubHeader(calibration),
                rl4RliFileHeader.synthParams.ToBrl4SynthSubHeader(polarization, angle_zond),
                rl4RliFileHeader.reserved);
        }


        public static RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader ToRl4(this Brl4RliFileHeader brl4RliFileHeader)
        {
            return new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader(brl4RliFileHeader.fileSign, brl4RliFileHeader.fileVersion,
                brl4RliFileHeader.rhgParams.ToRhgSubHeader<Rl4RhgSubHeaderStruct>(),
                brl4RliFileHeader.rlParams.ToRliSubHeader<Rl4RliSubHeaderStruct>(),
                brl4RliFileHeader.synthParams.ToSynthSubHeader<Rl4SynthesisSubHeaderStruct>(),
                brl4RliFileHeader.reserved);
        }


        #region FromRl4ToBrl4Helpers

        public static byte[] ToBrl4StrHeader(byte[] rl4StrHeader, double LatSns, double LonSns, double Hsns)
        {
            //16 это systime
            var offset = sizeof(bool) + 16;
            Buffer.BlockCopy(BitConverter.GetBytes(LatSns), 0, rl4StrHeader, offset, sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(LonSns), 0, rl4StrHeader, offset + sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Hsns),   0, rl4StrHeader, offset + sizeof(double) * 2, sizeof(double));

            return rl4StrHeader;
        }


        private static RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct ToBrl4StrHeader(this Rl4StrHeaderStruct rl4StrHeader, double LatSns, double LongSns, double Hsns)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Rl4StrHeaderStruct>(rl4StrHeader);

            var offset = sizeof(bool) + rl4StrHeader.time.Length;
            Buffer.BlockCopy(BitConverter.GetBytes(LatSns) , 0, headerStructArr, offset,  sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(LongSns), 0, headerStructArr, offset + sizeof(double),     sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Hsns)   , 0, headerStructArr, offset + sizeof(double) * 2, sizeof(double));

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct>(ms);
            }
        }


        private static RlViewer.Headers.Concrete.Brl4.Brl4SynthesisSubHeaderStruct ToBrl4SynthSubHeader
            (this Rl4SynthesisSubHeaderStruct rl4SynthSubHeader, byte polarization, float angle_zond)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Rl4SynthesisSubHeaderStruct>(rl4SynthSubHeader);

            //compute offset and place new values to header byte array
            var offset = headerStructArr.Length - rl4SynthSubHeader.reserved5.Length 
                         - rl4SynthSubHeader.rhgName.Length - rl4SynthSubHeader.reserved6.Length;

            headerStructArr[offset] = polarization;

            Buffer.BlockCopy(BitConverter.GetBytes(angle_zond), 0, headerStructArr, offset + 1, sizeof(float));

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4SynthesisSubHeaderStruct>(ms);
            }

        }

        private static RlViewer.Headers.Concrete.Brl4.Brl4RhgSubHeaderStruct ToBrl4RhgSubHeader(this Rl4RhgSubHeaderStruct rl4SynthSubHeader)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Rl4RhgSubHeaderStruct>(rl4SynthSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4RhgSubHeaderStruct>(ms);
            }
        }

        private static RlViewer.Headers.Concrete.Brl4.Brl4RliSubHeaderStruct ToBrl4RliSubHeader(this Rl4RliSubHeaderStruct rl4RliSubHeader, byte calibration)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Rl4RliSubHeaderStruct>(rl4RliSubHeader);
            //compute offset and place new values to header byte array
            var offset = headerStructArr.Length -
                (rl4RliSubHeader.reserved4.Length + rl4RliSubHeader.fileName.Length + rl4RliSubHeader.reserved3.Length);

            headerStructArr[offset] = calibration;
            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliSubHeaderStruct>(ms);
            }
        }

#endregion


        #region FromBrl4Helpers


        private static T ToRliSubHeader<T>(this Brl4RliSubHeaderStruct brl4RliSubHeader)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Brl4RliSubHeaderStruct>(brl4RliSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<T>(ms);
            }
        }

        private static T ToRhgSubHeader<T>(this Brl4RhgSubHeaderStruct brl4RliSubHeader)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Brl4RhgSubHeaderStruct>(brl4RliSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<T>(ms);
            }
        }

        private static T ToSynthSubHeader<T>(this Brl4SynthesisSubHeaderStruct brl4RliSubHeader)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Brl4SynthesisSubHeaderStruct>(brl4RliSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<T>(ms);
            }
        }



        public static byte[] ToRl4StrHeader(byte[] brl4StrHeader, double Vx, double Vy, double Vz)
        {
            //16 это systime
            var offset = sizeof(bool) + 16;
            Buffer.BlockCopy(BitConverter.GetBytes(Vx), 0, brl4StrHeader, offset, sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Vy), 0, brl4StrHeader, offset + sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Vz), 0, brl4StrHeader, offset + sizeof(double) * 2, sizeof(double));

            return brl4StrHeader;
        }


        public static RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct ToRl4StrHeader(this Brl4StrHeaderStruct brl4StrHeader, double Vx, double Vy, double Vz)
        {
            byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Brl4StrHeaderStruct>(brl4StrHeader);

            var offset = sizeof(bool) + brl4StrHeader.time.Length;
            Buffer.BlockCopy(BitConverter.GetBytes(Vx), 0, headerStructArr, offset, sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Vy), 0, headerStructArr, offset + sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Vz), 0, headerStructArr, offset + sizeof(double) * 2, sizeof(double));

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Files.LocatorFile.ReadStruct<RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct>(ms);
            }
        }


        #endregion
    }
}
