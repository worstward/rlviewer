using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Filters.Concrete
{
    class GammaCorrectionFilter : RlViewer.Behaviors.Filters.Abstract.ImageFiltering
    {
        public GammaCorrectionFilter()
        {
            RegisterFilter();
            FilterValue = Filters[this.GetType().ToString()].FilterValue;
        }


        private byte[] gammaCorrectionLookUp;

        public override byte[] LuTable
        {
            get
            {
                return gammaCorrectionLookUp;
            }
            protected set
            {
                gammaCorrectionLookUp = value;

            }
        }

        static int gammaCorrectionValue;

        public override int FilterValue
        {
            get
            {
                return gammaCorrectionValue;
            }
            set
            {
                gammaCorrectionValue = value;
                gammaCorrectionLookUp = InitLut(gammaCorrectionValue);
            }
        }


        public override byte[] InitLut(int step)
        {
            var gamma = (double)step;
            gamma = gamma == 0 ? 1 : gamma;
            gamma = gamma < 0 ? 1 / -gamma : gamma;

            double value = 0;

            byte[] table = new byte[256];

            for (int i = 0; i < 256; i++)
            {
                table[i] = (byte)i;
            }

            double gammaCorrection = 1 / gamma;

            for (int i = 0; i < 256; i++)
            {
                value = Math.Pow((table[i] / 255f), gammaCorrection) * 255;

                if (value > 255) value = 255;
                else if (value < 0) value = 0;
                table[i] = (byte)value;
            }
            return table;
        }

    }
}
