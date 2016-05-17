using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning.LeastSquares.Abstract
{
    /// <summary>
    /// Incapsulates Least Squares Method
    /// </summary>
    public abstract class LeastSquares : IRcsDependenceProvider
    {
        public LeastSquares(Behaviors.PointSelector.PointSelector selector)
        {
            _selector = selector;
        }


        private Behaviors.PointSelector.PointSelector _selector;
        protected Behaviors.PointSelector.PointSelector Selector
        {
            get
            {
                return _selector;
            }
        }


        protected abstract float LeastSquaresValueAt(float x);
        public abstract float GetRcsValueAt(float x);
       
    }
}
