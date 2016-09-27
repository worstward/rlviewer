using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.TileCreator
{
    public static class NormalizationHelpers
    {


        public static float GetLinearValue(float sample, float normalizationFactor)
        {
            return sample / normalizationFactor * 255;
        }

        /// <summary>
        /// Gets a logarithmic grade value of a provided sample
        /// </summary>
        /// <param name="sample">sample to convert</param>
        /// <param name="maxvalue">max sample value for full image</param>
        /// <returns></returns>
        public static float GetLogarithmicValue(float sample, float maxvalue)
        {
            var sampleLog = Math.Log10(sample);
            var normLog = Math.Log10(maxvalue);
            var quotient = sampleLog / normLog;
            return 255 * (float)quotient;
        }

        /// <summary>
        /// Gets a linear-logarithmic grade value of a provided sample
        /// </summary>
        /// <param name="sample">sample to convert</param>
        /// <param name="maxvalue">max sample value for full image</param>
        /// <param name="normalizationFactor">normalization value for full image</param>
        /// <returns></returns>
        public static float GetLinearLogarithmicValue(float sample, float maxvalue, float normalizationFactor)
        {

            float border = normalizationFactor / 9f * 7;

            //if sample is less then border value then it gets linear value, then logarithmic
            if (sample < border)
            {
                return 191 * sample / normalizationFactor;
            }
            else
            {
                return GetLogarithmicValue(sample, maxvalue);
            }
        }

        /// <summary>
        /// Casts float value to byte range
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte ToByteRange(float val)
        {
            val = val > 255 ? 255 : val;
            val = val < 1 ? 0 : val;
            byte b = (byte)val;
            return b;
        }


    }
}
