﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Saving.Concrete
{
    public class KSaver : Abstract.Saver
    {

        public KSaver(Files.LocatorFile file)
            : base(file)
        {
 
        }

        public override void Save(string path, FileType destinationType, System.Drawing.Rectangle area, Filters.ImageFilterFacade filter, float normalization, float maxValue)
        {
            throw new NotImplementedException();
        }

        public override void SaveAsAligned(string fileName, System.Drawing.Rectangle area, byte[] image)
        {
            throw new NotImplementedException();
        }

        public override Files.LocatorFile SourceFile
        {
            get { throw new NotImplementedException(); }
        }

    }
}