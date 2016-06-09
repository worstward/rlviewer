using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Interpolators.LeastSquares.Abstract
{
    /// <summary>
    /// Incapsulates Least Squares Method
    /// </summary>
    public abstract class LeastSquares : RlViewer.Behaviors.ImageAligning.IInterpolationProvider
    {
        public LeastSquares(Behaviors.PointSelector.PointSelector selector)
        {
            _selector = selector;
        }

        public LeastSquares(IEnumerable<System.Drawing.PointF> points)
        {
            
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
        public abstract float GetValueAt(float x);
       
    }
}
