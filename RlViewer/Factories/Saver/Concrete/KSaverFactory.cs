using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Saving.Concrete;

namespace RlViewer.Factories.Saver.Concrete
{
    class KSaverFactory : RlViewer.Factories.Saver.Abstract.SaverFactory
    {
        public override Behaviors.Saving.Abstract.Saver Create(Files.LocatorFile file)
        {
            return new KSaver(file);
        }
    }
}
