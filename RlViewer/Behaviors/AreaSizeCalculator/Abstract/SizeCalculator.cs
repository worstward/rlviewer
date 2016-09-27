using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.AreaSizeCalculator.Abstract
{
    public abstract class SizeCalculator
    {
        protected abstract float Dx
        {
            get;
        }

        protected abstract float Dy
        {
            get;
        }

        //protected abstract float Frequency
        //{
        //    get;
        //}

        //protected abstract float ImpulseLength
        //{
        //    get;
        //}

        /// <summary>
        /// Calculates area in square meters
        /// </summary>
        /// <param name="AreaWidth">Width of sought-for area</param>
        /// <param name="AreaHeight">Height of sought-for area</param>
        /// <returns></returns>
        public float CalculateArea(int AreaWidth, int AreaHeight)
        {
            return CalculateArea(AreaWidth, AreaHeight, Dx, Dy);
        }

        /// <summary>
        /// Calculates area in square meters
        /// </summary>
        /// <param name="AreaWidth">Width of sought-for area</param>
        /// <param name="AreaHeight">Height of sought-for area</param>
        /// <param name="dx">Range decomposition step</param>
        /// <param name="dy">Azimuth decomposition step</param>
        /// <returns></returns>
        protected virtual float CalculateArea(int AreaWidth, int AreaHeight, float dx = 1, float dy = 1)
        {
            return AreaWidth * dx * AreaHeight * dy;
        }

    }
}
