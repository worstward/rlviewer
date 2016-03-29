using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Sections
{
    public abstract class Section : PointReader
    {
        public Section(int sectionLength)
        {
            SectionLength = sectionLength;
        }

        protected int SectionLength
        {
            get;
            private set;
        }

        public float InitialPointMark
        {
            get;
            protected set;
        }

        public abstract IEnumerable<PointF> GetValues(RlViewer.Files.LocatorFile file, Point p);
    }
}
