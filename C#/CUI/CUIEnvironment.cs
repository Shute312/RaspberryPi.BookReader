using CGraphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public static class CUIEnvironment
    {
        public static byte BitsPerPixel;

        public static Int32 WidthOfPixel;//逻辑分辨率
        public static Int32 HeightOfPixel;

        public static float WidthOfInch;//物理尺寸
        public static float HeightOfInch;

        public static ScreenSize ScreenSize;
        public static Int32 FontSize;//默认字号

        public static Int32 MinFontSize = 12;

        public static void UpdateDefaultFontSize()
        {
            float heightOfInch = Math.Min(HeightOfInch,WidthOfInch);
            float line = 0;
            if (heightOfInch > 16)
            {
                line = (int)(heightOfInch/ 0.39);//10mm一行
            }
            else if (heightOfInch > 8)
            {
                line = (int)(heightOfInch / 0.35);//9mm一行
            }
            else if (heightOfInch > 4)
            {
                line = (int)(heightOfInch / 0.315);//8mm一行
            }
            else if (heightOfInch > 2)
            {
                line = (int)(heightOfInch / 0.236);//6mm一行
            }
            else
            {
                throw new NotImplementedException();
            }
            Int32 height = Math.Min(HeightOfPixel, WidthOfPixel);
            Int32 lineHeight = (Int32)(height / line);
            Int32 fontSize = lineHeight * 3 / 4;//字号
            if (fontSize < MinFontSize)
            {
                fontSize = MinFontSize;
            }
            CFontInfo fontInfo;
            CG.GetValidFontSize(fontSize, out fontInfo);
            FontSize = fontInfo.FontSize;
        }
    }
}