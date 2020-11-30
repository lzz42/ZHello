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
                Size = new Size(800, 600),
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
                rec1.Offset(f.Width / 2, 0);
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
                Size = new Size(1080, 800),
                Text = "ZForm"
            };
            var btn = new Button()
            {
                Text = "Draw",
                Location = new Point(10, 400),
            };
            var num = new NumericUpDown()
            {
                Minimum=1,
                Maximum=500,
                Value=1,
                Size = new Size(60,30),
                Location = new Point(10, 430),
            };
            num.ValueChanged += (s, e) =>
            {
                btn.PerformClick();
            };
            btn.MouseClick += (s, e) =>
            {
                ZHello.GDI.GDI_Draw.DDX = (int)num.Value;
                var dc = f.CreateGraphics();
                dc.Clear(Color.White);
                //f.DrawTestGrahpics();
                //dc.DrawStar(new Rectangle(200, 100, 200, 200), Color.Red);
                int radius = 80;
                dc.DrawPolygonX(new PointF(200, 200), radius, 3, Color.Red);
                dc.DrawPolygonX(new PointF(400, 200), radius, 4, Color.Blue);
                dc.DrawPolygonX(new PointF(600, 200), radius, 5, Color.Green);
                dc.DrawPolygonX(new PointF(800, 200), radius, 6, Color.Yellow);
                
                dc.DrawPolygonX(new PointF(200, 400), radius, 7, Color.Red);
                dc.DrawPolygonX(new PointF(400, 400), radius, 8, Color.Blue);
                dc.DrawPolygonX(new PointF(600, 400), radius, 9, Color.Green);
                dc.DrawPolygonX(new PointF(800, 400), radius, 10, Color.Yellow);

                dc.DrawPolygonX(new PointF(200, 600), radius, 11, Color.Red);
                dc.DrawPolygonX(new PointF(400, 600), radius, 12, Color.Blue);
                dc.DrawPolygonX(new PointF(600, 600), radius, 13, Color.Green);
                dc.DrawPolygonX(new PointF(800, 600), radius, 14, Color.Yellow);

            };
            var axis = new AxisRenderer()
            {
                AxisType = AxisType.AxisXY,
            };
            f.Paint += (s, e) =>
            {
                //axis.Bounds = new Rectangle(10, 10, f.Width-40, f.Height-70);
                //axis.Render(e.Graphics);
                //e.Graphics.DrawStar(new Rectangle(100, 100, 100, 100), Color.Red);
            };
            f.Controls.Add(btn);
            f.Controls.Add(num);
            f.ShowDialog();
        }
    }
}
