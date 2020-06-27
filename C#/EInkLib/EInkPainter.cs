using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EInkLib
{
    public partial class EInkPainter
    {
        private static byte[] eInkBuff;
        private static UInt32 eInkBufLength = 0;
        private static byte eInkBpp;
        private static ushort eInkWidth;
        private static ushort eInkHeight;
        private static ushort eInkDrawingMinX;//当前绘制的内容的范围，用于更新局部区域
        private static ushort eInkDrawingMaxX;
        private static ushort eInkDrawingMinY;
        private static ushort eInkDrawingMaxY;
        private static ushort eInkValidMinX;//锁定可以绘制的范围
        private static ushort eInkValidMaxX;
        private static ushort eInkValidMinY;
        private static ushort eInkValidMaxY;
        public static void eInkInit(in ushort width, in ushort height, in byte bitsPerPixel)
        {
            if (eInkBufLength > 0)
            {
                //回收对应内容
            }
            eInkBpp = bitsPerPixel;
            eInkBufLength = (UInt32)((width * height) * bitsPerPixel + 7) / 8;
            eInkBuff = new byte[eInkBufLength];
            eInkWidth = width;
            eInkHeight = height;
            eInkDrawingMinX = ushort.MaxValue;
            eInkDrawingMaxX = ushort.MinValue;
            eInkDrawingMinY = ushort.MaxValue;
            eInkDrawingMaxX = ushort.MinValue;
            eInkeUnlockValidArea();
        }

        /// <summary>
        /// 锁定有效渲染范围
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>超过最大范围时，会锁定失败</returns>
        public static bool eInkLockValidArea(in ushort x, in ushort y, in ushort width, in ushort height)
        {
            if (x < 0 || y < 0) return false;
            if (x + width > eInkWidth) return false;
            if (y + height > eInkHeight) return false;
            eInkValidMinX = x;
            eInkValidMinY = y;
            eInkValidMaxX = (ushort)(x + width);
            eInkValidMaxY = (ushort)(y + height);
            return true;
        }

        //
        public static void eInkeUnlockValidArea()
        {
            eInkValidMinX = 0;
            eInkValidMinY = 0;
            eInkValidMaxX = eInkWidth;
            eInkValidMaxY = eInkHeight;
        }

        public static bool eInkDrawHorLine(in ushort thickness, in ushort x, in ushort y, in ushort width, in EInkColor color)
        {
            Contract.Assert(thickness > 0);
            //ushort minY = (ushort)(y - (thickness - 1) >> 2);
            //ushort maxY = (ushort)(minY + thickness);
            //ushort minX = x;
            //ushort maxX = (ushort)(x + width);
            //return eInkFill(minX, maxX, minY, maxY, color);
            return eInkFill(x, (ushort)(x + width), y, (ushort)(y + thickness), color);
        }
        public static bool eInkDrawVerLine(in ushort thickness, in ushort x, in ushort y, in ushort height, in EInkColor color)
        {
            Contract.Assert(thickness > 0);
            //ushort minX = (ushort)(x - (thickness - 1) >> 2);
            //ushort maxX = (ushort)(minX + thickness);
            //ushort minY = y;
            //ushort maxY = (ushort)(y + height);
            //return eInkFill(minX, maxX, minY, maxY, color);
            return eInkFill(x, (ushort)(x + thickness), y, (ushort)(y + height), color);
        }

        public static bool eInkDrawRectangle(in ushort thickness, in ushort x1, in ushort y1, in ushort x2, in ushort y2, EInkColor color)
        {
            ushort minX = Math.Min(x1, x2);
            ushort maxX = Math.Max(x1, x2);
            ushort minY = Math.Min(y1, y2);
            ushort maxY = Math.Max(y1, y2);
            ushort width = (ushort)(maxX - minX);
            ushort height = (ushort)(maxY - minY);
            if (width == 0 || height == 0)
            {
                return false;
            }
            return eInkDrawHorLine(thickness, minX, minY, width, color) &
            eInkDrawHorLine(thickness, minX, (ushort)(maxY - thickness), width, color) &
            eInkDrawVerLine(thickness, minX, minY, height, color) &
            eInkDrawVerLine(thickness, (ushort)(maxX - thickness), minY, height, color);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="text">文本</param>
        ///// <param name="index">文本从哪个位置开始绘制</param>
        ///// <param name="length">要绘制的文本长度(剔除index的偏移量后的长度)</param>
        ///// <param name="width">容器宽度</param>
        ///// <param name="height">容器高度</param>
        ///// <returns>返回绘制的字符数</returns>
        //public static UInt32 eInkDrawString(in string text, in ushort index, in UInt32 length, in ushort fontSize,in ushort x,in ushort y,in FontMargin margin)
        //{
        //    FontInfo info;
        //    Fonts.ePageGetFontInfo(Fonts.DefaultSize, out info);

        //    //注意：设置负数的缩进，会有文本绘制不全的问题

        //    int startX = margin.PageMarginLeft + margin.MarginLeft;
        //    int x = startX + margin.ParagraphFirstLineMarginLeft;
        //    int y = margin.PageMarginTop + margin.MarginTop;
        //    FontBitmap matrix;
        //    int maxX = eInkValidMaxX - margin.PageMarginRight - margin.MarginRight - 1;
        //    int maxY = eInkValidMaxY - margin.PageMarginBottom - margin.LineMarginBottom - margin.MarginBottom - 1;
        //    int lineHeight = info.Height + margin.LineMarginTop + margin.LineMarginBottom + margin.MarginTop + margin.MarginBottom;
        //    //注意
        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        var character = text[i];
        //        //强制换行
        //        if (character == '\n' || (character == '\r' && i < text.Length - 1) && character == '\n')
        //        {
        //            x = startX + margin.ParagraphFirstLineMarginLeft;
        //            y += lineHeight;
        //            //跳过\n
        //            if (character == '\r')
        //            {
        //                i++;
        //            }
        //            continue;
        //        }

        //        if (Fonts.ePageGetFontMatrix(character, fontSize, out matrix))
        //        {
        //            int width = info.Width;
        //            //新起一行
        //            if (x + width > maxX)
        //            {
        //                x = startX;
        //                y += lineHeight;
        //            }
        //            if (y > maxY)
        //            {
        //                break;
        //            }
        //            Fonts.ePageDrawFontMatrix(bitmap, x, y, matrix, info);

        //            x += width + margin.MarginLeft + margin.MarginRight;
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="reset">是否重置，以便下次计算</param>
        /// <returns>是否需要更新</returns>
        public static bool eInkGetUpdateRectangle(ref ushort x, ref ushort y, ref ushort width, ref ushort height, in bool reset)
        {
            if (eInkDrawingMaxX <= eInkDrawingMinX || eInkDrawingMaxY <= eInkDrawingMinY)
            {
                return false;
            }
            x = eInkDrawingMinX;
            y = eInkDrawingMinY;
            width = (ushort)(eInkDrawingMaxX - eInkDrawingMinX);
            height = (ushort)(eInkDrawingMaxY - eInkDrawingMinY);

            if (((x * eInkBpp) & 0x8) != 0)
            {

            }

            if (reset)
            {
                eInkDrawingMinX = ushort.MaxValue;
                eInkDrawingMaxX = ushort.MinValue;
                eInkDrawingMinY = ushort.MaxValue;
                eInkDrawingMaxX = ushort.MinValue;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reset">是否重置，以便下次计算</param>
        /// <returns>是否需要更新</returns>
        public static bool eInkGetUpdateRectangle2(ref ushort minX, ref ushort maxX, ref ushort minY, ref ushort maxY, in bool reset)
        {
            if (eInkDrawingMaxX <= eInkDrawingMinX || eInkDrawingMaxY <= eInkDrawingMinY)
            {
                return false;
            }
            minX = eInkFormatUpdateAreaValue(eInkDrawingMinX,false);
            maxX = eInkFormatUpdateAreaValue(eInkDrawingMaxX,true);
            minY = eInkFormatUpdateAreaValue(eInkDrawingMinY,false);
            maxY = eInkFormatUpdateAreaValue(eInkDrawingMaxY,true);
            if (reset)
            {
                eInkDrawingMinX = 0;
                eInkDrawingMaxX = 0;
                eInkDrawingMinY = 0;
                eInkDrawingMaxX = 0;
            }
            return true;
        }

        public static byte eInkGetColorValue(in EInkColor color)
        {
            byte value = (byte)color;
            if (eInkBpp != 8)
            {
                //if (eInkBpp == 4)
                //{
                //    value = (byte)(value >> 4);
                //}
            }
            else {
                value = (byte)(value << 4);
            }
            return value;
        }
        private static void eInkSetBuffPoint(in ushort x, in ushort y, in byte value)
        {
            if (eInkBpp == 8)
            {
                eInkBuff[y * eInkWidth + x] = value;
            }
            else if (eInkBpp == 4)
            {
                int index = (y * eInkWidth + x);
                int byteIndex = (index + 1) >> 1;
                if ((index & 1) == 0)
                {
                    byte gray = (byte)((value << 4) | 0xF);
                    eInkBuff[byteIndex] = (byte)((eInkBuff[byteIndex] | 0xF0) & gray);
                }
                else
                {
                    byte gray = (byte)(0xF0 | value);
                    eInkBuff[byteIndex] = (byte)((eInkBuff[byteIndex] | 0xF) & gray);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        private static void eInkGetBuffPoint(in ushort x, in ushort y, out byte value)
        {
            if (eInkBpp == 8)
            {
                value = eInkBuff[y * eInkWidth + x];
            }
            else if (eInkBpp == 4)
            {
                int index = (y * eInkWidth + x);
                int byteIndex = (index + 1) >> 1;
                value = eInkBuff[byteIndex];
                if ((index & 1) == 0)
                {
                    value = (byte)(value >> 4);
                }
                else
                {
                    value &= 0xF;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public static bool eInkFill(ushort minX, ushort maxX, ushort minY, ushort maxY, in EInkColor color)
        {
            if (minX < eInkValidMinX)
            {
                minX = eInkValidMinX;
            }
            if (maxX > eInkValidMaxX)
            {
                maxX = eInkValidMaxX;
            }
            if (minY < eInkValidMinY)
            {
                minY = eInkValidMinY;
            }
            if (maxY > eInkValidMaxY)
            {
                maxY = eInkValidMaxY;
            }
            if (minX >= maxX) return false;
            if (minY >= maxY) return false;
            if (minX >= eInkValidMaxX) return false;
            if (minY >= eInkValidMaxY) return false;
            if (maxX <= eInkValidMinX) return false;
            if (maxY <= eInkValidMinY) return false;
            byte colorValue = eInkGetColorValue(color);
            for (ushort yy = minY; yy < maxY; yy++)
            {
                for (ushort xx = minX; xx < maxX; xx++)
                {
                    eInkSetBuffPoint(xx, yy, colorValue);
                }
            }
            if (eInkDrawingMinX > minX)
            {
                eInkDrawingMinX = minX;
            }
            if (eInkDrawingMinY > minY)
            {
                eInkDrawingMinY = minY;
            }
            if (eInkDrawingMaxX < maxX)
            {
                eInkDrawingMaxX = maxX;
            }
            if (eInkDrawingMaxY < maxY)
            {
                eInkDrawingMaxY = maxY;
            }
            return true;
        }

        private static ushort eInkFormatUpdateAreaValue(in ushort value, in bool isUpper)
        {
            if (((value * eInkBpp) & 0x8) != 0)
            {
                if (eInkBpp == 4)
                {
                    if (isUpper)
                    {
                        return (ushort)(value + 1);
                    }
                    else
                    {
                        return (ushort)(value - 1);
                    }
                }
                else {
                    throw new NotImplementedException();
                }
            }
            return value;
        }
    }
}
