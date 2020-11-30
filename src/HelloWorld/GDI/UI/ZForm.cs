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

    public class ZForm : Form
    {
        public Rectangle DRect { get; set; }
        public ZForm()
        {
            DRect = new Rectangle(50, 10, 80, 80);
        }

        public void DrawTestGrahpics()
        {
            var dc = CreateGraphics();
            dc.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new RectangleF(new PointF(0, 0), dc.VisibleClipBounds.Size);
            rect.Inflate(-20, -20);
            //rect.Offset(10, 10);
            dc.DrawRectangles(Pens.Red, new RectangleF[] { rect });
            dc.DrawEllipse(Pens.Green, rect);
            var s = $"X:{rect.X},Y:{rect.Y},W:{rect.Width},H:{rect.Height}";
            var loc = rect.Location;
            dc.DrawString(s, new Font("微软雅黑", 14f), Brushes.Black, loc);
            loc.Y += 15;
            s = $"X:{this.Location.X},Y:{this.Location.Y},W:{this.Width},H:{this.Height}";
            dc.DrawString(s, new Font("微软雅黑", 14f), Brushes.Black, loc);

            dc.DrawAxisX(Color.Red, new Rectangle(100, 100, 400, 200), 10);
            dc.DrawAxisY(Color.Red, new Rectangle(100, 100, 200, 400), 10);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //不要基类的调用，否则可能会阻止Windows正确执行任务，导致不可预料的结果 C#高级编程第七版 第48章 48.1.3
            base.OnPaint(e);
            //自己的绘图代码
            var dc = e.Graphics;
            //使用剪裁区域，提高绘图效率，原因：每次OnPaint会导致窗口内的所有内容被重绘，并没有考虑到多少需要重绘
            //获取剪裁区域
            var rect = e.ClipRectangle;
            if (rect.IntersectsWith(DRect))
            {
                dc.DrawRectangle(Pens.Red, DRect);
                dc.DrawEllipse(Pens.Red, DRect);
            }

            //默认值都是以客户端区域的左上角为原点
            PointF pf01 = new PointF(2.5f, 3.6f);
            Point p1 = new Point((int)pf01.X, (int)pf01.Y);
            Point p2 = Point.Round(pf01);
            Point p3 = Point.Truncate(pf01);
            Point p4 = Point.Ceiling(pf01);
            SizeF sizef01 = new SizeF(2.5f, 3.6f);
            PointF pf02 = pf01 + sizef01;

            //使用Region表示复杂图形的区域
            Region region = new Region(DRect);

            var path = new GraphicsPath();
            path.FillMode = FillMode.Winding;
            //追加圆弧
            path.AddArc(10,10, 150, 120, 30, 120);
            //追加线段
            path.AddLine(10, 10, 80, 80);
            path.CloseFigure();
            //添加贝塞尔曲线
            Pen pen = new Pen(Color.Green, 3);
            SolidBrush brush = new SolidBrush(Color.Red);
            dc.FillPath(brush, path);
            dc.DrawPath(pen, path);

        }
    }
}
