using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ZHello.GDI
{
    internal class ImageConvert
    {
        /// <summary>
        /// 获取颜色替换后的图片
        /// 仅替换指定颜色，其他颜色不变
        /// </summary>
        /// <param name="img"></param>
        /// <param name="lColor">要替换的目标颜色</param>
        /// <param name="rColor">替换颜色</param>
        /// <returns></returns>
        private static Image GetColorReplaceImgage(Image img, Color lColor, Color rColor)
        {
            var map = new Bitmap(img);
            Bitmap g = new Bitmap(map.Width, map.Height);
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    Color c = map.GetPixel(i, j);
                    if (c.A == lColor.A && c.R == lColor.R && c.G == lColor.G && c.B == lColor.B)
                    {
                        g.SetPixel(i, j, Color.FromArgb(c.A, rColor.R, rColor.G, rColor.B));
                    }
                }
            }
            return g;
        }

        /// <summary>
        /// 获取颜色替换后的反色的图片
        /// 1.将指定的原图片颜色替换为透明白色
        /// 2.将其他颜色替换为指定颜色
        /// </summary>
        /// <param name="img"></param>
        /// <param name="lColor">要替换的目标颜色</param>
        /// <param name="rColor">替换颜色</param>
        /// <returns></returns>
        private static Image GetColorReplaceInvertedImage(Image img, Color lColor, Color rColor)
        {
            var map = new Bitmap(img);
            Bitmap g = new Bitmap(map.Width, map.Height);
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    Color c = map.GetPixel(i, j);
                    if (c.A == lColor.A && c.R == lColor.R && c.G == lColor.G && c.B == lColor.B)
                    {
                        g.SetPixel(i, j, Color.FromArgb(c.A, Color.White.R, Color.White.G, Color.White.B));
                    }
                    else
                    {
                        g.SetPixel(i, j, rColor);
                    }
                }
            }
            return g;
        }

        /// <summary>
        /// 图像旋转
        /// </summary>
        /// <param name="srcBmp">原始图像</param>
        /// <param name="degree">旋转角度</param>
        /// <param name="dstBmp">目标图像</param>
        /// <returns>处理成功 true 失败 false</returns>
        public static bool Rotation(Bitmap srcBmp, double degree, out Bitmap dstBmp)
        {
            if (srcBmp == null)
            {
                dstBmp = null;
                return false;
            }
            dstBmp = null;
            BitmapData srcBmpData = null;
            BitmapData dstBmpData = null;
            switch ((int)degree)
            {
                case 0:
                    dstBmp = new Bitmap(srcBmp);
                    break;

                case -90:
                    dstBmp = new Bitmap(srcBmp.Height, srcBmp.Width);
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < srcBmp.Height; i++)
                        {
                            for (int j = 0; j < srcBmp.Width; j++)
                            {
                                ptrDst[j * dstBmpData.Stride + (dstBmp.Height - i - 1) * 3] = ptrSrc[i * srcBmpData.Stride + j * 3];
                                ptrDst[j * dstBmpData.Stride + (dstBmp.Height - i - 1) * 3 + 1] = ptrSrc[i * srcBmpData.Stride + j * 3 + 1];
                                ptrDst[j * dstBmpData.Stride + (dstBmp.Height - i - 1) * 3 + 2] = ptrSrc[i * srcBmpData.Stride + j * 3 + 2];
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;

                case 90:
                    dstBmp = new Bitmap(srcBmp.Height, srcBmp.Width);
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < srcBmp.Height; i++)
                        {
                            for (int j = 0; j < srcBmp.Width; j++)
                            {
                                ptrDst[(srcBmp.Width - j - 1) * dstBmpData.Stride + i * 3] = ptrSrc[i * srcBmpData.Stride + j * 3];
                                ptrDst[(srcBmp.Width - j - 1) * dstBmpData.Stride + i * 3 + 1] = ptrSrc[i * srcBmpData.Stride + j * 3 + 1];
                                ptrDst[(srcBmp.Width - j - 1) * dstBmpData.Stride + i * 3 + 2] = ptrSrc[i * srcBmpData.Stride + j * 3 + 2];
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;

                case 180:
                case -180:
                    dstBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < srcBmp.Height; i++)
                        {
                            for (int j = 0; j < srcBmp.Width; j++)
                            {
                                ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3] = ptrSrc[i * srcBmpData.Stride + j * 3];
                                ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3 + 1] = ptrSrc[i * srcBmpData.Stride + j * 3 + 1];
                                ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3 + 2] = ptrSrc[i * srcBmpData.Stride + j * 3 + 2];
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;

                default://任意角度
                    double radian = degree * Math.PI / 180.0;//将角度转换为弧度
                                                             //计算正弦和余弦
                    double sin = Math.Sin(radian);
                    double cos = Math.Cos(radian);
                    //计算旋转后的图像大小
                    int widthDst = (int)(srcBmp.Height * Math.Abs(sin) + srcBmp.Width * Math.Abs(cos));
                    int heightDst = (int)(srcBmp.Width * Math.Abs(sin) + srcBmp.Height * Math.Abs(cos));

                    dstBmp = new Bitmap(widthDst, heightDst);
                    //确定旋转点
                    int dx = (int)(srcBmp.Width / 2 * (1 - cos) + srcBmp.Height / 2 * sin);
                    int dy = (int)(srcBmp.Width / 2 * (0 - sin) + srcBmp.Height / 2 * (1 - cos));

                    int insertBeginX = srcBmp.Width / 2 - widthDst / 2;
                    int insertBeginY = srcBmp.Height / 2 - heightDst / 2;

                    //插值公式所需参数
                    double ku = insertBeginX * cos - insertBeginY * sin + dx;
                    double kv = insertBeginX * sin + insertBeginY * cos + dy;
                    double cu1 = cos, cu2 = sin;
                    double cv1 = sin, cv2 = cos;

                    double fu, fv, a, b, F1, F2;
                    int Iu, Iv;
                    srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    unsafe
                    {
                        byte* ptrSrc = (byte*)srcBmpData.Scan0;
                        byte* ptrDst = (byte*)dstBmpData.Scan0;
                        for (int i = 0; i < heightDst; i++)
                        {
                            for (int j = 0; j < widthDst; j++)
                            {
                                fu = j * cu1 - i * cu2 + ku;
                                fv = j * cv1 + i * cv2 + kv;
                                if ((fv < 1) || (fv > srcBmp.Height - 1) || (fu < 1) || (fu > srcBmp.Width - 1))
                                {
                                    ptrDst[i * dstBmpData.Stride + j * 3] = 150;
                                    ptrDst[i * dstBmpData.Stride + j * 3 + 1] = 150;
                                    ptrDst[i * dstBmpData.Stride + j * 3 + 2] = 150;
                                }
                                else
                                {//双线性插值
                                    Iu = (int)fu;
                                    Iv = (int)fv;
                                    a = fu - Iu;
                                    b = fv - Iv;
                                    for (int k = 0; k < 3; k++)
                                    {
                                        F1 = (1 - b) * *(ptrSrc + Iv * srcBmpData.Stride + Iu * 3 + k) + b * *(ptrSrc + (Iv + 1) * srcBmpData.Stride + Iu * 3 + k);
                                        F2 = (1 - b) * *(ptrSrc + Iv * srcBmpData.Stride + (Iu + 1) * 3 + k) + b * *(ptrSrc + (Iv + 1) * srcBmpData.Stride + (Iu + 1) * 3 + k);
                                        *(ptrDst + i * dstBmpData.Stride + j * 3 + k) = (byte)((1 - a) * F1 + a * F2);
                                    }
                                }
                            }
                        }
                    }
                    srcBmp.UnlockBits(srcBmpData);
                    dstBmp.UnlockBits(dstBmpData);
                    break;
            }
            return true;
        }

        public enum ZoomType
        {
            NearestNeighborInterpolation,
            BilinearInterpolation
        }

        /// <summary>
        /// 图像缩放
        /// </summary>
        /// <param name="srcBmp">原始图像</param>
        /// <param name="width">目标图像宽度</param>
        /// <param name="height">目标图像高度</param>
        /// <param name="dstBmp">目标图像</param>
        /// <param name="GetNearOrBil">缩放选用的算法</param>
        /// <returns>处理成功 true 失败 false</returns>
        public static bool Zoom(Bitmap srcBmp, double ratioW, double ratioH, out Bitmap dstBmp, ZoomType zoomType)
        {//ZoomType为自定义的枚举类型
            if (srcBmp == null)
            {
                dstBmp = null;
                return false;
            }
            //若缩放大小与原图一样，则返回原图不做处理
            if ((ratioW == 1.0) && ratioH == 1.0)
            {
                dstBmp = new Bitmap(srcBmp);
                return true;
            }
            //计算缩放高宽
            double height = ratioH * (double)srcBmp.Height;
            double width = ratioW * (double)srcBmp.Width;
            dstBmp = new Bitmap((int)width, (int)height);

            BitmapData srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = null;
                byte* dstPtr = null;
                int srcI = 0;
                int srcJ = 0;
                double srcdI = 0;
                double srcdJ = 0;
                double a = 0;
                double b = 0;
                double F1 = 0;//横向插值所得数值
                double F2 = 0;//纵向插值所得数值
                if (zoomType == ZoomType.NearestNeighborInterpolation)
                {//邻近插值法
                    for (int i = 0; i < dstBmp.Height; i++)
                    {
                        srcI = (int)(i / ratioH);//srcI是此时的i对应的原图像的高
                        srcPtr = (byte*)srcBmpData.Scan0 + srcI * srcBmpData.Stride;
                        dstPtr = (byte*)dstBmpData.Scan0 + i * dstBmpData.Stride;
                        for (int j = 0; j < dstBmp.Width; j++)
                        {
                            dstPtr[j * 3] = srcPtr[(int)(j / ratioW) * 3];//j / ratioW求出此时j对应的原图像的宽
                            dstPtr[j * 3 + 1] = srcPtr[(int)(j / ratioW) * 3 + 1];
                            dstPtr[j * 3 + 2] = srcPtr[(int)(j / ratioW) * 3 + 2];
                        }
                    }
                }
                else if (zoomType == ZoomType.BilinearInterpolation)
                {//双线性插值法
                    byte* srcPtrNext = null;
                    for (int i = 0; i < dstBmp.Height; i++)
                    {
                        srcdI = i / ratioH;
                        srcI = (int)srcdI;//当前行对应原始图像的行数
                        srcPtr = (byte*)srcBmpData.Scan0 + srcI * srcBmpData.Stride;//指原始图像的当前行
                        srcPtrNext = (byte*)srcBmpData.Scan0 + (srcI + 1) * srcBmpData.Stride;//指向原始图像的下一行
                        dstPtr = (byte*)dstBmpData.Scan0 + i * dstBmpData.Stride;//指向当前图像的当前行
                        for (int j = 0; j < dstBmp.Width; j++)
                        {
                            srcdJ = j / ratioW;
                            srcJ = (int)srcdJ;//指向原始图像的列
                            if (srcdJ < 1 || srcdJ > srcBmp.Width - 1 || srcdI < 1 || srcdI > srcBmp.Height - 1)
                            {//避免溢出（也可使用循环延拓）
                                dstPtr[j * 3] = 255;
                                dstPtr[j * 3 + 1] = 255;
                                dstPtr[j * 3 + 2] = 255;
                                continue;
                            }
                            a = srcdI - srcI;//计算插入的像素与原始像素距离（决定相邻像素的灰度所占的比例）
                            b = srcdJ - srcJ;
                            for (int k = 0; k < 3; k++)
                            {//插值    公式：f(i+p,j+q)=(1-p)(1-q)f(i,j)+(1-p)qf(i,j+1)+p(1-q)f(i+1,j)+pqf(i+1, j + 1)
                                F1 = (1 - b) * srcPtr[srcJ * 3 + k] + b * srcPtr[(srcJ + 1) * 3 + k];
                                F2 = (1 - b) * srcPtrNext[srcJ * 3 + k] + b * srcPtrNext[(srcJ + 1) * 3 + k];
                                dstPtr[j * 3 + k] = (byte)((1 - a) * F1 + a * F2);
                            }
                        }
                    }
                }
            }
            srcBmp.UnlockBits(srcBmpData);
            dstBmp.UnlockBits(dstBmpData);
            return true;
        }

        ///// <summary>
        ///// 获取颜色替换后的图片
        ///// 仅替换指定颜色，其他颜色不变
        ///// </summary>
        ///// <param name="img"></param>
        ///// <param name="lColor">原图片颜色</param>
        ///// <param name="rColor">新图片颜色</param>
        ///// <returns></returns>
        //internal static Image GetColorReplaceImgage(Image img, Color lColor, Color rColor)
        //{
        //    var map = new Bitmap(img);
        //    Bitmap g = new Bitmap(map.Width, map.Height);
        //    for (int i = 0; i < map.Width; i++)
        //    {
        //        for (int j = 0; j < map.Height; j++)
        //        {
        //            Color c = map.GetPixel(i, j);
        //            if (c.A == lColor.A && c.R == lColor.R && c.G == lColor.G && c.B == lColor.B)
        //            {
        //                g.SetPixel(i, j, Color.FromArgb(c.A, rColor.R, rColor.G, rColor.B));
        //            }
        //        }
        //    }
        //    return g;
        //}

        ///// <summary>
        ///// 获取颜色替换后的反色的图片
        ///// 1.将指定的原图片颜色替换为透明白色
        ///// 2.将其他颜色替换为指定颜色
        ///// </summary>
        ///// <param name="img">指定的原图片</param>
        ///// <param name="lColor">原图片颜色</param>
        ///// <param name="rColor">新图片颜色</param>
        ///// <returns></returns>
        //internal static Image GetColorReplaceInvertedImage(Image img, Color lColor, Color rColor)
        //{
        //    var map = new Bitmap(img);
        //    Bitmap g = new Bitmap(map.Width, map.Height);
        //    for (int i = 0; i < map.Width; i++)
        //    {
        //        for (int j = 0; j < map.Height; j++)
        //        {
        //            Color c = map.GetPixel(i, j);
        //            if (c.A == lColor.A && c.R == lColor.R && c.G == lColor.G && c.B == lColor.B)
        //            {
        //                g.SetPixel(i, j, Color.FromArgb(c.A, Color.White.R, Color.White.G, Color.White.B));
        //            }
        //            else
        //            {
        //                g.SetPixel(i, j, rColor);
        //            }
        //        }
        //    }
        //    return g;
        //}

        ///// <summary>
        ///// 图像旋转
        ///// </summary>
        ///// <param name="srcBmp">原始图像</param>
        ///// <param name="degree">旋转角度逆时针</param>
        ///// <param name="dstBmp">目标图像</param>
        ///// <returns>处理成功 true 失败 false</returns>
        //internal static bool Rotation(Bitmap srcBmp, double degree, out Bitmap dstBmp)
        //{
        //    if (srcBmp == null)
        //    {
        //        dstBmp = null;
        //        return false;
        //    }
        //    dstBmp = null;
        //    BitmapData srcBmpData = null;
        //    BitmapData dstBmpData = null;
        //    degree = degree % 360;
        //    degree = degree < 0 ? degree + 360d : degree;
        //    if (degree % 90 == 0)
        //    {
        //        switch ((int)degree)
        //        {
        //            case 0:
        //                dstBmp = new Bitmap(srcBmp);
        //                break;
        //            case 90:
        //            case 270:
        //                dstBmp = new Bitmap(srcBmp.Height, srcBmp.Width);
        //                srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        //                dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        //                unsafe
        //                {
        //                    byte* ptrSrc = (byte*)srcBmpData.Scan0;
        //                    byte* ptrDst = (byte*)dstBmpData.Scan0;
        //                    for (int i = 0; i < srcBmp.Height; i++)
        //                    {
        //                        for (int j = 0; j < srcBmp.Width; j++)
        //                        {
        //                            var p1 = j * dstBmpData.Stride + (dstBmp.Height - i - 1) * 3;
        //                            if ((int)degree == 270)
        //                            {
        //                                p1 = (srcBmp.Width - j - 1) * dstBmpData.Stride + i * 3;
        //                            }
        //                            var p2 = i * srcBmpData.Stride + j * 3;
        //                            ptrDst[p1] = ptrSrc[p2];
        //                            ptrDst[p1 + 1] = ptrSrc[p2 + 1];
        //                            ptrDst[p1 + 2] = ptrSrc[p2 + 2];
        //                        }
        //                    }
        //                }
        //                srcBmp.UnlockBits(srcBmpData);
        //                dstBmp.UnlockBits(dstBmpData);
        //                break;
        //            case 180:
        //                dstBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
        //                srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        //                dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        //                unsafe
        //                {
        //                    byte* ptrSrc = (byte*)srcBmpData.Scan0;
        //                    byte* ptrDst = (byte*)dstBmpData.Scan0;
        //                    for (int i = 0; i < srcBmp.Height; i++)
        //                    {
        //                        for (int j = 0; j < srcBmp.Width; j++)
        //                        {
        //                            ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3] = ptrSrc[i * srcBmpData.Stride + j * 3];
        //                            ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3 + 1] = ptrSrc[i * srcBmpData.Stride + j * 3 + 1];
        //                            ptrDst[(srcBmp.Width - i - 1) * dstBmpData.Stride + (dstBmp.Height - j - 1) * 3 + 2] = ptrSrc[i * srcBmpData.Stride + j * 3 + 2];
        //                        }
        //                    }
        //                }
        //                srcBmp.UnlockBits(srcBmpData);
        //                dstBmp.UnlockBits(dstBmpData);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        //任意角度
        //        //将角度转换为弧度
        //        double radian = degree * Math.PI / 180.0;
        //        //计算正弦和余弦
        //        double sin = Math.Sin(radian);
        //        double cos = Math.Cos(radian);
        //        //计算旋转后的图像大小
        //        int widthDst = (int)(srcBmp.Height * Math.Abs(sin) + srcBmp.Width * Math.Abs(cos));
        //        int heightDst = (int)(srcBmp.Width * Math.Abs(sin) + srcBmp.Height * Math.Abs(cos));
        //        dstBmp = new Bitmap(widthDst, heightDst);
        //        //确定旋转点
        //        int dx = (int)(srcBmp.Width / 2 * (1 - cos) + srcBmp.Height / 2 * sin);
        //        int dy = (int)(srcBmp.Width / 2 * (0 - sin) + srcBmp.Height / 2 * (1 - cos));
        //        int insertBeginX = srcBmp.Width / 2 - widthDst / 2;
        //        int insertBeginY = srcBmp.Height / 2 - heightDst / 2;
        //        //插值公式所需参数
        //        double ku = insertBeginX * cos - insertBeginY * sin + dx;
        //        double kv = insertBeginX * sin + insertBeginY * cos + dy;
        //        double cu1 = cos, cu2 = sin;
        //        double cv1 = sin, cv2 = cos;
        //        double fu, fv, a, b, F1, F2;
        //        int Iu, Iv;
        //        srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        //        dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        //        unsafe
        //        {
        //            byte* ptrSrc = (byte*)srcBmpData.Scan0;
        //            byte* ptrDst = (byte*)dstBmpData.Scan0;
        //            for (int i = 0; i < heightDst; i++)
        //            {
        //                for (int j = 0; j < widthDst; j++)
        //                {
        //                    fu = j * cu1 - i * cu2 + ku;
        //                    fv = j * cv1 + i * cv2 + kv;
        //                    if ((fv < 1) || (fv > srcBmp.Height - 1) || (fu < 1) || (fu > srcBmp.Width - 1))
        //                    {
        //                        ptrDst[i * dstBmpData.Stride + j * 3] = 150;
        //                        ptrDst[i * dstBmpData.Stride + j * 3 + 1] = 150;
        //                        ptrDst[i * dstBmpData.Stride + j * 3 + 2] = 150;
        //                    }
        //                    else
        //                    {//双线性插值
        //                        Iu = (int)fu;
        //                        Iv = (int)fv;
        //                        a = fu - Iu;
        //                        b = fv - Iv;
        //                        for (int k = 0; k < 3; k++)
        //                        {
        //                            F1 = (1 - b) * *(ptrSrc + Iv * srcBmpData.Stride + Iu * 3 + k) + b * *(ptrSrc + (Iv + 1) * srcBmpData.Stride + Iu * 3 + k);
        //                            F2 = (1 - b) * *(ptrSrc + Iv * srcBmpData.Stride + (Iu + 1) * 3 + k) + b * *(ptrSrc + (Iv + 1) * srcBmpData.Stride + (Iu + 1) * 3 + k);
        //                            *(ptrDst + i * dstBmpData.Stride + j * 3 + k) = (byte)((1 - a) * F1 + a * F2);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        srcBmp.UnlockBits(srcBmpData);
        //        dstBmp.UnlockBits(dstBmpData);
        //    }
        //    return true;
        //}

        /// <summary>
        /// 顺时针旋转指定角度
        /// </summary>
        /// <param name="img"></param>
        /// <param name="degree">顺时针角度</param>
        /// <returns></returns>
        internal static Image GetRotateImage(Image img, int degree)
        {
            if (img == null)
                return null;
            degree = degree % 360;
            degree = degree < 0 ? degree + 360 : degree;
            Bitmap outmap = null;
            switch (degree)
            {
                case 0:
                    outmap = new Bitmap(img);
                    break;

                case 90:
                    outmap = new Bitmap(img);
                    outmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;

                case 180:
                    outmap = new Bitmap(img);
                    outmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;

                case 270:
                    outmap = new Bitmap(img);
                    outmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;

                default:
                    if (!Rotation(new Bitmap(img), -degree, out outmap))
                    {
                        outmap = null;
                    }
                    break;
            }
            return outmap;
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
        public static void DrawRoundedRectangle(Graphics g, Pen pen, Rectangle rect, int radius, Color color)
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
        public static void FillRoundedRectangle(Graphics g, Color color, Rectangle rect, int radius = 0)
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