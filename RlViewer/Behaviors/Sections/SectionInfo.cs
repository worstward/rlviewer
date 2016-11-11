using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Sections
{
    public class SectionInfo
    {
        public SectionInfo(RlViewer.Behaviors.Sections.Abstract.Section section, List<System.Drawing.PointF> points, float mark)
        {
            _section = section;
            _points = points;
            _mark = mark;
        }

        private Behaviors.Sections.Abstract.Section _section;

        public Behaviors.Sections.Abstract.Section Section
        {
            get
            {
                return _section;
            }
        }


        private List<System.Drawing.PointF> _points;
        public List<System.Drawing.PointF> Points
        {
            get
            { 
                return _points;
            }
        }

        private float _mark;
        public float Mark
        {
            get 
            { 
                return _mark;
            }
        }

    }
}
