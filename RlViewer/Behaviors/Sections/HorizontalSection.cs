using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Sections
{
    public class HorizontalSection : Section
    {
        public HorizontalSection(int sectionLength) : base(sectionLength)
        {
        }

        public override IEnumerable<Tuple<float, float>> GetValues(RlViewer.Files.LocatorFile file, System.Drawing.Point p)
        {
            InitialPointMark = p.X;

            var coordPairList = new List<Tuple<float, float>>();

            for (int i = p.X - SectionLength / 2; i < p.X + SectionLength / 2; i++)
            {
                if (i < 0 || i > file.Width) continue;

                coordPairList.Add(new Tuple<float, float>(i, GetValue(file, new System.Drawing.Point(i, p.Y))));
            }

            return coordPairList;
        }


    }
}
