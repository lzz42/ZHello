using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.GDI.UI
{
    /// <summary>
    /// 坐标轴类型
    /// </summary>
    public enum AxisType
    {
        AxisX = 1,
        AxisY = 2,
        AxisXY = 3,
        AxisZ = 4,
        AxisXYZ = 7,
        Polor = 8,
    }

    /// <summary>
    /// 平面直角坐标轴渲染器
    /// </summary>
    public class AxisRenderer
    {
        public AxisRenderer()
        {
            Bounds = Rectangle.Empty;
            AxisColor = Color.Black;
            AxisTextFont = new Font("微软雅黑", 14f);
            AxixTextDistance = 2;
            MainScaleCount = 10;
            SubScaleCount = 10;
            MainScaleHight = 6;
            SubScaleHight = 3;
            IsReverseAxisX = false;
            IsReverseAxisY = false;
            AxisType = AxisType.AxisX;
            Visible = true;
            AxisXLables = new string[MainScaleCount + 1];
            AxisYLables = new string[MainScaleCount + 1];
            for (int i = 0; i < MainScaleCount + 1; i++)
            {
                AxisXLables[i] = i.ToString();
                AxisYLables[i] = i.ToString();
            }
        }

        public Rectangle Bounds { get; set; }
        public bool Visible { get; set; }
        public Color AxisColor { get; set; }
        public Font AxisTextFont { get; set; }
        public int AxixTextDistance { get; set; }
        public int MainScaleCount { get; set; }
        public int SubScaleCount { get; set; }
        public int MainScaleHight { get; set; }
        public int SubScaleHight { get; set; }
        public bool IsReverseAxisX { get; set; }
        public bool IsReverseAxisY { get; set; }
        public AxisType AxisType { get; set; }
        public string[] AxisXLables { get; set; }
        public string[] AxisYLables { get; set; }

        public virtual void Render(Graphics g)
        {
            if (!Visible)
                return;
            if (Bounds.IsEmpty)
                return;
            try
            {
                switch (AxisType)
                {
                    case AxisType.AxisX:
                        DrawAxisX(g, AxisColor, Bounds, MainScaleCount, SubScaleCount, MainScaleHight, SubScaleHight, AxixTextDistance, AxisTextFont, IsReverseAxisX, AxisXLables);
                        break;
                    case AxisType.AxisY:
                        DrawAxisY(g, AxisColor, Bounds, MainScaleCount, SubScaleCount, MainScaleHight, SubScaleHight, AxixTextDistance, AxisTextFont, IsReverseAxisY, AxisYLables);
                        break;
                    case AxisType.AxisXY:
                        DrawAxisX(g, AxisColor, Bounds, MainScaleCount, SubScaleCount, MainScaleHight, SubScaleHight, AxixTextDistance, AxisTextFont, IsReverseAxisX, AxisXLables);
                        DrawAxisY(g, AxisColor, Bounds, MainScaleCount, SubScaleCount, MainScaleHight, SubScaleHight, AxixTextDistance, AxisTextFont, IsReverseAxisY, AxisYLables);
                        break;
                    case AxisType.Polor:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                g.DrawString(ex.Message, AxisTextFont, Brushes.Red, 10, 10);
                g.DrawString(ex.StackTrace, AxisTextFont, Brushes.Red, 10, 10 + AxisTextFont.Height);
            }
        }

        public static void DrawAxisXY(Graphics g, Color color, Rectangle rect, int lCount = 10, int sCount = 2, int ly = 5, int sy = 3, int fTop = 2, Font axisFont = null, bool bReverse = false, string[] labels = null)
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
        public static void DrawAxisX(Graphics g, Color color, Rectangle rect, int lCount = 10, int sCount = 2, int ly = 5, int sy = 3, int fTop = 2, Font axisFont = null, bool bReverse = false, string[] labels = null)
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
        public static void DrawAxisY(Graphics g, Color color, Rectangle rect, int lCount = 10, int sCount = 2, int ly = 5, int sy = 3, int fTop = 2, Font axisFont = null, bool bReverse = false, string[] labels = null)
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
    }
}
