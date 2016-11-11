using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis.Hologram.Concrete
{
    class ThirdFrequncyRangeHologram : Abstract.SynthesisSourceKRhg
    {
        public ThirdFrequncyRangeHologram(RlViewer.Files.Rhg.Abstract.RhgFile rhgFile)
            : base(rhgFile)
        { }

        public override bool RangeSign
        {
            get
            {
                return true;
            }
        }

        public override bool AzimuthSign
        {
            get
            {
                return true;
            }
        }
    }
}
