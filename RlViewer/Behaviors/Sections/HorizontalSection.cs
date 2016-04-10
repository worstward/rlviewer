using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.Sections
{
    public class HorizontalSection : Section
    {
        public HorizontalSection(int sectionLength) : base(sectionLength)
        {
        }

        public override IEnumerable<PointF> GetValues(RlViewer.Files.LocatorFile file, Point p)
        {
            InitialPointMark = p.X;

            var coordPairList = new List<PointF>();

            for (int i = p.X - SectionLength / 2; i < p.X + SectionLength / 2; i++)
            {
                if (i < 0 || i > file.Width) continue;

                coordPairList.Add(new PointF(i, FileReader.GetSample(file, new Point(i, p.Y))));
            }

            return coordPairList;
        }


    }
}
