using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.Filter.Concrete
{
    class BrightnessFilterFactory : Filter.Abstract.FilterFactory
    {
        public override RlViewer.Behaviors.Filters.Abstract.ImageFiltering GetFilter()
        {
            return new RlViewer.Behaviors.Filters.Concrete.BrightnessFilter();
        }
    }
}
