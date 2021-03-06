﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Filters
{

    public class ImageFilterProxy
    {
        public ImageFilterProxy()
        {
            GetFilter(FilterType.Brightness, 4);
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

        public void GetFilter(FilterType filterType, int filterDelta)
        {
            _filterDelta = filterDelta;
            _filter = RlViewer.Factories.Filter.Abstract.FilterFactory.GetFactory(filterType).Create();
        }

        public void ResetFilters()
        {
            foreach (var filter in RlViewer.Behaviors.Filters.Abstract.ImageFiltering.Filters)
            {
                filter.Value.FilterValue = 0;
            }
        }


        public void ChangeFilterValue(int value)
        {
            _filter.FilterValue = value << _filterDelta;
        }
    }
}
