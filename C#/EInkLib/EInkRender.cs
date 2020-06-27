using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using CCommonLib;
using CGraphic;

namespace EInkLib
{
    public static class EInkRender
    {
        public unsafe static Int32 MeasureText(in CTextInfo textInfo, in CGraphic.CFontInfo fontInfo, in CPoint startPosition, in CRect rect, out CRect renderRect)
        {
            Contract.Assert(startPosition.X >= 0 && startPosition.Y >= 0
                && startPosition.X <= (rect.MaxX - rect.MinX) && startPosition.Y <= (rect.MaxY - rect.MinY));
            /*
             * 需要支持的情况:
             * 1、单行
             * 2、单行，开头空2格
             * 3、多行
             * 4、多行，首行空2格
             */
            Int32 firstRowMaxX = 0;//首行的宽度
            Int32 length = 0;
            Int32 lineCount = 1;
            for (Int32 i = textInfo.Start, x = startPosition.X + rect.MinX, y = startPosition.Y + rect.MinY; i < textInfo.End; i++, length++)
            {
                var character = textInfo.Text[i];
                Int32 newLineCount = 0;//要换多少行，如果文本已经满一行，又遇到换行符，目前会换两行
                //强制换行
                if (character == '\n' || (character == '\r' && i < textInfo.End - 1 && textInfo.Text[i + 1] == '\n'))
                {
                    if (character == '\r')
                    {
                        i++;
                        length++;
                    }
                    newLineCount++;
                }
                if (x + fontInfo.Width >= rect.MaxX)
                {
                    newLineCount++;
                }
                if (newLineCount > 0)
                {
                    x = rect.MinX;
                    y += (fontInfo.Height * newLineCount);
                    lineCount += newLineCount;
                }
                else
                {
                    firstRowMaxX = x;
                }
                if (y + fontInfo.Height + fontInfo.Height * newLineCount >= rect.MaxY)
                {
                    break;
                }
                x += fontInfo.Width;
            }
            renderRect = new CRect();
            if (lineCount == 1)
            {
                renderRect.MinY = startPosition.X + rect.MinX;
                renderRect.MinY = startPosition.Y + rect.MinY;
            }
            else
            {
                renderRect.MinY = rect.MinX;
                renderRect.MinY = rect.MinY;
            }
            renderRect.MaxX = firstRowMaxX + fontInfo.Width;
            renderRect.MaxY = renderRect.MinY + fontInfo.Height * lineCount;
            return length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="textInfo"></param>
        /// <param name="fontInfo"></param>
        /// <param name="rect"></param>
        /// <param name="startPosition">是在rect内部从(0,0)开始的</param>
        /// <param name="renderRect"></param>
        /// <returns></returns>
        public static unsafe Int32 DrawText(ref CBitmap bitmap, in CTextInfo textInfo, CGraphic.CFontInfo fontInfo, in CRect rect, in CPoint startPosition, out CRect renderRect)
        {
            //Contract.Assert(startPosition.X >= 0 && startPosition.Y >= 0
            //    && startPosition.X <= (rect.MaxX - rect.MinX) && startPosition.Y <= (rect.MaxY - rect.MinY));
            ///*
            // * 需要支持的情况:
            // * 1、单行
            // * 2、单行，开头空2格
            // * 3、多行
            // * 4、多行，首行空2格
            // */

            //Int32 firstRowMaxX = 0;//首行的宽度
            //Int32 length = 0;
            //Int32 lineCount = 1;
            //FontBitmap fontBitmap;
            //for (Int32 i = textInfo.Start, x = startPosition.X + rect.MinX, y = startPosition.Y + rect.MinY; i < textInfo.End; i++, length++)
            //{
            //    var character = textInfo.Text[i];
            //    Int32 newLineCount = 0;//要换多少行，如果文本已经满一行，又遇到换行符，目前会换两行
            //    //强制换行
            //    if (character == '\n' || (character == '\r' && i < textInfo.End - 1 && textInfo.Text[i + 1] == '\n'))
            //    {
            //        if (character == '\r')
            //        {
            //            i++;
            //            length++;
            //        }
            //        newLineCount++;
            //    }
            //    if (x + fontInfo.Width >= rect.MaxX)
            //    {
            //        newLineCount++;
            //    }
            //    if (newLineCount > 0)
            //    {
            //        x = rect.MinX;
            //        y += (fontInfo.Height * newLineCount);
            //        lineCount += newLineCount;
            //    }
            //    else
            //    {
            //        firstRowMaxX = x;
            //    }
            //    CFont.GetFontBitmap(character, fontInfo.FontSize, out fontBitmap);
            //    DrawFont(bitmap, x, y, fontBitmap, fontInfo);

            //    if (y + fontInfo.Height + fontInfo.Height * newLineCount >= rect.MaxY)
            //    {
            //        break;
            //    }
            //    x += fontInfo.Width;
            //}
            //renderRect = new CRect();
            //if (lineCount == 1)
            //{
            //    renderRect.MinY = startPosition.X + rect.MinX;
            //    renderRect.MinY = startPosition.Y + rect.MinY;
            //}
            //else
            //{
            //    renderRect.MinY = rect.MinX;
            //    renderRect.MinY = rect.MinY;
            //}
            //renderRect.MaxX = firstRowMaxX + fontInfo.Width;
            //renderRect.MaxY = renderRect.MinY + fontInfo.Height * lineCount;
            //return length;
            return DrawTextInternal(bitmap, textInfo, fontInfo, rect, startPosition, out renderRect);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="textInfo"></param>
        /// <param name="fontInfo"></param>
        /// <param name="rect"></param>
        /// <param name="startPosition">是在rect内部从(0,0)开始的</param>
        /// <param name="renderRect"></param>
        /// <returns></returns>
        public static unsafe Int32 DrawTextInternal(CBitmap? bitmap, in CTextInfo textInfo, CGraphic.CFontInfo fontInfo, in CRect rect, in CPoint startPosition, out CRect renderRect)
        {
            Contract.Assert(startPosition.X >= 0 && startPosition.Y >= 0
                && startPosition.X <= (rect.MaxX - rect.MinX) && startPosition.Y <= (rect.MaxY - rect.MinY));
            /*
             * 需要支持的情况:
             * 1、单行
             * 2、单行，开头空2格
             * 3、多行
             * 4、多行，首行空2格
             */

            Int32 length = 0;
            FontBitmap fontBitmap;
            Int32 maxX = startPosition.X + rect.MinX;
            Int32 maxY = startPosition.Y + rect.MinY;
            int lineCount = 0;


            if (maxY + fontInfo.Height < rect.MaxY)
            {
                lineCount = 1;
                for (Int32 i = textInfo.Start, x = maxX; i < textInfo.End; i++, length++)
                {
                    var character = textInfo.Text[i];
                    //强制换行
                    if (character == '\n' || (character == '\r' && i < textInfo.End - 1 && textInfo.Text[i + 1] == '\n'))
                    {
                        if (character == '\r')
                        {
                            i++;
                            length++;
                        }
                        maxY += fontInfo.Height;
                        if (maxY + fontInfo.Height > rect.MaxY)
                        {
                            break;
                        }
                        x = rect.MinX;//折行的时候，忽略startPosition
                        lineCount++;
                        continue;
                    }
                    CFont.GetFontBitmap(character, fontInfo.FontSize, out fontBitmap);
                    if (x + fontBitmap.Width >= rect.MaxX)
                    {
                        x = rect.MinX;//折行的时候，忽略startPosition
                        maxY += fontInfo.Height;

                        if (maxY + fontInfo.Height > rect.MaxY)
                        {
                            break;
                        }
                        lineCount++;
                    }
                    if (bitmap != null)
                    {
                        DrawFont(bitmap.Value, x, maxY, fontBitmap, fontInfo);
                    }
                    x += fontBitmap.Width;
                    if (maxX < x)
                    {
                        maxX = x;
                    }
                }
            }
            renderRect = new CRect();
            if (lineCount == 1)
            {
                renderRect.MinY = startPosition.X + rect.MinX;
                renderRect.MinY = startPosition.Y + rect.MinY;
            }
            else
            {
                renderRect.MinY = rect.MinX;
                renderRect.MinY = rect.MinY;
            }
            renderRect.MaxX = maxX;
            renderRect.MaxY = maxY;
            return length;
        }

        public static void GetPixel(in byte[] eInkBuff,in Int32 eInkWidth, in byte eInkBpp, in Int32 x, in Int32 y, out byte value)
        {
            //var cnt = eInkBuff.Count(p => p != 0);
            if (eInkBpp == 8)
            {
                value = eInkBuff[y * eInkWidth + x];
            }
            else if (eInkBpp < 8)
            {
                Contract.Assert(eInkBpp == 4 || eInkBpp == 2 || eInkBpp == 1);
                Int32 stride = (eInkWidth * eInkBpp + 7) / 8;
                Int32 byteIndex = y * stride + x * eInkBpp / 8;
                Int32 mode = 8 / eInkBpp - 1;
                Int32 index = x & mode;
                byte temp = eInkBuff[byteIndex];
                value = (byte)(((temp << (index * eInkBpp)) & 0xFF) >> (8 - eInkBpp));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public unsafe static void GetPixel(byte* eInkBuff, in Int32 eInkWidth, in byte eInkBpp, in Int32 x, in Int32 y, out byte value)
        {
            //var cnt = eInkBuff.Count(p => p != 0);
            if (eInkBpp == 8)
            {
                value = eInkBuff[y * eInkWidth + x];
            }
            else if (eInkBpp < 8)
            {
                Contract.Assert(eInkBpp == 4 || eInkBpp == 2 || eInkBpp == 1);
                Int32 stride = (eInkWidth * eInkBpp + 7) / 8;
                Int32 byteIndex = y * stride + x * eInkBpp / 8;
                Int32 mode = 8 / eInkBpp - 1;
                Int32 index = x & mode;
                byte temp = eInkBuff[byteIndex];
                value = (byte)(((temp << (index * eInkBpp)) & 0xFF) >> (8 - eInkBpp));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public unsafe static void DrawToBitmap(ref Bitmap image, in CBitmap bitmap, in CRect drawRect)
        {
            byte eInkBpp = bitmap.BitsPerPixel;
            CRect rect = new CRect() { MinX = 0, MinY = 0, MaxX = image.Width, MaxY = image.Height };
            ValueFuns.MinRect(rect, drawRect, ref rect);
            for (Int32 y = rect.MinY; y < rect.MaxY; y++)
            {
                for (Int32 x = rect.MinX; x < rect.MaxX; x++)
                {
                    byte value;
                    GetPixel(bitmap.Buffer, bitmap.Width, eInkBpp, x, y, out value);
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
                    }
                    value = (byte)(~value);
                    image.SetPixel(x, y, Color.FromArgb(value, value, value));
                }
            }
        }

        private static bool DrawFont(in CBitmap bitmap, in int offsetX, in int offsetY, in FontBitmap matrix, in CGraphic.CFontInfo info)
        {
            Int32 maxX = matrix.X + matrix.InnerWidth;
            Int32 maxY = matrix.Y + matrix.InnerHeight;
            for (Int32 y = matrix.Y; y < maxY; y++)
            {
                var newY = offsetY + y;
                var yy = y - matrix.Y;
                if (newY >= bitmap.Height)
                {
                    break;
                }
                for (Int32 x = matrix.X; x < maxX; x++)
                {
                    var newX = offsetX + x;
                    if (newX >= bitmap.Width)
                    {
                        continue;
                    }
                    var xx = x - matrix.X;
                    Int32 fontStride = (matrix.InnerWidth * matrix.BitsPerPixel + 7) / 8;
                    Int32 byteIndex = yy * fontStride + (xx * matrix.BitsPerPixel / 8);
                    byte color = matrix.Data[byteIndex];
                    //if (color != 0)
                    {
                        byte gray;
                        if (matrix.BitsPerPixel != 8)
                        {
                            Contract.Assert(matrix.BitsPerPixel == 4 || matrix.BitsPerPixel == 2 || matrix.BitsPerPixel == 1);
                            Int32 mode = (8 / matrix.BitsPerPixel) - 1;
                            Int32 index = xx & mode;
                            //Int32 index = ((yy * matrix.InnerWidth + xx) * matrix.BitsPerPixel) % (8 / matrix.BitsPerPixel);
                            gray = (byte)(((color << (index * matrix.BitsPerPixel)) & 0xFF) >> (8 - matrix.BitsPerPixel));

                            if (gray != 0)
                            {
                                if (matrix.BitsPerPixel == 4)
                                {
                                    gray <<= 4;
                                    gray |= 0xF;
                                }
                                else if (matrix.BitsPerPixel == 2)
                                {
                                    gray <<= 6;
                                    gray |= 0x3F;
                                }
                                else if (matrix.BitsPerPixel == 1)
                                {
                                    gray <<= 7;
                                    gray |= 0x7F;
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }
                            }
                        }
                        else
                        {
                            gray = color;
                        }
                        //gray = (byte)(~gray);
                        if (gray != 0)
                        {
                            DrawPixel(bitmap, newX, newY, gray);
                        }
                        //bitmap.SetPixel(newX, newY, Color.FromArgb(gray, gray, gray));
                    }
                }
            }
            return true;
        }

        public unsafe static void DrawPixel(in CBitmap bitmap, in Int32 x, in Int32 y, UInt32 color)
        {
            if (bitmap.BitsPerPixel == 8)
            {
                bitmap.Buffer[y * bitmap.Width + x] = (byte)color;
            }
            else if (bitmap.BitsPerPixel == 4)
            {
                Int32 byteIndex = y * bitmap.Stride + ((x + 1) >> 1);
                int index = x & 1;
                if (index == 0)
                {
                    byte gray = (byte)((color << 4) | 0xF);
                    bitmap.Buffer[byteIndex] = (byte)((bitmap.Buffer[byteIndex] | 0xF0) & gray);
                }
                else
                {
                    byte gray = (byte)(0xF0 | color);
                    bitmap.Buffer[byteIndex] = (byte)((bitmap.Buffer[byteIndex] | 0xF) & gray);
                }
            }
            else if (bitmap.BitsPerPixel == 2)
            {
                Int32 byteIndex = y * bitmap.Stride + ((x + 3) >> 2);
                var index = x & 3;
                if (index == 0)
                {
                    byte gray = (byte)((color << 6) | 0x3F);
                    bitmap.Buffer[byteIndex] = (byte)((bitmap.Buffer[byteIndex] | 0xC0) & gray);
                }
                else if (index == 1)
                {
                    byte gray = (byte)((color << 4) | 0xCF);
                    bitmap.Buffer[byteIndex] = (byte)((bitmap.Buffer[byteIndex] | 0x30) & gray);
                }
                else if (index == 2)
                {
                    byte gray = (byte)((color << 2) | 0xF3);
                    bitmap.Buffer[byteIndex] = (byte)((bitmap.Buffer[byteIndex] | 0x0C) & gray);
                }
                else
                {
                    byte gray = (byte)((color << 2) | 0xFC);
                    bitmap.Buffer[byteIndex] = (byte)((bitmap.Buffer[byteIndex] | 0x03) & gray);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static bool GetValidFontSize(in Int32 fontSize, out CFontInfo fontInfo)
        {
            Contract.Assert(fontSize > 0 && fontSize <= CFont.MAX_FONT_SIZE);
            //这里改用其他获取Fonts支持的来，
            var validSize = CFont.GetValidFontSize(fontSize);

            CGraphicInits.InitFontInfo(validSize, out fontInfo);
            return fontInfo.FontSize == fontSize;
        }
    }
}
