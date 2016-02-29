using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Factories.Filter.Concrete;

namespace RlViewer.Factories.Filter.Abstract
{
    abstract class FilterFactory
    {
        public abstract RlViewer.Behaviors.Filters.Abstract.ImageFiltering GetFilter();

         public static FilterFactory GetFactory(string filterType)
        {
            switch (filterType)
            {
                case "Brightness":
                    return new BrightnessFilterFactory();
                case "Contrast":
                    return new ContrastFilterFactory();
                case "Gamma Correction":
                    return new GammaCorrectionFilterFactory();
                default:
                    throw new NotSupportedException("Unsupported filter type");
            }
        }

    }
}
