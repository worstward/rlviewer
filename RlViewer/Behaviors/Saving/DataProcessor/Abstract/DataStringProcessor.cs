using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Saving.DataProcessor.Abstract
{
    public abstract class DataStringProcessor
    {
        public abstract float[] ProcessDataString(byte[] frameStrData);

        public byte[] ProcessRawDataString(byte[] frameStrData)
        {
            if (frameStrData == null)
            {
                throw new ArgumentNullException("frameStrData");
            }

            var processedFloatData = ProcessDataString(frameStrData);
            var processedData = new byte[processedFloatData.Length * sizeof(float)];

            Buffer.BlockCopy(processedFloatData, 0, processedData, 0, processedData.Length);
            return processedData;
        }
    }
}
