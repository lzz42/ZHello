namespace ZHello.GDI
{
    public class ColorAlgorithm
    {
        public static int[] ArgbToGray(int[] argb)
        {
            //基础公式：Gray = R*0.299 + G*0.587 + B*0.114
            //Gray = (R*299 + G*587 + B*114 + 500) / 1000
            //改进公式：Gray = (R*19595 + G*38469 + B*7472) >> 16
            if (argb == null)
                return null;
            int gray = 0;
            int alpha = 0;
            if (argb.Length == 3)
            {
                gray = (argb[0] * 19595 + argb[1] * 38469 + argb[2] * 7472) >> 16;
            }
            else if (argb.Length == 4)
            {
                alpha = argb[0];
                gray = (argb[1] * 19595 + argb[2] * 38469 + argb[3] * 7472) >> 16;
            }
            gray = gray > 255 ? 255 : gray;
            return new int[] { alpha, gray, gray, gray };
        }

        //public static int[] GrayToArgb(int[] gray)
        //{
        //    if (gray == null)
        //        return null;
        //    int r = 0, g = 0, b = 0;
        //    int alpha = 0;
        //    if (gray.Length == 3)
        //    {
        //        gray = (argb[0] * 19595 + argb[1] * 38469 + argb[2] * 7472) >> 16;
        //    }
        //    else if (gray.Length == 4)
        //    {
        //        alpha = gray[0];
        //        gray = (argb[1] * 19595 + argb[2] * 38469 + argb[3] * 7472) >> 16;
        //    }
        //    gray = gray > 255 ? 255 : gray;
        //    return new int[] { alpha, gray, gray, gray };
        //}

        public static float[] ARGBToHSB(int[] argb)
        {
            return new float[] { 0f, 0f, 0f, 0f };
        }

        public static int[] HSBToARGB(float[] argb)
        {
            return new int[] { 0, 0, 0, 0 };
        }
    }
}