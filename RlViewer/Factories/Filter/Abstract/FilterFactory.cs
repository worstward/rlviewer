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
        public abstract RlViewer.Behaviors.Filters.Abstract.ImageFiltering Create();

        public static FilterFactory GetFactory(Behaviors.Filters.FilterType filterType)
        {
            switch (filterType)
            {
                case Behaviors.Filters.FilterType.Brightness:
                    return new BrightnessFilterFactory();
                case Behaviors.Filters.FilterType.Contrast:
                    return new ContrastFilterFactory();
                case Behaviors.Filters.FilterType.GammaCorrection:
                    return new GammaCorrectionFilterFactory();
                default:
                    throw new NotSupportedException("Unsupported filter type");
            }
        }

    }
}
