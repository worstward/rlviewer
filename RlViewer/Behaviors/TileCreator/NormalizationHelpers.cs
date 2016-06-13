using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.TileCreator
{
    public static class NormalizationHelpers
    {
        public static float GetLogarithmicValue(float sample, float maxvalue)
        {
            var sampleLog = Math.Log10(sample);
            var normLog = Math.Log10(maxvalue);
            var quotient = sampleLog / normLog;
            return 255 * (float)quotient;
        }

        public static short GetLogarithmicValue(short sample, short maxvalue)
        {
            var sampleLog = Math.Log10(sample);
            var normLog = Math.Log10(maxvalue);
            var quotient = sampleLog / normLog;
            return (short)(255 * quotient);
        }


        public static float GetLinearLogarithmicValue(float sample, float border, float maxvalue, float normalizationFactor)
        {
            if (sample < border)
            {
                return 191 * sample / normalizationFactor;
            }
            else
            {
                return GetLogarithmicValue(sample, maxvalue);
            }
        }

        public static float GetLinearLogarithmicValue(short sample, short border, short maxvalue, short normalizationFactor)
        {
            if (sample < border)
            {
                return (short)(191 * sample / (float)normalizationFactor);
            }
            else
            {
                return GetLogarithmicValue(sample, maxvalue);
            }
        }
        public static byte ToByteRange(short val)
        {
            val = val > 255 ? (short)255 : val;
            val = val < 1 ? (short)0 : val;
            byte b = (byte)val;
            return b;
        }


        public static byte ToByteRange(float val)
        {
            val = val > 255 ? 255 : val;
            val = val < 1 ? 0 : val;
            byte b = (byte)val;
            return b;
        }


    }
}
