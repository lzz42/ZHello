using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
    }
}