﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.Filter.Concrete
{
    class ContrastFilterFactory : Filter.Abstract.FilterFactory
    {
        public override RlViewer.Behaviors.Filters.Abstract.ImageFiltering Create()
        {
            return new RlViewer.Behaviors.Filters.Concrete.ContrastFilter();
        }
    }
}
