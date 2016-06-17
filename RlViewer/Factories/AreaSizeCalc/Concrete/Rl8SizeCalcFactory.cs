﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.AreaSizeCalc.Concrete
{
    class Rl8SizeCalcFactory : Abstract.AreaSizeCalcFactory
    {
        public override Behaviors.AreaSizeCalculator.Abstract.SizeCalculator Create(Headers.Abstract.LocatorFileHeader header)
        {
            return new Behaviors.AreaSizeCalculator.Concrete.Rl8SizeCalculator(header);
        }
    }
}
