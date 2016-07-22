﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RlViewer.Behaviors.Sections.Abstract;

namespace RlViewer.Behaviors.Sections.Concrete
{
    public class HorizontalSection : Section
    {
        public HorizontalSection(int sectionLength, Point p) : base(sectionLength, p)
        {
            InitialPoint = p;
            InitialPointMark = p.X;
        }


        public override IEnumerable<PointF> GetValues(RlViewer.Files.LocatorFile file, Point p1)
        {
            var coordPairList = new List<PointF>();

            for (int i = p1.X - SectionLength / 2; i < p1.X + SectionLength / 2; i++)
            {
                if (i < 0 || i >= file.Width || p1.Y < 0 || p1.Y >= file.Height) continue;

                try
                { 
                    coordPairList.Add(new PointF(i, file.GetSample(new Point(i, p1.Y)).ToFileSample(file.Properties.Type, file.Header.BytesPerSample)));
                }
                catch(Exception)
                {
                    throw;
                }
            }

            return coordPairList;
        }


    }
}
