using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Headers.Concrete.Brl4;
using RlViewer.Files;

namespace RlViewer.Behaviors.Converters
{
    public static class FileHeaderConverters
    {

        public static Rl4RliSubHeaderStruct ChangeFragmentShift(this Rl4RliSubHeaderStruct head, int shiftX, int shiftY)
        {
            var header = head;
            header.sx += shiftX;
            header.sy += shiftY;
            return header;
        }

        public static Brl4RliSubHeaderStruct ChangeFragmentShift(this Brl4RliSubHeaderStruct head, int shiftX, int shiftY)
        {
            var header = head;
            header.sx += shiftX;
            header.sy += shiftY;
            return header;
        }


        public static Rl4RliSubHeaderStruct ChangeImgDimensions(this Rl4RliSubHeaderStruct head, int width, int height)
        {
            var header = head;
            //header.cadrHeight = height;
            header.cadrWidth = width;
            header.height = height;
            header.width = width;

            return header;
        }

        public static Brl4RliSubHeaderStruct ChangeImgDimensions(this Brl4RliSubHeaderStruct head, int width, int height)
        {           
            var header = head;
            //header.cadrHeight = height;
            header.cadrWidth = width;
            header.height = height;
            header.width = width;

            return header;
        }

        public static RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct ToBrl4StrHeader(this RlViewer.Headers.Concrete.K.KStrHeaderStruct kStrHeader)
        {
            var brl4StrHead = new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct();

            brl4StrHead.Ve = kStrHeader.navigationHeader.speedLatIns;
            brl4StrHead.Vu = kStrHeader.navigationHeader.verticalSpeed;
            brl4StrHead.Vn = kStrHeader.navigationHeader.speedLongIns;
            brl4StrHead.V =  kStrHeader.navigationHeader.speedIns;
            brl4StrHead.latitude = kStrHeader.navigationHeader.latitudeIns;
            brl4StrHead.longtitude = kStrHeader.navigationHeader.longtitudeIns;
            brl4StrHead.H = kStrHeader.navigationHeader.heightInsSns;
            brl4StrHead.f = kStrHeader.navigationHeader.tangageAngleIns;
            brl4StrHead.g = kStrHeader.navigationHeader.tiltAngleIns;
            brl4StrHead.a = kStrHeader.navigationHeader.kursAngle;

            return brl4StrHead;
        }

        public static RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct ToRl4StrHeader(this RlViewer.Headers.Concrete.K.KStrHeaderStruct kStrHeader)
        {
            var rl4StrHead = new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct();

            rl4StrHead.Ve = kStrHeader.navigationHeader.speedLatIns;
            rl4StrHead.Vu = kStrHeader.navigationHeader.verticalSpeed;
            rl4StrHead.Vn = kStrHeader.navigationHeader.speedLongIns;
            rl4StrHead.V = kStrHeader.navigationHeader.speedIns;
            rl4StrHead.latitude = kStrHeader.navigationHeader.latitudeIns;
            rl4StrHead.longtitude = kStrHeader.navigationHeader.longtitudeIns;
            rl4StrHead.H = kStrHeader.navigationHeader.heightInsSns;
            rl4StrHead.f = kStrHeader.navigationHeader.tangageAngleIns;
            rl4StrHead.g = kStrHeader.navigationHeader.tiltAngleIns;
            rl4StrHead.a = kStrHeader.navigationHeader.kursAngle;

            return rl4StrHead;
        }




        public static RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct ToBrl4StrHeader(this RlViewer.Headers.Concrete.Ba.BaStrHeader baStrHeader)
        {
            var brl4StrHead = new RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct();

            brl4StrHead.Ve = baStrHeader.Ve;
            brl4StrHead.Vu = baStrHeader.Vh;
            brl4StrHead.Vn = baStrHeader.Vn;
            brl4StrHead.V = baStrHeader.V;
            brl4StrHead.latitude = baStrHeader.latitude;
            brl4StrHead.longtitude = baStrHeader.longtitude;
            brl4StrHead.H = baStrHeader.H;
            brl4StrHead.f = baStrHeader.pitch;
            brl4StrHead.g = baStrHeader.roll;
            brl4StrHead.a = baStrHeader.heading;

            return brl4StrHead;
        }

        public static RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct ToRl4StrHeader(this RlViewer.Headers.Concrete.Ba.BaStrHeader baStrHeader)
        {
            var rl4StrHead = new RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct();

            rl4StrHead.Ve = baStrHeader.Ve;
            rl4StrHead.Vu = baStrHeader.Vh;
            rl4StrHead.Vn = baStrHeader.Vn;
            rl4StrHead.V = baStrHeader.V;
            rl4StrHead.latitude = baStrHeader.latitude;
            rl4StrHead.longtitude = baStrHeader.longtitude;
            rl4StrHead.H = baStrHeader.H;
            rl4StrHead.f = baStrHeader.pitch;
            rl4StrHead.g = baStrHeader.roll;
            rl4StrHead.a = baStrHeader.heading;

            return rl4StrHead;
        }




        public static Headers.Concrete.Brl4.Brl4RliFileHeader ChangeFlightTime(this RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader brl4Header, DateTime newTime)
        {
            brl4Header.rhgParams.fileTime = newTime.ToSystime();
            return brl4Header;
        }

        public static Headers.Concrete.Rl4.Rl4RliFileHeader ChangeFlightTime(this RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader rl4Header, DateTime newTime)
        {
            rl4Header.rhgParams.fileTime = newTime.ToSystime();
            return rl4Header;
        }
        //public static RlViewer.Headers.Concrete.Ba.BaStrHeader ToBa(this RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct rl4StrHeader)
        //{
        //    var ba = new Headers.Concrete.Ba.BaStrHeader();
        //    ba.Ve         = (float)rl4StrHeader.Ve;
        //    ba.Vh         = (float)rl4StrHeader.Vu;
        //    ba.Vn         = (float)rl4StrHeader.Vn;
        //    ba.V          = (float)rl4StrHeader.V;
        //    ba.latitude   = (float)rl4StrHeader.latitude;
        //    ba.longtitude = (float)rl4StrHeader.longtitude;
        //    ba.H          = (float)rl4StrHeader.H;
        //    ba.pitch      = (float)rl4StrHeader.f;
        //    ba.roll       = (float)rl4StrHeader.g;
        //    ba.heading    = (float)rl4StrHeader.a;

        //    return ba;
        //}


        public static RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader ToBrl4
            (this Rl4RliFileHeader rl4RliFileHeader, byte calibration, byte polarization, float angle_zond)
        {

            //3 = aligning, range, azimuth coefs from brl4Header
            var bytesToSkip = sizeof(int) * 3;
            var reserved = rl4RliFileHeader.reserved.Skip(bytesToSkip).Take(rl4RliFileHeader.reserved.Count() - bytesToSkip).ToArray();

            return new RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader(rl4RliFileHeader.fileSign, rl4RliFileHeader.fileVersion,
                rl4RliFileHeader.rhgParams.ToBrl4RhgSubHeader(),
                rl4RliFileHeader.rlParams.ToBrl4RliSubHeader(calibration), 
                rl4RliFileHeader.synthParams.ToBrl4SynthSubHeader(polarization, angle_zond),
                0,
                0,
                0,
                reserved);
        }


        public static RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader ToRl4(this Brl4RliFileHeader brl4RliFileHeader)
        {
            var reserved = new List<byte>();
            reserved.AddRange(BitConverter.GetBytes(brl4RliFileHeader.aligningPointsCount));
            reserved.AddRange(BitConverter.GetBytes(brl4RliFileHeader.rangeCompressionCoef));
            reserved.AddRange(BitConverter.GetBytes(brl4RliFileHeader.azimuthCompressionCoef));
            reserved.AddRange(brl4RliFileHeader.reserved);


            return new RlViewer.Headers.Concrete.Rl4.Rl4RliFileHeader(brl4RliFileHeader.fileSign, brl4RliFileHeader.fileVersion,
                brl4RliFileHeader.rhgParams.ToRhgSubHeader<Rl4RhgSubHeaderStruct>(),
                brl4RliFileHeader.rlParams.ToRliSubHeader<Rl4RliSubHeaderStruct>(),
                brl4RliFileHeader.synthParams.ToSynthSubHeader<Rl4SynthesisSubHeaderStruct>(),
                reserved.ToArray());
        }


        #region FromRl4ToBrl4Helpers

        public static byte[] ToBrl4StrHeader(byte[] rl4StrHeader, double LatSns, double LonSns, double Hsns)
        {
            //16 это systime
            var offset = sizeof(bool) + 16;
            Buffer.BlockCopy(BitConverter.GetBytes(LatSns), 0, rl4StrHeader, offset, sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(LonSns), 0, rl4StrHeader, offset + sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Hsns), 0, rl4StrHeader, offset + sizeof(double) * 2, sizeof(double));

            return rl4StrHeader;
        }


        public static Brl4StrHeaderStruct ToBrl4StrHeader(Rl4StrHeaderStruct rl4StrHeader, double LatSns, double LonSns, double Hsns)
        {
            //16 это systime
            using (var ms = new System.IO.MemoryStream(RlViewer.Behaviors.Converters.StructIO.WriteStruct<RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct>(rl4StrHeader)))
            {
                var header = RlViewer.Behaviors.Converters.StructIO.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4StrHeaderStruct>(ms);
                header.LatSNS = LatSns;
                header.LongSNS = LonSns;
                header.Hsns = Hsns;
                return header;
            }
        }

        [Obsolete("Демка для питерцев")]
        public static byte[] ToBrl4StrHeader(byte[] rl4StrHeader)
        {
            //16 это массив systime
            var offset = sizeof(bool) + 16;
            Buffer.BlockCopy(rl4StrHeader, offset + sizeof(double) * 3, rl4StrHeader, offset, sizeof(double));
            Buffer.BlockCopy(rl4StrHeader, offset + sizeof(double) * 4, rl4StrHeader, offset + sizeof(double), sizeof(double));
            Buffer.BlockCopy(rl4StrHeader, offset + sizeof(double) * 5, rl4StrHeader, offset + sizeof(double) * 2, sizeof(double));

            return rl4StrHeader;
        }



        private static RlViewer.Headers.Concrete.Brl4.Brl4SynthesisSubHeaderStruct ToBrl4SynthSubHeader
            (this Rl4SynthesisSubHeaderStruct rl4SynthSubHeader, byte polarization, float angle_zond)
        {
            byte[] headerStructArr = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Rl4SynthesisSubHeaderStruct>(rl4SynthSubHeader);

            //compute offset and place new values to header byte array
            var offset = headerStructArr.Length - rl4SynthSubHeader.reserved5.Length 
                         - rl4SynthSubHeader.rhgName.Length - rl4SynthSubHeader.reserved6.Length;

            headerStructArr[offset] = polarization;

            Buffer.BlockCopy(BitConverter.GetBytes(angle_zond), 0, headerStructArr, offset + 1, sizeof(float));

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Behaviors.Converters.StructIO.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4SynthesisSubHeaderStruct>(ms);
            }

        }

        private static RlViewer.Headers.Concrete.Brl4.Brl4RhgSubHeaderStruct ToBrl4RhgSubHeader(this Rl4RhgSubHeaderStruct rl4SynthSubHeader)
        {
            byte[] headerStructArr = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Rl4RhgSubHeaderStruct>(rl4SynthSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Behaviors.Converters.StructIO.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4RhgSubHeaderStruct>(ms);
            }
        }

        private static RlViewer.Headers.Concrete.Brl4.Brl4RliSubHeaderStruct ToBrl4RliSubHeader(this Rl4RliSubHeaderStruct rl4RliSubHeader, byte calibration)
        {
            byte[] headerStructArr = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Rl4RliSubHeaderStruct>(rl4RliSubHeader);
            //compute offset and place new values to header byte array
            var offset = headerStructArr.Length -
                (rl4RliSubHeader.reserved4.Length + rl4RliSubHeader.fileName.Length + rl4RliSubHeader.reserved3.Length);

            headerStructArr[offset] = calibration;
            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Behaviors.Converters.StructIO.ReadStruct<RlViewer.Headers.Concrete.Brl4.Brl4RliSubHeaderStruct>(ms);
            }
        }

#endregion


        #region FromBrl4Helpers


        private static T ToRliSubHeader<T>(this Brl4RliSubHeaderStruct brl4RliSubHeader)
        {
            byte[] headerStructArr = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Brl4RliSubHeaderStruct>(brl4RliSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Behaviors.Converters.StructIO.ReadStruct<T>(ms);
            }
        }

        private static T ToRhgSubHeader<T>(this Brl4RhgSubHeaderStruct brl4RliSubHeader)
        {
            byte[] headerStructArr = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Brl4RhgSubHeaderStruct>(brl4RliSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Behaviors.Converters.StructIO.ReadStruct<T>(ms);
            }
        }

        private static T ToSynthSubHeader<T>(this Brl4SynthesisSubHeaderStruct brl4RliSubHeader)
        {
            byte[] headerStructArr = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Brl4SynthesisSubHeaderStruct>(brl4RliSubHeader);

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Behaviors.Converters.StructIO.ReadStruct<T>(ms);
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
            byte[] headerStructArr = RlViewer.Behaviors.Converters.StructIO.WriteStruct<Brl4StrHeaderStruct>(brl4StrHeader);

            var offset = sizeof(bool) + brl4StrHeader.time.Length;
            Buffer.BlockCopy(BitConverter.GetBytes(Vx), 0, headerStructArr, offset, sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Vy), 0, headerStructArr, offset + sizeof(double), sizeof(double));
            Buffer.BlockCopy(BitConverter.GetBytes(Vz), 0, headerStructArr, offset + sizeof(double) * 2, sizeof(double));

            using (var ms = new System.IO.MemoryStream(headerStructArr))
            {
                return RlViewer.Behaviors.Converters.StructIO.ReadStruct<RlViewer.Headers.Concrete.Rl4.Rl4StrHeaderStruct>(ms);
            }
        }


        #endregion
    }
}
