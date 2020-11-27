using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ZHello.GDI
{
    public static class GDI_Draw
    {
        /// <summary>
        /// 绘制图片 颜色矩阵
        /// </summary>
        /// <param name="g"></param>
        /// <param name="img"></param>
        /// <param name="rect"></param>
        public static void DrawImage_ColorMatrix(this Graphics g, Image img, Rectangle rect)
        {
            ImageAttributes imgAttrs = new ImageAttributes();
            float[][] colorMatrixElements = new float[][]
            {
                new float[]{0.5f, 0, 0, 0,0},//红色缩放值
                new float[]{0, 0.5f, 0, 0,0},//绿色色缩放值
                new float[]{0, 0, 0.5f, 0,0},//蓝色缩放值f
                new float[]{0, 0, 0, 1f,0},//透明度缩放值
                new float[]{0, 0, 0, 0,1},
            };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imgAttrs.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            g.DrawImage(img, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Point, imgAttrs);
        }

        #region 绘制圆角矩形

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius, Color color)
        {
            if (g == null || rect == null || radius < 0)
                return;
            // 圆角半径
            //int radius = radius;
            var circleSize = new Size(radius * 2, radius * 2);
            // 指定图形路径， 有一系列 直线/曲线 组成
            GraphicsPath myPath = new GraphicsPath();
            myPath.StartFigure();

            //绘制左上角弧线
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Y), circleSize), 180, 90);
            //绘制上边直线
            myPath.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.Right - radius, rect.Y));

            //或者有上角弧线
            myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * radius + (int)pen.Width / 2, rect.Y), circleSize), 270, 90);
            //绘制右边直线
            myPath.AddLine(new Point(rect.Right + (int)pen.Width, rect.Y + radius), new Point(rect.Right + (int)pen.Width, rect.Bottom - radius));

            //绘制右下角弧线
            myPath.AddArc(new Rectangle(new Point(rect.Right - radius * 2 + (int)pen.Width / 2, rect.Bottom - radius * 2), circleSize), 0, 90);
            //绘制下边直线
            myPath.AddLine(new Point(rect.Right - radius + (int)pen.Width / 2, rect.Bottom + (int)pen.Width / 2), new Point(rect.X + radius, rect.Bottom + (int)pen.Width / 2));

            //绘制左下角弧线
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - radius * 2 + (int)pen.Width / 2), circleSize), 90, 90);
            //绘制左边直线
            myPath.AddLine(new Point(rect.X, rect.Bottom - radius + (int)pen.Width / 2), new Point(rect.X, rect.Y + radius));

            myPath.CloseFigure();
            g.DrawPath(pen, myPath);
            g.FillPath(new SolidBrush(color), myPath);
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public static void FillRoundedRectangle(this Graphics g, Color color, Rectangle rect, int radius = 0)
        {
            if (g == null || rect == null || radius < 0)
                return;
            // 圆角半径
            //int radius = radius;
            if (radius == 0)
            {
                radius = Math.Min(rect.Width, rect.Height) / 3;
            }
            var circleSize = new Size(radius * 2, radius * 2);
            // 指定图形路径， 有一系列 直线/曲线 组成
            GraphicsPath myPath = new GraphicsPath();
            myPath.StartFigure();

            //绘制左上角弧线
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Y), circleSize), 180, 90);
            //绘制上边直线
            myPath.AddLine(new Point(rect.X + radius, rect.Y), new Point(rect.Right - radius, rect.Y));

            //或者有上角弧线
            myPath.AddArc(new Rectangle(new Point(rect.Right - 2 * radius, rect.Y), circleSize), 270, 90);
            //绘制右边直线
            myPath.AddLine(new Point(rect.Right, rect.Y + radius), new Point(rect.Right, rect.Bottom - radius));

            //绘制右下角弧线
            myPath.AddArc(new Rectangle(new Point(rect.Right - radius * 2, rect.Bottom - radius * 2), circleSize), 0, 90);
            //绘制下边直线
            myPath.AddLine(new Point(rect.Right - radius, rect.Bottom), new Point(rect.X + radius, rect.Bottom));

            //绘制左下角弧线
            myPath.AddArc(new Rectangle(new Point(rect.X, rect.Bottom - radius * 2), circleSize), 90, 90);
            //绘制左边直线
            myPath.AddLine(new Point(rect.X, rect.Bottom - radius), new Point(rect.X, rect.Y + radius));
            myPath.CloseFigure();
            using (var brush = new SolidBrush(color))
            {
                g.FillPath(brush, myPath);
            }
        }

        #endregion 绘制圆角矩形

        #region 绘制坐标轴

        /// <summary>
        /// 绘制XY坐标轴
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        /// <param name="lCount"></param>
        /// <param name="sCount"></param>
        /// <param name="ly"></param>
        /// <param name="sy"></param>
        /// <param name="fTop"></param>
        /// <param name="axisFont"></param>
        /// <param name="bReverse"></param>
        /// <param name="labels"></param>
        public static void DrawAxisXY(this Graphics g, Color color, Rectangle rect, int lCount = 10, int sCount = 2, int ly = 5, int sy = 3, int fTop = 2, Font axisFont = null, bool bReverse = false, string[] labels = null)
        {
            DrawAxisX(g, color, rect, lCount, sCount, ly, sy, fTop, axisFont, bReverse, labels);
            DrawAxisY(g, color, rect, lCount, sCount, ly, sy, fTop, axisFont, bReverse, labels);
        }

        /// <summary>
        /// 绘制水平坐标轴
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">坐标轴颜色</param>
        /// <param name="rect">绘图区域</param>
        /// <param name="lCount">主刻度数量</param>
        /// <param name="sCount">子刻度数量</param>
        /// <param name="ly">主刻度长度</param>
        /// <param name="sy">子刻度长度</param>
        /// <param name="fTop">字体离轴距离</param>
        /// <param name="axisFont">字体</param>
        /// <param name="bReverse">是否翻转</param>
        /// <param name="labels">坐标刻度文字</param>
        public static void DrawAxisX(this Graphics g, Color color, Rectangle rect, int lCount = 10, int sCount = 2, int ly = 5, int sy = 3, int fTop = 2, Font axisFont = null, bool bReverse = false, string[] labels = null)
        {
            //主刻度数量
            int longCount = lCount;
            //主刻度长度
            int longY = ly;
            //子刻度数量
            int shortCount = sCount;
            //子刻度长度
            int shortY = sy;
            //字体
            var font = axisFont ?? new Font("微软雅黑", 12f);
            //字体离轴距离
            int fontTop = fTop;
            //是否翻转
            bool reverse = bReverse;
            //坐标轴颜色
            Color axisColor = color;
            longCount = longCount <= 0 ? 1 : longCount;
            shortCount = shortCount <= 0 ? 1 : shortCount;
            bool isDiyStr = (labels != null && labels.Length >= lCount + 1);
            string[] axisLabels = new string[lCount + 1];
            for (int i = 0; i < lCount + 1; i++)
            {
                axisLabels[i] = isDiyStr ? labels[i] : i.ToString();
            }
            //是否翻转
            if (reverse)
            {
                longY = -longY;
                shortY = -shortY;
                fontTop = -fontTop;
            }
            var stepLen = (float)rect.Width / longCount;
            var miniStepLen = stepLen / shortCount;
            float stax = rect.X;
            float stay = rect.Y + rect.Height / 2;
            var pen = new Pen(axisColor);
            //设置抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawRectangle(Pens.Gray, rect);
            //1.绘制横线
            var endx = stax + rect.Width;
            var endy = stay;
            g.DrawLine(pen, stax, stay, endx, endy);
            //2.绘制小竖线
            float xx, yy;
            for (int i = 0; i <= longCount; i++)
            {
                //绘制长竖线
                xx = stax;
                yy = stay - longY;
                g.DrawLine(pen, stax, stay, xx, yy);
                //绘制文字
                var str = axisLabels[i];
                var fsize = g.MeasureString(str, font);
                var fx = stax - fsize.Width / 2;
                var fy = stay - fontTop;
                if (reverse)
                    fy -= fsize.Height;
                g.DrawString(str, font, Brushes.Black, fx, fy);
                if (i == longCount)
                    continue;
                //绘制短竖线
                float x1, y1, y2;
                x1 = stax;
                y1 = stay;
                y2 = stay - shortY;
                for (int j = 0; j < shortCount; j++)
                {
                    x1 += miniStepLen;
                    g.DrawLine(pen, x1, y1, x1, y2);
                }
                stax += stepLen;
            }
            pen.Dispose();
            if (axisFont == null)
                font.Dispose();
        }

        /// <summary>
        /// 绘制垂直坐标轴
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">坐标轴颜色</param>
        /// <param name="rect">绘图区域</param>
        /// <param name="lCount">主刻度数量</param>
        /// <param name="sCount">子刻度数量</param>
        /// <param name="ly">主刻度长度</param>
        /// <param name="sy">子刻度长度</param>
        /// <param name="fTop">字体离轴距离</param>
        /// <param name="axisFont">字体</param>
        /// <param name="bReverse">是否翻转</param>
        /// <param name="labels">坐标刻度文字</param>
        public static void DrawAxisY(this Graphics g, Color color, Rectangle rect, int lCount = 10, int sCount = 2, int ly = 5, int sy = 3, int fTop = 2, Font axisFont = null, bool bReverse = false, string[] labels = null)
        {
            //主刻度数量
            int longCount = lCount;
            //主刻度长度
            int longY = ly;
            //子刻度数量
            int shortCount = sCount;
            //子刻度长度
            int shortY = sy;
            //字体
            var font = axisFont ?? new Font("微软雅黑", 12f);
            //字体离轴距离
            int fontTop = fTop;
            //是否翻转
            bool reverse = bReverse;
            //坐标轴颜色
            Color axisColor = color;
            longCount = longCount <= 0 ? 1 : longCount;
            shortCount = shortCount <= 0 ? 1 : shortCount;
            bool isDiyStr = (labels != null && labels.Length >= lCount + 1);
            string[] axisLabels = new string[lCount + 1];
            for (int i = 0; i < lCount + 1; i++)
            {
                axisLabels[i] = isDiyStr ? labels[i] : i.ToString();
            }
            //是否翻转
            if (reverse)
            {
                longY = -longY;
                shortY = -shortY;
                fontTop = -fontTop;
            }
            var stepLen = (float)rect.Height / longCount;
            var miniStepLen = stepLen / shortCount;
            float stax = rect.X + rect.Width / 2;
            float stay = rect.Y;
            var pen = new Pen(axisColor);
            //设置抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawRectangle(Pens.Gray, rect);
            //1.绘制竖线
            var endx = stax;
            var endy = stay + rect.Height;
            g.DrawLine(pen, stax, stay, endx, endy);
            //2.绘制坐标标记
            float xx, yy;
            for (int i = 0; i <= longCount; i++)
            {
                //绘制长横线
                xx = stax + longY;
                yy = stay;
                g.DrawLine(pen, stax, stay, xx, yy);
                //绘制文字
                var str = axisLabels[i];
                var fsize = g.MeasureString(str, font);
                var fx = stax - fontTop;
                var fy = stay - fsize.Height / 2;
                if (!reverse)
                    fx -= fsize.Width;
                g.DrawString(str, font, Brushes.Black, fx, fy);
                if (i == longCount)
                    continue;
                //绘制短横线
                float x1, y1, x2;
                x1 = stax;
                y1 = stay;
                x2 = stax + shortY;
                for (int j = 0; j < shortCount; j++)
                {
                    y1 += miniStepLen;
                    g.DrawLine(pen, x1, y1, x2, y1);
                }
                stay += stepLen;
            }
            pen.Dispose();
            if (axisFont == null)
                font.Dispose();
        }

        #endregion 绘制坐标轴
    }

    public class ZForm : Form
    {
        public ZForm()
        {
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
    }
}