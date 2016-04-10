using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Sections
{
    public class VerticalSection : Section
    {
        public VerticalSection(int sectionLength) : base(sectionLength)
        {
        }

        public override IEnumerable<PointF> GetValues(RlViewer.Files.LocatorFile file, Point p)
        {
            InitialPointMark = p.Y;

            var coordPairList = new List<PointF>();

            for (int i = p.Y - SectionLength / 2; i < p.Y + SectionLength / 2; i++)
            {
                if (i < 0 || i > file.Height) continue;

                coordPairList.Add(new PointF(i, FileReader.GetSample(file, new Point(p.X, i))));
            }

            return coordPairList;
        }


    }
}
