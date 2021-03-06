﻿using CCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CGraphic
{
    public static class CGraphicInits
    {
        private static UInt32 _BitmapHashCodeUniqueCode = 0;
        public unsafe static void InitBitmap(out CBitmap bitmap, in CPixelFormat format, in Int32 width, in Int32 height)
        {

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            bitmap = new CBitmap();
            bitmap.HashCode = ++_BitmapHashCodeUniqueCode;
            bitmap.Format = format;
            bitmap.Width = width;
            bitmap.Height = height;
            switch (format)
            {
                case CPixelFormat.Format24bppRgb:
                    bitmap.BitsPerPixel = 24;
                    break;
                case CPixelFormat.Format1bppIndexed:
                    bitmap.BitsPerPixel = 1;
                    break;
                case CPixelFormat.Format4bppIndexed:
                    bitmap.BitsPerPixel = 4;
                    break;
                case CPixelFormat.Format8bppIndexed:
                    bitmap.BitsPerPixel = 8;
                    break;
                case CPixelFormat.Format32bppArgb:
                    bitmap.BitsPerPixel = 32;
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
            bitmap.Stride = (width * bitmap.BitsPerPixel + 7) / 8;
            bitmap.Length = bitmap.Stride * bitmap.Height;
            //bitmap.Buffer = new byte[bitmap.Stride * bitmap.Height];
            IntPtr ptr = Marshal.AllocHGlobal(bitmap.Length);
            bitmap.Buffer = (byte*)ptr;
            var temp = Enumerable.Repeat<byte>(byte.MaxValue, bitmap.Length).ToArray();
            Marshal.Copy(temp, 0, ptr, temp.Length);
            var time = stopwatch.ElapsedMilliseconds;
        }

        public static void InitFontInfo(in Int32 fontSize, out CFontInfo info)
        { 
            info = new CFontInfo();
            info.FontSize = fontSize;
            info.Width = (fontSize * 4 + 2) / 3;
            info.Height = info.Width;
        }
    }
}
