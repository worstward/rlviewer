using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSelector
{
    public class AreaSelectorsAlignerContainer : IEnumerable<AreaSelectorWrapper>
    {

        private IList<AreaSelectorWrapper> _areaSelectors = new List<AreaSelectorWrapper>();

        protected IList<AreaSelectorWrapper> SelectedAreas
        {
            get { return _areaSelectors; }
            set { _areaSelectors = value; }
        }


        public AreaSelectorWrapper this[int index]
        {
            get
            {
                return SelectedAreas[index];
            }
        }

        public IEnumerator<AreaSelectorWrapper> GetEnumerator()
        {
            return SelectedAreas.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public void AddArea(AreaSelectorWrapper area)
        {
            _areaSelectors.Add(area);
        }

        public void RemoveArea()
        {
            if (_areaSelectors.Count != 0)
            {
                _areaSelectors.Remove(_areaSelectors.Last());
            }
        }

    }
}
