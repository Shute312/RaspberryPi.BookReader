using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CCommonLib;

namespace CGraphic
{
    //目前版本是不支持透明度的，要支持透明度，可以参考UC/MBitmap
    public static partial class CG
    {
        private static DrawTextDelegate _DrawText;
        private static MeasureTextDelegate _MeasureText;
        private static GetValiedFontSizeDelegate _GetValidFontSize;
        public static void Init(in GetValiedFontSizeDelegate getValidFontSize, in DrawTextDelegate drawText, in MeasureTextDelegate measureText)
        {
            _DrawText = drawText;
            _MeasureText = measureText;
            _GetValidFontSize = getValidFontSize;
        }

        public static bool DrawLine(ref CBitmap bitmap, in int thickness, in CPoint p1, in CPoint p2, in UInt32 color)
        {
            return DrawLine2(ref bitmap, thickness, p1.X, p1.Y, p2.X, p2.Y, color);
        }
        public static bool DrawLine2(ref CBitmap bitmap, in int thickness, in Int32 x1, in Int32 y1, in Int32 x2, in Int32 y2, in UInt32 color)
        {
            if (y1 == y2)
            {
                return DrawHorLine(ref bitmap, thickness, Math.Min(x1, x2), y1, Math.Abs(x1 - x2), color);
            }
            else if (x1 == x2)
            {
                return DrawVerLine(ref bitmap, thickness, x1, Math.Min(y1, y2), Math.Abs(y1 - y2), color);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static bool DrawHorLine(ref CBitmap bitmap, in Int32 thickness, in Int32 x, in Int32 y, in Int32 width, in UInt32 color)
        {
            Contract.Assert(thickness > 0);
            return FillRect2(ref bitmap, x, y, x + width, y + thickness, color);
        }
        private static bool DrawVerLine(ref CBitmap bitmap, in Int32 thickness, in Int32 x, in Int32 y, in Int32 height, in UInt32 color)
        {
            Contract.Assert(thickness > 0);
            return FillRect2(ref bitmap, x, y, x + thickness, y + height, color);
        }

        public static bool DrawRectangle(ref CBitmap bitmap, in Int32 thickness, in CRect rect, in UInt32 color)
        {
            return DrawRectangle2(ref bitmap, thickness, rect.MinX, rect.MinY, rect.MaxX, rect.MaxY, color);
        }

        public static bool DrawRectangle2(ref CBitmap bitmap, in Int32 thickness, in Int32 x1, in Int32 y1, in Int32 x2, in Int32 y2, in UInt32 color)
        {
            Int32 minX = Math.Min(x1, x2);
            Int32 maxX = Math.Max(x1, x2);
            Int32 minY = Math.Min(y1, y2);
            Int32 maxY = Math.Max(y1, y2);
            Int32 width = (ushort)(maxX - minX);
            Int32 height = (ushort)(maxY - minY);
            if (width == 0 || height == 0)
            {
                return false;
            }
            return DrawHorLine(ref bitmap, thickness, minX, minY, width, color) &
            DrawHorLine(ref bitmap, thickness, minX, (ushort)(maxY - thickness), width, color) &
            DrawVerLine(ref bitmap, thickness, minX, minY, height, color) &
            DrawVerLine(ref bitmap, thickness, (ushort)(maxX - thickness), minY, height, color);
        }

        public static bool FillRect(ref CBitmap bitmap, in CRect rect, in UInt32 color)
        {
            return FillRect2(ref bitmap, rect.MinX, rect.MinY, rect.MaxX, rect.MaxY, color);
        }
        public static bool FillRect2(ref CBitmap bitmap, Int32 minX, Int32 minY, Int32 maxX, Int32 maxY, in UInt32 color)
        {
            Contract.Ensures(minX > -1 && maxX <= bitmap.Width && minY > -1 && maxY <= bitmap.Height);
            for (int yy = minY; yy < maxY; yy++)
            {
                for (int xx = minX; xx < maxX; xx++)
                {
                    DrawPixel(bitmap, xx, yy, color);
                }
            }
            return true;
        }

        public static Int32 MeasureText(in CTextInfo textInfo, in CFontInfo fontInfo, in CPoint startPosition, in CRect rect, out CRect renderRect)
        {
            return _MeasureText(textInfo, fontInfo, startPosition, rect, out renderRect);
        }

        public static Int32 DrawText(ref CBitmap bitmap, in CTextInfo textInfo, CFontInfo fontInfo,in CRect rect, in CPoint startPosition, out CRect renderRect)
        {
            return _DrawText(ref bitmap, textInfo, fontInfo, rect, startPosition, out renderRect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcRect"></param>
        /// <param name="dst"></param>
        /// <param name="dstRect"></param>
        /// <param name="srcOffsetDst">src相对dst，偏移了多少</param>
        public unsafe static void DrawBitmap(in CBitmap src, in CRect srcRect, ref CBitmap dst, in CRect dstRect, in CPoint srcOffsetDst)
        {
            var tempRect = new CRect()
            {
                MinX = srcRect.MinX + srcOffsetDst.X,
                MaxX = srcRect.MaxX + srcOffsetDst.X,
                MinY = srcRect.MinX + srcOffsetDst.Y,
                MaxY = srcRect.MaxX + srcOffsetDst.Y,
            };
            CRect rect = new CRect();
            ValueFuns.MinRect(tempRect, dstRect, ref rect);
            if (!ValueFuns.IsValidRect(rect))
            {
                return;
            }

            if (src.BitsPerPixel == dst.BitsPerPixel)
            {
                if (src.BitsPerPixel >= 8)
                {
                    if ((src.BitsPerPixel % 8) == 0)
                    {
                        var bytesPerPixel = src.BitsPerPixel / 8;
                        int rectStride = (rect.MaxX - rect.MinX) * bytesPerPixel;
                        var dstX = rect.MinX;
                        int srcX = dstX - srcOffsetDst.X;
                        for (int dstY = rect.MinY; dstY < rect.MaxY; dstY++)
                        {
                            int srcY = dstY - srcOffsetDst.Y;
                            //Buffer.BlockCopy(src.Buffer, (srcY * src.Width + srcX) * bytesPerPixel, dst.Buffer, (dstY * dst.Width + dstX) * bytesPerPixel, rectStride);
                            byte[] srcBytes = new byte[rectStride];
                            Marshal.Copy((IntPtr)(src.Buffer+ (srcY * src.Width + srcX) * bytesPerPixel), srcBytes, 0, srcBytes.Length);
                            Marshal.Copy(srcBytes, 0,(IntPtr)(dst.Buffer + (dstY * dst.Width + dstX) * bytesPerPixel), srcBytes.Length);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    return;
                }
                else
                {
                    //开头跟结束的半个字符要做处理
                    if (src.BitsPerPixel == 1 || src.BitsPerPixel == 2 || src.BitsPerPixel == 4)//能夠被8整除
                    {
                        int pixelsPerByte = 8 / src.BitsPerPixel;
                        int dstLeftRemainBits = (8 - ((rect.MinX * src.BitsPerPixel) % pixelsPerByte)) % 8;//左边剩余多少位，后边剩余多少位
                        int srcLeftRemainBits = (8 - (((rect.MinX - srcOffsetDst.X) * src.BitsPerPixel) % pixelsPerByte)) % 8;//左边剩余多少位，后边剩余多少位
                        if (dstLeftRemainBits == srcLeftRemainBits)//位数对齐，可以做批量操作优化
                        {
                            int dstRightRemainBits = ((rect.MaxX * src.BitsPerPixel) % pixelsPerByte) % 8;
                            int dstX = rect.MinX + dstLeftRemainBits / src.BitsPerPixel;
                            int srcX = (rect.MinX - srcOffsetDst.X) + srcLeftRemainBits / src.BitsPerPixel;
                            int rectStride = ((rect.MaxX - dstRightRemainBits / src.BitsPerPixel) - dstX) / pixelsPerByte;
                            for (int dstY = rect.MinY; dstY < rect.MaxY; dstY++)
                            {
                                if (rectStride > 0)
                                {
                                    //todo 还未完成整个算法
                                }
                                int srcY = dstY - srcOffsetDst.Y;
                                //Buffer.BlockCopy(src.Buffer, (srcY * src.Width + srcX) / pixelsPerByte, dst.Buffer, (dstY * dst.Width + dstY) / pixelsPerByte, rectStride);
                                byte[] srcBytes = new byte[rectStride];
                                Marshal.Copy((IntPtr)(src.Buffer + (srcY * src.Width + srcX) / pixelsPerByte), srcBytes, 0 ,srcBytes.Length);
                                Marshal.Copy(srcBytes, 0, (IntPtr)(dst.Buffer + (dstY * dst.Width + dstY) / pixelsPerByte), srcBytes.Length);
                            }
                            return;
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            if (src.BitsPerPixel <= 8 && dst.BitsPerPixel <= 8)
            {
                Contract.Assert(src.BitsPerPixel == 8 || src.BitsPerPixel == 4 || src.BitsPerPixel == 2 || src.BitsPerPixel == 1);
                for (int y = rect.MinY; y < rect.MaxY; y++)
                {
                    for (int x = rect.MinX; x < rect.MaxX; x++)
                    {
                        UInt32 color = 0xFF;
                        DrawPixel(dst, x, y,color);
                    }
                }
                return;
            }
            throw new NotImplementedException();
        }

        public static bool GetValidFontSize(in Int32 fontSize,out CFontInfo fontInfo)
        {
            return _GetValidFontSize(fontSize, out fontInfo);
        }
        public unsafe static void DrawPixel(in CBitmap bitmap, in Int32 x, in Int32 y, UInt32 color)
        {
            if (bitmap.BitsPerPixel == 8)
            {
                bitmap.Buffer[y * bitmap.Width + x] = (byte)color;
            }
            else if (bitmap.BitsPerPixel ==4)
            {
                Int32 byteIndex = y * bitmap.Stride + (x >> 1);
                var index = x & 1;
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
                Int32 byteIndex = y * bitmap.Stride + (x >> 2);
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
            else if(bitmap.BitsPerPixel==1)
            {
                Int32 byteIndex = y * bitmap.Stride + (x >> 2);
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
    }
}
