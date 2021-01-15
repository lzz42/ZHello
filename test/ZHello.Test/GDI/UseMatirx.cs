using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHello.Test.GDI
{
    [TestClass]
    public class UseMatirx
    {
        public static Image Img { get; set; }
        [TestInitialize]
        public void Init()
        {
            Img = Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "GDI","ColorRing.png"));
        }


        [TestMethod]
        public void Main()
        {
            var f = new Form()
            {
                Text = "UseMatrix",
                Size = new System.Drawing.Size(800, 600),
                Padding=new Padding(10),
            };
            var p = new Panel()
            {
                Dock = DockStyle.Left,
                Width = 700,
                BackgroundImage = Img,
                BackgroundImageLayout= ImageLayout.Center,
                BackColor= Color.LightGreen,
            };
            var num = new NumericUpDown()
            {
                Value=10,
                Dock = DockStyle.Top,
            };
            var btn = new Button()
            {
                Text = "旋转",
                Dock = DockStyle.Top,
            };
            var btn2 = new Button()
            {
                Text = "自动旋转36",
                Dock = DockStyle.Top,
            };
            btn.MouseClick += (s, e) =>
            {
                var h = Math.Min(Img.Width, Img.Height);
                h = h / 2;
                var pp = new Point(h, h);
                var img = ZHello.GDI.UseMatrix.RotateAt(Img, (int)num.Value, pp);
                p.BackgroundImage = img;
            };

            btn2.MouseClick += (s, e) =>
            {
                var v = (int)num.Value;
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(300);
                    var img = ZHello.GDI.UseMatrix.RotateAt(Img, v, new PointF(0f, 0f));
                    v += 10;
                    p.BackgroundImage = img;
                }
            };

            f.Controls.Add(num);
            f.Controls.Add(btn);
            f.Controls.Add(btn2);
            f.Controls.Add(p);
            f.ShowDialog();
        }
    }
}
