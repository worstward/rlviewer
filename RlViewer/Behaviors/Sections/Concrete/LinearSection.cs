using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RlViewer.Behaviors.Sections.Abstract;

namespace RlViewer.Behaviors.Sections.Concrete
{
    class LinearSection : Section
    {
        public LinearSection(int sectionLength, Point p) : base(sectionLength, p)
        {
            InitialPoint = p;
        }

        public override IEnumerable<PointF> GetValues(Files.LocatorFile file, Point p)
        {
            int fromInclusive;
            int toInclusive;
            var coordPairList = new List<PointF>();

            if (Math.Abs(InitialPoint.X - p.X) > Math.Abs(InitialPoint.Y - p.Y))
            {
                fromInclusive = InitialPoint.X > p.X ? p.X : InitialPoint.X;
                toInclusive = InitialPoint.X > p.X ? InitialPoint.X : p.X;

                fromInclusive = fromInclusive > file.Width ? file.Width : fromInclusive;
                fromInclusive = fromInclusive < 0 ? 0 : fromInclusive;

                toInclusive = toInclusive > file.Width ? file.Width : toInclusive;
                toInclusive = toInclusive < 0 ? 0 : toInclusive;

                for (int i = fromInclusive; i < toInclusive; i++)
                {
                    var pointToGetSample = new Point(i, GeometryHelper.GetY(InitialPoint, p, i));
                    if (pointToGetSample.X < 0 || pointToGetSample.X > file.Width ||
                        pointToGetSample.Y < 0 || pointToGetSample.Y > file.Height)
                    {
                        continue;
                    }

                    try
                    {
                        coordPairList.Add(new PointF(i, (float)file.GetSample(pointToGetSample).ToFileSample(file.Properties.Type, file.Header.BytesPerSample)));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }


            }
            else
            {
                fromInclusive = InitialPoint.Y > p.Y ? p.Y : InitialPoint.Y;
                toInclusive = InitialPoint.Y > p.Y ? InitialPoint.Y : p.Y;

                fromInclusive = fromInclusive > file.Height ? file.Height : fromInclusive;
                fromInclusive = fromInclusive < 0 ? 0 : fromInclusive;

                toInclusive = toInclusive > file.Height ? file.Height : toInclusive;
                toInclusive = toInclusive < 0 ? 0 : toInclusive;

                for (int i = fromInclusive; i < toInclusive; i++)
                {
                    var pointToGetSample = new Point(GeometryHelper.GetX(InitialPoint, p, i), i);
                    if (pointToGetSample.X < 0 || pointToGetSample.X >= file.Width ||
                        pointToGetSample.Y < 0 || pointToGetSample.Y >= file.Height)
                    {
                        continue;
                    }

                    coordPairList.Add(new PointF(i, file.GetSample(pointToGetSample).ToFileSample(file.Properties.Type, file.Header.BytesPerSample)));
                }

            }          
           
            return coordPairList;
        }
    }
}
