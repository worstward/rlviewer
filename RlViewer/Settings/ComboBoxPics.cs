using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace RlViewer.Settings
{
    class ComboBoxPics : ComboBox
    {
        public ComboBoxPics()
            : base()
        {

            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= this.Items.Count)
            {
                base.OnDrawItem(e);
                return;
            }

            if (!(this.Items[e.Index] is CboItem))
            {
                base.OnDrawItem(e);
                return;
            }

            var item = this.Items[e.Index] as CboItem;

            e.DrawBackground();

            if (item.Image != null)
            {
                e.Graphics.DrawImage(item.Image, e.Bounds.X, e.Bounds.Y, item.Image.Width, e.Bounds.Height);
                //e.Graphics.DrawString(item.Text, e.Font, Brushes.Black, new RectangleF(
                //        item.Image.Width + 5,
                //        e.Bounds.Y,
                //        e.Bounds.Width - item.Image.Width - 5,
                //        e.Bounds.Height
                //    )
                //);
            }
            else
            {
                e.Graphics.DrawString(item.Text, e.Font, Brushes.Black,
                    new RectangleF(
                        5,
                        e.Bounds.Y,
                        e.Bounds.Width - 5,
                        e.Bounds.Height
                    )
                );
            }

            if (e.Index == this.SelectedIndex)
                e.DrawFocusRectangle();

            base.OnDrawItem(e);
        }

    }

    class CboItem : IDisposable
    {
        public string Text
        {
            get;
            set;
        }

        public Image Image
        {
            get;
            set;
        }

        public CboItem(string text, Image img)
        {
            Text = text;
            Image = img;
        }

        public CboItem(string text)
            : this(text, null)
        {

            // TODO
        }

        public override string ToString()
        {
            return Text;
        }

        public void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
                Image = null;
            }
        }
    }

}
