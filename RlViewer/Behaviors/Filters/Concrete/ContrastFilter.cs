using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Filters.Concrete
{
    class ContrastFilter : RlViewer.Behaviors.Filters.Abstract.ImageFiltering
    {
        public ContrastFilter()
        {
            RegisterFilter();
            FilterValue = Filters[this.GetType().ToString()].FilterValue;
        }


        private byte[] contrastLookUp;
        public override byte[] LuTable
        {
            get
            {
                return contrastLookUp;
            }
            protected set
            {
                contrastLookUp = value;
            }
        }

        private static int contrastValue;

        public override int FilterValue
        {
            get
            {
                return contrastValue;
            }
            set
            {
                contrastValue = value;
                contrastLookUp = InitLut(contrastValue);
            }
        }
        

     
        public override byte[] InitLut(int step)
        {
            double value = 0;
           
            byte[] table = new byte[256];

            for (int i = 0; i < 256; i++)
            {
                table[i] = (byte)i;
            }

            double contrast = (100f + step) / 100f;

            for (int i = 0; i < 256; i++)
            {
                value = ((((table[i] / 255f) - 0.5) * contrast) + 0.5) * 255f;

                if (value > 255) value = 255;
                else if (value < 0) value = 0;
                table[i] = (byte)value;
            }
            return table;
        }

        public byte[] InitLut(byte[] table, int step)
        {
            double value = 0;

            double contrast = (100f + step) / 100f;

            for (int i = 0; i < 256; i++)
            {
                value = ((((table[i] / 255f) - 0.5) * contrast) + 0.5) * 255f;

                if (value > 255) value = 255;
                else if (value < 0) value = 0;
                table[i] = (byte)value;
            }
            return table;
        }


    }
}
