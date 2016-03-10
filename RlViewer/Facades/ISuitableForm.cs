using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.Facades
{
    public interface ISuitableForm
    {
        PictureBox Canvas { get; }
        HScrollBar Horizontal { get; }
        VScrollBar Vertical { get; }
        TrackBar TrackBar { get; }
        ProgressBar ProgressBar { get; }
        Label ProgressLabel { get; }
        Button CancelButton { get; }
        RadioButton DragRb { get; }
        RadioButton MarkPointRb { get; }
        RadioButton MarkAreaRb { get; }
    }
}
