﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.CornerCoords.Concrete
{
    class KCornerCoordFactory : Abstract.CornerCoordFactory
    {
        public override Behaviors.ReportGenerator.CornerCoord.Abstract.CornerCoordinates Create(Files.LocatorFile file, int firstLine, int lastLine, bool readToEnd)
        {
            return new Behaviors.ReportGenerator.CornerCoord.Concrete.KCornerCoord(file, firstLine, lastLine, readToEnd);
        }
    }
}
