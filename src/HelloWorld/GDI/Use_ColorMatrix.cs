using System.Drawing;
using System.Drawing.Imaging;

namespace ZHello.GDI
{
    public class Use_ColorMatrix
    {
        public void Example_TransfromImageColor2()
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form
            {
                Text = "ColorMatrix Test",
                Size = new Size(500, 300),
                Location = new Point(100, 100)
            };
            form.Paint += (s, e) =>
            {
                e.Graphics.DrawString("OKOKOKO", new Font("微软雅黑", 24), Brushes.Black, new Point(20, 20));
            };
            //var g = form.CreateGraphics();
            //g.DrawString("OKOKOKO", new Font("微软雅黑", 24), Brushes.Black, new Point(20, 20));
            form.ShowDialog();
        }

        public void Example_TransfromImageColor()
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form
            {
                Text = "ColorMatrix Test",
                Size = new Size(1200, 600),
                Location = new Point(100, 100)
            };
            /*
             1.初始化ColorMatrix对象；
             2.创建一个ImageAttributes对象，并将ColorMatrix对象传递给ImageAttributes对象的SetColorMatrix方法；
             3.传递ImageAttributes对象给Graphics对象的DrawImage方法
             */
            //获取一个有图像数据的Image对象
            System.IO.Stream sm = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("HelloWorld.Resources.ColorRing.png");
            Image img = Image.FromStream(sm);
            sm.Close();

            ImageAttributes imgAttrs = new ImageAttributes();

            float[][] colorMatrixElements = new float[][]
            {
                new float[]{0f, 0, 0, 0,0},//红色缩放值
                new float[]{0, 1f, 0, 0,0},//绿色色缩放值
                new float[]{0, 0, 1f, 0,0},//蓝色缩放值
                new float[]{0, 0, 0, 1,0},//透明度缩放值
                new float[]{0, 0, 0, 0,1},
            };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imgAttrs.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            form.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.DrawImage(img, 60, 20);
                g.DrawImage(img,
                    new Rectangle(img.Width + 70, 20, img.Width, img.Height),
                    0, 0,
                    img.Width,
                    img.Height,
                    GraphicsUnit.Pixel,
                    imgAttrs
                    );
            };
            form.ShowDialog();
        }
    }
}