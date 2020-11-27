using System;
using System.Collections.Generic;
using System.Drawing;
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
        }
    }
}
