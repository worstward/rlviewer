using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Settings
{
    public class Settings
    {
        private bool allowViewWhileLoading;
        public bool AllowViewWhileLoading
        {
            get { return allowViewWhileLoading; }
            set { allowViewWhileLoading = value; }
        }

        private bool forceTileGen;
        public bool ForceTileGeneration
        {
            get { return forceTileGen; }
            set { forceTileGen = value; }
        }


        private int[] palette = new int[3] { 1, 1, 1 };

        public int[] Palette
        {
            get { return palette; }
            set { palette = value; }
        }

        private bool isPaletteReversed;

        public bool IsPaletteReversed
        {
            get { return isPaletteReversed; }
            set { isPaletteReversed = value; }
        }


    }
}
