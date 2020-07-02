using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EInkLib
{
    public partial class EInkPainter
    {
        public static void DrawToBitmap(Bitmap bitmap)
        {
            ushort minX = 0;
            ushort maxX = 0;
            ushort minY = 0;
            ushort maxY = 0;
            if (eInkGetUpdateRectangle2(ref minX, ref maxX, ref minY, ref maxY, true))
            {
                for (ushort y = minY; y < maxY; y++)
                {
                    for (ushort x = minX; x < maxX; x++)
                    {
                        byte value;
                        eInkGetBuffPoint(x, y, out value);
                        if (eInkBpp != 8)
                        {
                            if (eInkBpp == 4)
                            {
                                value <<= 4;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                            //value = (byte)(~value);
                            bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
                        }
                    }
                }
            }
            //eInkUpdate()
        }
    }
}
