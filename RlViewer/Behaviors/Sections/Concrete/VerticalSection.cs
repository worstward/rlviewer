using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RlViewer.Behaviors.Sections.Abstract;

namespace RlViewer.Behaviors.Sections.Concrete
{
    public class VerticalSection : Section
    {
        public VerticalSection(int sectionLength, Point p) : base(sectionLength, p)
        {
            InitialPoint = p;
            InitialPointMark = p.Y;
        }

        public override IEnumerable<PointF> GetValues(RlViewer.Files.LocatorFile file, Point p1)
        {
            var coordPairList = new List<PointF>();

            for (int i = p1.Y - SectionLength / 2; i < p1.Y + SectionLength / 2; i++)
            {
                if (i < 0 || i > file.Height) continue;

                coordPairList.Add(new PointF(i,
                    file.GetSample(new Point(p1.X, i)).ToFloatSample(file.Header.BytesPerSample)));
            }

            return coordPairList;
        }

       
    }
}
