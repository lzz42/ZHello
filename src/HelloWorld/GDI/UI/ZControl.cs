using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZHello.GDI.UI
{
    public class ZControl : Control
    {
        protected Graphics DC { get; set; }
        protected ZControl()
        {
            DC = CreateGraphics();
            DRect = new Rectangle(10, 10, 100, 100);
        }

        public Rectangle DRect { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            //region.Xor()
        }
    }
}
