using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IDCM.Base.Utils
{
    public class ImageUtil
    {
        /// <summary>
        /// 图片半透明化实现
        /// </summary>
        /// <param name="baseImg"></param>
        /// <returns></returns>
        public static Bitmap setAlphaImage(Image baseImg)
        {
            Bitmap bmp = new Bitmap(baseImg); for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color bmpcolor = bmp.GetPixel(i, j);
                    byte A = bmpcolor.A;
                    byte R = bmpcolor.R;
                    byte G = bmpcolor.G;
                    byte B = bmpcolor.B;
                    bmpcolor = Color.FromArgb(128, R, G, B);
                    bmp.SetPixel(i, j, bmpcolor);
                }
            }
            return bmp;
        }
    }
}
