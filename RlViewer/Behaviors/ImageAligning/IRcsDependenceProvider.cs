﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ImageAligning
{
    public interface IRcsDependenceProvider
    {
        float GetRcsValueAt(float x);
    }
}
