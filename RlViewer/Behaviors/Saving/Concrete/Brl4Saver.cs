using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Saving.Concrete
{
    class Brl4Saver : RlViewer.Behaviors.Saving.Abstract.Saver
    {
        public Brl4Saver(Files.LocatorFile loc)
            : base(loc)
        { }

        public override void Save(string path, FileType saveAsType, Point leftTop, Size areaSize)
        {
            throw new NotImplementedException();
        }

        private void SaveAsBrl4()
        {
 
        }



    }
}
