﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Headers.Abstract;
using RlViewer.Behaviors.Draw;

namespace RlViewer.Files.Rli.Abstract
{

    /// <summary>
    /// Incapsulates radiolocation image file
    /// </summary>
    public abstract class RliFile : LocatorFile
    {
        protected RliFile(FileProperties properties, Headers.Abstract.LocatorFileHeader header,
            RlViewer.Navigation.NavigationContainer navi) : base(properties, header, navi)
        {

        }

        public override abstract LocatorFileHeader Header { get; }
        public abstract override int Width { get; }
        public abstract override int Height { get; }        
    }
}
