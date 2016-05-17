using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.Saver
{
    class RSaverFactory : RlViewer.Factories.Saver.Abstract.SaverFactory
    {
        public override Behaviors.Saving.Abstract.Saver Create(Files.LocatorFile file)
        {
            return new RlViewer.Behaviors.Saving.Concrete.RSaver(file);
        }
    }
}
