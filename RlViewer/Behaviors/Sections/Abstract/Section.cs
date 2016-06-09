using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Sections.Abstract
{
    public abstract class Section
    {
        public Section(int sectionLength, Point p)
        {
            SectionLength = sectionLength;
        }

        protected int SectionLength
        {
            get;
            private set;
        }

        public Point InitialPoint
        {
            get;
            protected set;
        }

        public float InitialPointMark
        {
            get;
            protected set;
        }

        public abstract IEnumerable<PointF> GetValues(RlViewer.Files.LocatorFile file, Point p1);

        public void ResetSection()
        {
            InitialPoint = default(Point);
            InitialPointMark = 0;
        }

    }

}
