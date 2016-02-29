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
            FilterValue = Filters[this.GetType().ToString()].FilterValue;
        }


        private byte[] _brightnessLookUp;

        public override byte[] LuTable
        {
            get
            {
                return _brightnessLookUp;
            }
            protected set
            {
                _brightnessLookUp = value;
            }
        }

        static int _brightnesValue;
        public override int FilterValue
        {
            get
            {
                return _brightnesValue;
            }
            set
            {
                _brightnesValue = value;
                _brightnessLookUp = InitLut(_brightnesValue);
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
