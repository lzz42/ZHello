using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.GDI;
using ZHello.GDI.UI;

namespace ZHello.Test.GDI
{
    [TestClass]
    public class GDI_Draw
    {
        [TestMethod]
        public void DrawImage_ColorMatrix()
        {
            var f = new Form()
            {
                Text = "DrawImage_ColorMatrix Test",
                Size = new Size(800,600),
            };
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var img = Image.FromFile(Path.Combine(dir, "GDI", "ColorRing.png"));
            f.Paint += (s, e) =>
            {
                var g = e.Graphics;
                var rec1 = new Rectangle(0, 0, f.Width / 2 - 5, f.Height / 2 - 5);
                g.DrawRectangle(Pens.Red, rec1);
                rec1.Inflate(-2, -2);
                g.DrawImage(img, rec1);
                rec1.Offset(f.Width/2, 0);
                g.DrawRectangle(Pens.Red, rec1);
                g.DrawImage_ColorMatrix(img, rec1);
            };
            f.ShowDialog();
        }

        [TestMethod]
        public void DrawForm()
        {
            var f = new ZForm()
            {
                Size = new Size(300, 300),
                Text = "ZForm"
            };
            var btn = new Button()
            {
                Text = "Draw",
                Location = new Point(10, 100),
            };
            btn.MouseClick += (s, e) =>
            {
                f.DrawTestGrahpics();
            };
            var axis = new AxisRenderer()
            {
                AxisType= AxisType.AxisXY,
            };
            f.Paint += (s, e) =>
            {
                axis.Bounds = new Rectangle(10, 10, f.Width-40, f.Height-70);
                axis.Render(e.Graphics);
            };
            f.Controls.Add(btn);
            f.ShowDialog();
        }
    }
}
