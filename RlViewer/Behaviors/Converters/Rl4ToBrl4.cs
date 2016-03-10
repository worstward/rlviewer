using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Concrete.Rl4;
using RlViewer.Headers.Concrete.Brl4;

namespace RlViewer.Behaviors.Converters
{
    static class Rl4ToBrl4
    {
        public static RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader ToBrl4(this Rl4RliFileHeader rl4RliFileHeader)
        {
            return new RlViewer.Headers.Concrete.Brl4.Brl4RliFileHeader(rl4RliFileHeader.fileSign, rl4RliFileHeader.fileVersion,
                (RlViewer.Headers.Concrete.Brl4.Brl4RhgSubHeaderStruct)rl4RliFileHeader.rhgParams,
                (RlViewer.Headers.Concrete.Brl4.Brl4RliSubHeaderStruct)rl4RliFileHeader.rlParams,
                (RlViewer.Headers.Concrete.Brl4.Brl4SynthesisSubHeaderStruct)rl4RliFileHeader.synthParams,
                rl4RliFileHeader.reserved);
        }



        //public static explicit operator Brl4RliSubHeaderStruct(Rl4RliSubHeaderStruct rl4RliSubHeader)
        //{
        //    byte[] headerStructArr = RlViewer.Files.LocatorFile.WriteStruct<Rl4RliSubHeaderStruct>(rl4RliSubHeader);

        //    Brl4RliSubHeaderStruct brl4RliSubHeader;
        //    using (var ms = new System.IO.MemoryStream(headerStructArr))
        //    {
        //        brl4RliSubHeader = RlViewer.Files.LocatorFile.ReadStruct<Brl4RliSubHeaderStruct>(ms);
        //    }
        //    return brl4RliSubHeader;
        //}


    }
}
