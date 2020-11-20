using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.GDI
{
    /*
     https://docs.microsoft.com/zh-cn/dotnet/api/system.drawing.drawing2d.matrix?view=netframework-4.0&f1url=%3FappId%3DDev16IDEF1%26l%3DZH-CN%26k%3Dk(System.Drawing.Drawing2D.Matrix);k(TargetFrameworkMoniker-.NETFramework,Version%253Dv4.0);k(DevLang-csharp)%26rd%3Dtrue
    Matrix:几何变换的 3x3 仿射矩阵
    仿射转换的矩阵的第三列始终 (0，0，1) ，因此，在构造对象时，只应在前两列中指定六个数字 Matrix,eg:var matrix = new Matrix(12,3,4,5,6)
    将构造如下仿射矩阵
    |1,2,0|
    |3,4,0|
    |5,6,1|
    复合转换：矩阵A,B,C,数据X,D=A*B*C
    X*A*B*C=X*D

     */
    public class UseMatrix
    {
        public static Image RotateAt(Image img, float angle, PointF point)
        {
            int w = img.Width;
            int h = img.Height;
            double nW = Math.Ceiling(w * Math.Abs(Math.Cos(angle)) + h * Math.Abs(Math.Sin(angle)));
            double nH = Math.Ceiling(w * Math.Abs(Math.Sin(angle)) + h * Math.Abs(Math.Cos(angle)));
            var img2 = new Bitmap((int)nW, (int)nH);
            using (Graphics g = Graphics.FromImage(img2))
            {
                //在中心位置绘制图形
                var matrix = g.Transform;
                //采用矩阵旋转
                matrix.RotateAt(angle, point);
                g.Transform = matrix;
                g.DrawImage(img, new Point((int)(nW - w) / 2, (int)(nH - h) / 2));
                string str = string.Format("Angle:{0},PointF:{1},{2}", angle, point.X, point.Y);
                using (var f = new Font("微软雅黑", 12f))
                {
                    g.DrawString(str, f, Brushes.Red, new PointF(2f, 2f));
                }
            }
            return img2;
        }
    }
}
