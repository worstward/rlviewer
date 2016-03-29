using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Settings
{
    public class Settings
    {
        private bool _allowViewWhileLoading;
        public bool AllowViewWhileLoading
        {
            get { return _allowViewWhileLoading; }
            set { _allowViewWhileLoading = value; }
        }

        private bool _forceTileGen;
        public bool ForceTileGeneration
        {
            get { return _forceTileGen; }
            set { _forceTileGen = value; }
        }


        private int[] _palette = new int[3] { 1, 1, 1 };

        public int[] Palette
        {
            get { return _palette; }
            set { _palette = value; }
        }

        private bool _isPaletteReversed;

        public bool IsPaletteReversed
        {
            get { return _isPaletteReversed; }
            set { _isPaletteReversed = value; }
        }

        private int _sectionSize = 500;

        public int SectionSize
        {
            get { return _sectionSize; }
            set { _sectionSize = value; }
        }



    }
}
