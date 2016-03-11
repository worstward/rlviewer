using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Facades
{

    class ImageFilterFacade
    {

        private RlViewer.Behaviors.Filters.Abstract.ImageFiltering _filter;

        public RlViewer.Behaviors.Filters.Abstract.ImageFiltering Filter
        {
            get
            {
                return _filter;
            }
        }
        private int _filterDelta;

        public void GetFilter(string filterType, int filterDelta)
        {
            _filterDelta = filterDelta;
            _filter = RlViewer.Factories.Filter.Abstract.FilterFactory.GetFactory(filterType).GetFilter();

        }

        public void ChangeFilterValue(int value)
        {
            _filter.FilterValue = value << _filterDelta;
        }
    }
}
