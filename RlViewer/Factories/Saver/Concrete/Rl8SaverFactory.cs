using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors.Saving.Concrete;

namespace RlViewer.Factories.Saver.Concrete
{
    class Rl8SaverFactory : RlViewer.Factories.Saver.Abstract.SaverFactory
    {
        public override Behaviors.Saving.Abstract.Saver Create(Files.LocatorFile file)
        {
            return new Rl8Saver(file);
        }


    }
}
