using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RlViewer.Behaviors.AreaSelector
{
    public class SelectedArea
    {
        private Point location;
        public Point Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        private int width;

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        private int height;
        public int Height
        {
            get
            {
                return height;
            }
                set
            {
                height = value;
            }
        }

    }


}
