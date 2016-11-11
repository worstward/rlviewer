using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis.Hologram.Concrete
{
    class SecondFrequencyRangeHologram : Abstract.SynthesisSourceKRhg
    {
        public SecondFrequencyRangeHologram(RlViewer.Files.Rhg.Abstract.RhgFile rhgFile)
            : base(rhgFile)
        { }

        public override bool RangeSign
        {
            get
            {
                return false;
            }
        }

        public override bool AzimuthSign
        {
            get
            {
                return false;
            }
        }
    }
}
