using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RlViewer.Forms
{
    public class BindableToolStripPB : ToolStripProgressBar, IBindableComponent
    {
        private ControlBindingsCollection _bindings;

        private BindingContext _context;

        public BindingContext BindingContext
        {
            get
            {
                if (_context == null)
                {
                    _context = new BindingContext();
                }
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        public ControlBindingsCollection DataBindings
        {
            get
            {
                if (_bindings == null)
                {
                    _bindings = new ControlBindingsCollection(this);
                }
                return _bindings;
            }
        }
    }
}
