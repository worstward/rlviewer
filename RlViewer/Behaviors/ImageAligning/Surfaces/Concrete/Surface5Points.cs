using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using RlViewer.Behaviors;
using RlViewer.Behaviors.PointSelector;

namespace RlViewer.Behaviors.ImageAligning.Surfaces.Concrete
{
    /// <summary>
    /// Incapsulates surface made from 5 points
    /// </summary>
    class Surface5Points : Surface3Points
    {
        public Surface5Points(PointSelector.CompressedPointSelectorWrapper selector, IInterpolationProvider rcsProvider)
            : base(selector, rcsProvider)
        {
            _rcsProvider = rcsProvider;
        }

        private object _amplitudeSolutionLocker = new object();

        private float[][] _amplitudeSolution;
        private float[][] AmplitudeSolution
        {
            get
            {
                lock (_amplitudeSolutionLocker)
                {
                    return _amplitudeSolution = _amplitudeSolution ?? InitAmplitudePlanes();
                }
            }
        }


        private IInterpolationProvider _rcsProvider;
        protected override IInterpolationProvider RcsProvider
        {
            get
            {
                return _rcsProvider;
            }
        
        }

        private object _rscSolutionLocker = new object();

        private float[][] _rcsSolution;
        private float[][] RcsSolution
        {
            get
            {
                lock (_rscSolutionLocker)
                {
                    return _rcsSolution = _rcsSolution ?? InitRcsPlanes();
                }
            }
        }




        public override byte[] ResampleImage(RlViewer.Files.LocatorFile file, System.Drawing.Rectangle area)
        {
            float[] image = new float[area.Width * area.Height];

            float[] imageArea = file.GetArea(area).ToArea<float>(file.Header.BytesPerSample);
            int toInclusiveX = area.Location.X + area.Width;
            toInclusiveX = toInclusiveX > file.Width ? file.Width : toInclusiveX;

            int toInclusiveY = area.Location.Y + area.Height;
            toInclusiveY = toInclusiveY > file.Height ? file.Height : toInclusiveY;
            int counter = 0;


            Parallel.For(area.Location.X, toInclusiveX, (i,loopState) =>
            {
                for (int j = area.Location.Y; j < toInclusiveY; j++)
                {
                    var oldVal = imageArea[(j - area.Location.Y) * area.Width + (i - area.Location.X)];
                    var newVal = GetAmplitude(i, j);
                    var diff = oldVal / newVal * RcsProvider.GetValueAt(oldVal);
                    diff = diff < 0 ? 0 : diff;
                    image[(j - area.Location.Y) * area.Width + (i - area.Location.X)] = diff;
                }

                System.Threading.Interlocked.Increment(ref counter);
                OnProgressReport((int)(counter / Math.Ceiling((double)(toInclusiveX - area.Location.X)) * 100));
                if (OnCancelWorker())
                {
                    loopState.Break();
                }

            });

            if (Cancelled)
            {
                return null;
            }

            byte[] imageB = new byte[image.Length * 4];

            Buffer.BlockCopy(image, 0, imageB, 0, imageB.Length);

            return imageB;
        }

        /// <summary>
        /// Gets Z coordinate of a point
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns></returns>
        protected float GetAmplitude(int x, int y)
        {
            return GetPlaneValue(x, y, PointToPlane(new System.Drawing.Point(x, y)));
        }


        protected override IList<SelectedPoint> OrderAsMatrix(IList<SelectedPoint> selectedPoints)
        {
            var centralPoint = GetCentralPoint(selectedPoints);
            var selectedNoCentral = selectedPoints.Where(x => !x.Equals(centralPoint)).ToList();
            var matrix = base.OrderAsMatrix(selectedNoCentral).ToList();

            matrix.Add(centralPoint);

            return matrix;
        }


        private SelectedPoint GetCentralPoint(IEnumerable<SelectedPoint> selectedPoints)
        {
            var initialPoints = selectedPoints.Select(x => x).ToList();
            var maxAngles = new List<double>();
            var vectors = new List<System.Windows.Vector>();
            var angles = new List<double>();

            //build up vectors from 1 selected point to all others, repeat for each point
            for(int i = 0; i < initialPoints.Count - 1; i++)
            {
                vectors.Clear();
                angles.Clear();

                foreach (var sidePoint in initialPoints.Where(x => !x.Equals(initialPoints[i])))
                {
                    vectors.Add(new System.Windows.Vector(sidePoint.Location.X - initialPoints[i].Location.X, sidePoint.Location.Y - initialPoints[i].Location.Y));
                }

                //get angle between each two co-bordered vectors 
                for (int j = 0; j < vectors.Count; j++)
                {
                    if (j == vectors.Count - 1)
                    {
                        angles.Add(System.Windows.Vector.AngleBetween(vectors[j], vectors[0]));
                        break;
                    }

                    angles.Add(System.Windows.Vector.AngleBetween(vectors[j], vectors[j + 1]));
                }
                angles = angles.Select(x => x < 0 ? 180 - x : x).ToList();


                maxAngles.Add(angles.Max());
            }

            //assuming that least max angle will give more average angles overall
            var minMaxAngle = maxAngles.Min();

            //take center point with that angle combination
            var centerPointIndex = maxAngles.IndexOf(minMaxAngle);

            return selectedPoints.Skip(centerPointIndex).Take(1).FirstOrDefault();
        }


        /// <summary>
        /// Builds a plane through each 3 selected points
        /// </summary>
        /// <returns></returns>
        protected override float[][] InitAmplitudePlanes()
        {
            var centerPoint = Selector.Last();

            var planes = Selector.Combinations<PointSelector.SelectedPoint>(3)
                .Where(x => x.Contains(centerPoint))
                .Where(x => !(x.Contains(centerPoint) && x.Contains(Selector[0]) && x.Contains(Selector[3])))
                .Where(x => !(x.Contains(centerPoint) && x.Contains(Selector[1]) && x.Contains(Selector[2])))
                .ToList();

            float[][] solution = new float[planes.Count][];

            for (int i = 0; i < planes.Count; i++)
            {
                solution[i] = SolveAmplitude(planes[i].ToList());
            }
            return solution;
        }

        /// <summary>
        /// Builds a plane through center and every 2 combinations point
        /// </summary>
        /// <returns></returns>
        protected override float[][] InitRcsPlanes()
        {
            var centerPoint = Selector.Last();

            var planes = Selector.Combinations<PointSelector.SelectedPoint>(3)
               .Where(x => x.Contains(centerPoint))
               .Where(x => !(x.Contains(centerPoint) && x.Contains(Selector[0]) && x.Contains(Selector[3])))
               .Where(x => !(x.Contains(centerPoint) && x.Contains(Selector[1]) && x.Contains(Selector[2])))
               .ToList();


            float[][] solution = new float[planes.Count][];

            for (int i = 0; i < planes.Count; i++)
            {                
                solution[i] = SolveRcs(planes[i].ToList());
            }

            return solution;
        }

        private bool IsInsideAngle(System.Windows.Vector v1, System.Windows.Vector v2, Point p)
        {

            var center = Selector.Last().Location;

            var angle = Math.Abs(System.Windows.Vector.AngleBetween(v1, v2));
            var halfAngle = angle / 2;

            v1.Normalize();
            v2.Normalize();

            var bisector = v1 + v2;
            bisector.Normalize();

            var vectorToPoint = new System.Windows.Vector(p.X - center.X, p.Y - center.Y);
            vectorToPoint.Normalize();

            var AngleBetweenBisectorAndVectorToPoint = Math.Abs(System.Windows.Vector.AngleBetween(bisector, vectorToPoint));

            return AngleBetweenBisectorAndVectorToPoint <= halfAngle; 
        }


        private float[] PointToPlane(Point p)
        {
            var initialPoints = Selector.Select(x => x).ToList();
            var vectors = new List<System.Windows.Vector>();

            var centerPoint = Selector.Last();

            foreach (var sidePoint in initialPoints.Where(x => !x.Equals(centerPoint)))
            {
                vectors.Add(new System.Windows.Vector(sidePoint.Location.X - centerPoint.Location.X, sidePoint.Location.Y - centerPoint.Location.Y));
            }


            if (IsInsideAngle(vectors[0], vectors[1], p))
            {
                return AmplitudeSolution[0];
            }
            else if (IsInsideAngle(vectors[1], vectors[2], p))
            {
                return AmplitudeSolution[1];
            }
            else if (IsInsideAngle(vectors[2], vectors[3], p))
            {
                return AmplitudeSolution[2];
            }
            else if (IsInsideAngle(vectors[3], vectors[0], p))
            {
                return AmplitudeSolution[3];
            }



            return AmplitudeSolution[0];
            
        }

    }
}
