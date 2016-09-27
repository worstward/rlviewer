using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Filters.Abstract
{
    public abstract class ImageFiltering : ILuTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImageFiltering()
        {
            RegisterFilter();
            FilterValue = Filters[GetType().ToString()].FilterValue;
        }

        public abstract byte[] LuTable { get; protected set; }

        public abstract byte[] InitLut(int step);

        private static Dictionary<string, ImageFiltering> filters = new Dictionary<string, ImageFiltering>();

        public static Dictionary<string, ImageFiltering> Filters
        {
            get
            {
                return filters;
            }
        }

        protected void RegisterFilter()
        {
            var filterType = GetType().ToString();
            //Logging.Logger.Log(Logging.SeverityGrades.Info, string.Format("{0} initialized", filterType));
            filters[filterType] = this;
        }

        public abstract int FilterValue 
        {
            get;
            set;
        }


        public byte[] MergeLut()
        {
            byte[] arr = new byte[256];
            //we start merging with BrightnessFilter
            Array.Copy(filters[typeof(RlViewer.Behaviors.Filters.Concrete.BrightnessFilter).ToString()].LuTable,
                arr, arr.Length);

            if (filters.Values.Count > 1)
            {
                foreach (var f in filters.Values)
                {

                    if (f.GetType() == typeof(RlViewer.Behaviors.Filters.Concrete.BrightnessFilter))
                    {
                        continue;
                    }
                    for (int i = 0; i < 256; i++)
                    {
                        arr[i] = f.LuTable[arr[i]];
                    }

                }
            }
            return arr;
        }


        public System.Drawing.Imaging.ColorPalette ApplyColorFilters(System.Drawing.Imaging.ColorPalette palette)
        {
            byte[] lut = MergeLut();

            System.Drawing.Color[] initialPalette = new System.Drawing.Color[256];
            for (int i = 0; i < initialPalette.Length; i++)
            {
                initialPalette[i] = palette.Entries[i];
            }


            for (int i = 0; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = initialPalette[lut[i]];
            }

            return palette;
        }


        public byte[] ApplyFilters(byte[] imgData)
        {
            byte[] lut = MergeLut();
            //RlViewer.ParallelProperties prop = new RlViewer.ParallelProperties(0, imgData.Length);

            byte[] img = new byte[imgData.Length];

            for (int i = 0; i < img.Length; i++)
            {
                img[i] = lut[imgData[i]];
            }


                //Parallel.ForEach(prop.Chunks, prop.Options, range =>
                //{
                //    for (int i = range.Item1; i < range.Item2; i++)
                //    {
                //        img[i] = lut[imgData[i]];
                //    }
                //});

            return img;

        }


    }
}
