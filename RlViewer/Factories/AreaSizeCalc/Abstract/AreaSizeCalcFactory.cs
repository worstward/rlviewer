using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.AreaSizeCalc.Abstract
{
    public abstract class AreaSizeCalcFactory
    {

        public abstract Behaviors.AreaSizeCalculator.Abstract.SizeCalculator Create(Headers.Abstract.LocatorFileHeader header);

        public static AreaSizeCalcFactory GetFactory(Files.FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Concrete.Brl4SizeCalcFactory();
                case FileType.rl4:
                    return new Concrete.Rl4SizeCalcFactory();
                case FileType.raw:
                    return new Concrete.RawSizeCalcFactory();
                case FileType.r:
                    return new Concrete.RSizeCalcFactory();
                case FileType.k:
                    return new Concrete.KSizeCalcFactory();
                case FileType.rl8:
                    return new Concrete.Rl8SizeCalcFactory();
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }

    }
}
