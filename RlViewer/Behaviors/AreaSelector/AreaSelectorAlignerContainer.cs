using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSelector
{
    public class AreaSelectorsAlignerContainer : IEnumerable<AreaSelectorDecorator>
    {

        private IList<AreaSelectorDecorator> _areaSelectors = new List<AreaSelectorDecorator>();

        protected IList<AreaSelectorDecorator> SelectedAreas
        {
            get { return _areaSelectors; }
            set { _areaSelectors = value; }
        }


        public AreaSelectorDecorator this[int index]
        {
            get
            {
                return SelectedAreas[index];
            }
        }

        public IEnumerator<AreaSelectorDecorator> GetEnumerator()
        {
            return SelectedAreas.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public void AddArea(AreaSelectorDecorator area)
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
