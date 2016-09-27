using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Saving.DataProcessor.Concrete
{
    class DataStringModulusProcessor : Abstract.DataStringProcessor
    {
        public override float[] ProcessDataString(byte[] frameStrData)
        {
            var floatFrameStrData = new float[frameStrData.Length / sizeof(float)];
            var amplitudeModulus = new float[floatFrameStrData.Length / 2];
            Buffer.BlockCopy(frameStrData, 0, floatFrameStrData, 0, frameStrData.Length);

            for (int j = 0; j < floatFrameStrData.Length; j += 2)
            {
                amplitudeModulus[j / 2] = (float)Math.Sqrt(floatFrameStrData[j] * floatFrameStrData[j] +
                    floatFrameStrData[j + 1] * floatFrameStrData[j + 1]);
            }
            return amplitudeModulus;
        }

    }
}
