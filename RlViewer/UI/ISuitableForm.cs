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
        HScrollBar Horizontal { get; }
        VScrollBar Vertical { get; }
        TrackBar FilterTrackBar { get; }
        Label FilterValueLabel { get; }
        ToolStripProgressBar ProgressBar { get; }
        ToolStripStatusLabel ProgressLabel { get; }
        ToolStripStatusLabel StatusLabel { get; }
        Label ScaleLabel { get; }
        ToolStripStatusLabel CoordinatesLabel { get; }
        ToolStripStatusLabel DistanceLabel { get; }
        ToolStripDropDownButton CancelButton { get; }
        RadioButton DragRb { get; }
        RadioButton MarkPointRb { get; }
        RadioButton MarkAreaRb { get; }
        RadioButton AnalyzePointRb { get; }
        RadioButton VerticalSectionRb { get; }
        RadioButton HorizontalSectionRb { get; }
        RadioButton RulerRb { get; }
        Button AlignBtn { get; }
        CheckBox NavigationCb { get; }
        DataGridView NavigationDgv { get; }
        SplitContainer WorkingAreaSplitter { get; }
    }
}
