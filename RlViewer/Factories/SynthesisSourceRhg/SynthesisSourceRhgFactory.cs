using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.SynthesisSourceRhg
{
    public class SynthesisSourceRhgFactory
    {
        public static Behaviors.Synthesis.Hologram.Abstract.SynthesisSourceKRhg Create(int frequencyRange, Files.Rhg.Abstract.RhgFile rhgFile)
        {
            switch (frequencyRange)
            {
                case 1:
                    return new Behaviors.Synthesis.Hologram.Concrete.FirstFrequencyRangeHologram(rhgFile);
                case 2:
                    return new Behaviors.Synthesis.Hologram.Concrete.SecondFrequencyRangeHologram(rhgFile);
                case 3:
                    return new Behaviors.Synthesis.Hologram.Concrete.ThirdFrequncyRangeHologram(rhgFile);
                case 4:
                    return new Behaviors.Synthesis.Hologram.Concrete.FourthFrequencyRangeHologram(rhgFile);

                default:
                    throw new ArgumentException("frequencyRange");
            }
        }


    }
}
