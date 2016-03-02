﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.GuiFacade
{
    interface ISuitableForm
    {
        PictureBox Canvas { get; }
        HScrollBar Horizontal { get; }
        VScrollBar Vertical { get; }
        TrackBar TrackBar { get; }
        ProgressBar ProgressBar { get; }
        CheckBox ReverseCheckBox { get; }
        ComboBox PaletteComboBox { get; }        
    }
}