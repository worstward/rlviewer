using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Filters.Concrete
{
    class BrightnessFilter : RlViewer.Behaviors.Filters.Abstract.ImageFiltering
    {
        public BrightnessFilter()
        {
            RegisterFilter();
            FilterValue = Filters[GetType().ToString()].FilterValue;
        }


        private byte[] brightnessLookUp;

        public override byte[] LuTable
        {
            get
            {
                return brightnessLookUp;
            }
            protected set
            {
                brightnessLookUp = value;
            }
        }

        static int brightnesValue;
        public override int FilterValue
        {
            get
            {
                return brightnesValue;
            }
            set
            {
                brightnesValue = value;
                brightnessLookUp = InitLut(brightnesValue);
            }
        }

        public override byte[] InitLut(int step)
        {
            byte[] table = new byte[256];
            int value = 0;
            for (int i = 0; i < 256; i++)
            {
                value = i + step;

                if (value > 255) value = 255;
                else if (value < 0) value = 0;
                table[i] = (byte)value;
            }
            return table;
        }

    }
}
