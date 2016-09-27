using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Saving.DataProcessor.Concrete
{
    class DataStringSampleProcessor : Abstract.DataStringProcessor
    {
        public override float[] ProcessDataString(byte[] frameStrData)
        {
            if (frameStrData == null)
            {
                throw new ArgumentNullException("frameStrData");
            }

            float[] floatFrameStrData = new float[frameStrData.Length / sizeof(float)];
            Buffer.BlockCopy(frameStrData, 0, floatFrameStrData, 0, frameStrData.Length);
            return floatFrameStrData;
        }

    }
}
