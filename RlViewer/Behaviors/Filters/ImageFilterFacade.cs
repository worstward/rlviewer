using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Filters
{

    class ImageFilterFacade
    {
        public ImageFilterFacade()
        {
            GetFilter("Brightness", 4);
        }


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
            _filter = RlViewer.Factories.Filter.Abstract.FilterFactory.GetFactory(filterType).Create();

        }

        public void ChangeFilterValue(int value)
        {
            _filter.FilterValue = value << _filterDelta;
        }
    }
}
