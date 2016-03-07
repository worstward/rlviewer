using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.GuiFacade
{
    class ImageFilterFacade
    {
        public ImageFilterFacade(System.Windows.Forms.TrackBar filterTrackBar)
        {
            _filterTrackBar = filterTrackBar;
        }

        private System.Windows.Forms.TrackBar _filterTrackBar;
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
            _filterTrackBar.Value = _filter.FilterValue >> _filterDelta;
        }

        public void ChangeFilterValue()
        {
            _filter.FilterValue = _filterTrackBar.Value << _filterDelta;
        }
    }
}
