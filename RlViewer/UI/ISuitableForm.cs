using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.UI
{
    public interface ISuitableForm
    {
        PictureBox Canvas { get; }
        TrackBar FilterTrackBar { get; }
        ToolStripProgressBar ProgressBar { get; }
        ToolStripStatusLabel ProgressLabel { get; }
        ToolStripStatusLabel DistanceLabel { get; }
        RadioButton MarkPointRb { get; }
        RadioButton MarkAreaRb { get; }
        RadioButton SharerRb { get; }
        Button AlignBtn { get; }
        CheckBox NavigationPanelCb { get; }
        DataGridView NavigationDgv { get; }

    }
}
