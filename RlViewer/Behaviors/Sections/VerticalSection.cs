using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Sections
{
    public class VerticalSection : Section
    {
        public VerticalSection(int sectionLength) : base(sectionLength)
        {
        }

        public override IEnumerable<Tuple<float, float>> GetValues(RlViewer.Files.LocatorFile file, System.Drawing.Point p)
        {
            InitialPointMark = p.Y;

            var coordPairList = new List<Tuple<float, float>>();

            for (int i = p.Y - SectionLength / 2; i < p.Y + SectionLength / 2; i++)
            {
                if (i < 0 || i > file.Height) continue;

                coordPairList.Add(new Tuple<float, float>(i, GetValue(file, new System.Drawing.Point(p.X, i))));
            }

            return coordPairList;
        }


    }
}
