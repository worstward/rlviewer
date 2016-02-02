﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Hierarchy;
using RlViewer.Hierarchy.Rli.Concrete;

namespace RlViewer.Factories
{
    public class Brl4Factory : FileFactory
    {
        public override  LoadedFile Create(FileProperties properties)
        {
            return new Brl4(properties);
        }

       

    }
}