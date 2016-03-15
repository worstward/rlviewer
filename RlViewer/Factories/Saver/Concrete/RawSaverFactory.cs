using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Saving.Concrete;

namespace RlViewer.Factories.Saver.Concrete
{
    class RawSaverFactory : RlViewer.Factories.Saver.Abstract.SaverFactory
    {
        public override Behaviors.Saving.Abstract.Saver Create(Files.LoadedFile file)
        {
            return new RawSaver(file);
        }
    }
}
