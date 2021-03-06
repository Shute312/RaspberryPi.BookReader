﻿using CCommonLib;
using CGraphic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace EInkLib
{
    public partial class CFont
    {
        public const byte MAX_FONT_SIZE = 128;
        private const string ExtName = ".font";
        public static Int32  DefaultSize = 24;
        //public static Color Background = Color.Black;
        //public static Color Foreground = Color.White;
        public static Color Background = Color.White;
        public static Color Foreground = Color.Black;
        public static FontBitmap[][] BitmapCache;
        public static byte[] ExistedFont;//值为0时，表示不存在对应字号；指为1时，表示存在字号，但是未读取、解析字体文件
        public static string FontDir;


        private static readonly byte FONT_STATE_NO = 0;//不支持的字号(不存在字体文件、或者解析不了)
        private static readonly byte FONT_STATE_UNLOAD = 1;//支持的字号，但未加载
        private static readonly byte FONT_STATE_READY = 2;//支持并已经加载的字号


        public static void Init(string dir)
        {
            Charset.Init();
            FontDir = dir;
            //for C：记得初始化默认值为0
            ExistedFont = new byte[MAX_FONT_SIZE + 1];
            var files = Directory.GetFiles(dir);
            if (files != null)
            {
                for (Int32 i = 0; i < files.Length; i++)
                {
                    var f = files[i];
                    if (f.Length - dir.Length <= 5)
                    {
                        continue;
                    }
                    string fileName = f.Substring(dir.Length + 1, f.Length - dir.Length - 5 - 1);//.font为5个字符+一个斜杠
                    Int32 fontSize;
                    if (Int32.TryParse(fileName, out fontSize))
                    {
                        if (fontSize > 0 && fontSize <= MAX_FONT_SIZE)
                        {
                            ExistedFont[fontSize] = FONT_STATE_UNLOAD;
                        }
                    }
                }
            }
            BitmapCache = new FontBitmap[ExistedFont.Length][];
        }

        public static bool WriteFont(in char[] text, Int32 textLength, in Int32 fontSize, in string path, in Font font, Int32 bitsPerPixel)
        {
            Contract.Assert(bitsPerPixel < 9);
            Contract.Assert(text != null && textLength > 0 && text.Length == textLength);

            var file = Path.Combine(path, fontSize.ToString() + ExtName);
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (Stream stream = File.Open(file, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                CFontInfo info;
                CGraphicInits.InitFontInfo(fontSize, out info);
                Bitmap image = new Bitmap(info.Width, info.Height);
                Graphics graphics = Graphics.FromImage(image);
                Brush brush = new SolidBrush(Foreground);
                
                byte[] lengthBuff = new byte[] { (byte)(textLength >> 8), (byte)(textLength & 0xFF) };
                stream.Write(lengthBuff, 0, lengthBuff.Length);
                stream.WriteByte((byte)bitsPerPixel);
                byte halfWidth = (byte)((info.Width + 1) >> 1);
                for (Int32 i = 0; i < textLength; i++)
                {
                    FontBitmap bitmap = new FontBitmap();
                    graphics.Clear(Background);
                    var ch = text[i];
                    graphics.DrawString(ch.ToString(), font, brush, 0, 0);

                    //var buffer = new byte[info.Width * info.Height];
                    var buffer = Enumerable.Repeat<byte>(byte.MaxValue,info.Width * info.Height).ToArray();
                    bitmap.BitsPerPixel = (byte)bitsPerPixel;
                    bitmap.FontSize = info.FontSize;
                    bitmap.Unicode = (ushort)ch;

                    //文字实际绘制的位置
                    byte minX = (byte)info.Width;
                    byte maxX = 0;
                    byte minY = (byte)info.Height;
                    byte maxY = 0;
                    Int32 shiftX = 0;//当用非标准大小去描述字模时，位置要做二次偏移
                    for (byte y = 0; y < info.Height; y++)
                    {
                        for (byte x = 0; x < info.Width; x++)
                        {
                            Color color = image.GetPixel(x, y);
                            //Gray = R*0.299 + G*0.587 + B*0.114 放大 65536 倍(2的16次方)
                            byte gray = (byte)((color.R * 19595 + color.G * 38470 + color.B * 7471) >> 16);
                            //byte gray = color.G;//直接用绿色分量代替
                            if (gray < byte.MaxValue)
                            {
                                if (minX > x) minX = x;
                                if (maxX < x) maxX = x;
                                if (minY > y) minY = y;
                                if (maxY < y) maxY = y;
                            }
                            if (gray < byte.MaxValue)
                            {
                                if (bitsPerPixel != 8)
                                {
                                    //先处理BPP，可以获得更小的绘制范围
                                    gray = (byte)(gray >> (8 - bitsPerPixel));
                                }
                            }
                            buffer[y * info.Width + x] = gray;
                        }
                    }
                    bitmap.X = minX;
                    bitmap.Y = minY;
                    bitmap.InnerWidth = (byte)(maxX > minX ? maxX - minX + 1 : 0);
                    bitmap.InnerHeight = (byte)(maxY > minY ? maxY - minY + 1 : 0);
                    bitmap.Width =  (byte)info.Width;
                    bitmap.Height = (byte)info.Height;

                    //比较小的，宽度只占半个字号
                    if (bitmap.InnerWidth > 0)
                    {
                        if (bitmap.InnerWidth < halfWidth)
                        {
                            bitmap.Width = halfWidth;
                            var newX = (byte)((bitmap.Width - bitmap.InnerWidth) >> 1);
                            //图像整体向左移动
                            shiftX = newX - bitmap.X;
                            bitmap.X = newX;
                        }
                    }

                    if (bitmap.InnerWidth > 0)
                    {
                        Int32 fontStride = (bitmap.InnerWidth * bitsPerPixel + 7) / 8;
                        bitmap.Data = Enumerable.Repeat<byte>(byte.MaxValue, fontStride * bitmap.InnerHeight).ToArray();
                        for (Int32 y = 0; y < bitmap.InnerHeight; y++)
                        {
                            Int32 yy = bitmap.Y + y;
                            for (Int32 x = 0; x < bitmap.InnerWidth; x++)
                            {
                                Int32 xx = bitmap.X + x - shiftX;
                                Int32 byteIndex = y * fontStride + (x * bitmap.BitsPerPixel / 8);
                                byte color = buffer[yy * info.Width + xx];
                                if (color < byte.MaxValue)
                                {
                                    if (bitmap.BitsPerPixel == 8)
                                    {
                                        bitmap.Data[byteIndex] = (byte)color;
                                    }
                                    else
                                    {
                                        Contract.Assert(bitmap.BitsPerPixel == 4 || bitmap.BitsPerPixel == 2 || bitmap.BitsPerPixel == 1);
                                        Int32 mode = (8 / bitmap.BitsPerPixel) - 1;
                                        Int32 index = x & mode;
                                        //Int32 index = ((y * bitmap.InnerWidth + x) * bitsPerPixel) % (8 / bitsPerPixel);
                                        var skip = ((mode - index) * bitmap.BitsPerPixel);
                                        Int32 gray = (byte)(color << skip);
                                        var headOffset = skip + bitmap.BitsPerPixel;
                                        var head = (0xff>> headOffset)<< headOffset;//将前面的位数全弄成1
                                        var tailOffet = 8 - skip;
                                        var tail = (((byte)(0xff<< tailOffet)) >> tailOffet);//将后边的位全弄成1
                                        gray = head | tail | gray;

                                        bitmap.Data[byteIndex] &= (byte)gray;
                                    }
                                }
                            }
                        }
                    }
                    //martix.BitsPerPixel = (byte)BPP;
                    stream.WriteByte((byte)(bitmap.Unicode >> 8));
                    stream.WriteByte((byte)(bitmap.Unicode & 0xFF));
                    stream.WriteByte(bitmap.X);
                    stream.WriteByte(bitmap.Y);
                    stream.WriteByte(bitmap.InnerWidth);
                    stream.WriteByte(bitmap.InnerHeight);
                    stream.WriteByte(bitmap.Width);//只记录宽度，不记录高度
                    if (bitmap.InnerWidth > 0)
                    {
                        stream.Write(bitmap.Data, 0, bitmap.Data.Length);
                    }
                    stream.Flush();
                }
                image.Dispose();
                graphics.Dispose();
            }
            return true;
        }

        public static ushort ReadFont(in string path, in Int32 fontSize, out FontBitmap[] bitmaps)
        {
            ushort length = 0;
            bitmaps = null;
            if (File.Exists(path))
            {
                byte[] bytes;
                CFontInfo info;
                CGraphicInits.InitFontInfo(fontSize, out info);
                Int32 byteSize = ReadFile(path, out bytes);
                if (byteSize > 2)
                {
                    Contract.Assert(byteSize == bytes.Length);
                    ushort total = (ushort)((bytes[0] << 8) | (bytes[1]));
                    bitmaps = new FontBitmap[total];
                    byte bpp = bytes[2];
                    Int32 byteIndex = 3;

                    for (ushort index = 0; index < total; index++)//FontMatrix占用内存为2+4+1+BytesSize
                    {
                        var bitmap = new FontBitmap();
                        bitmap.BitsPerPixel = bpp;
                        bitmap.FontSize = fontSize;
                        bitmap.Unicode = (ushort)((bytes[byteIndex] << 8) | (bytes[byteIndex + 1]));
                        bitmap.X = bytes[byteIndex + 2];
                        bitmap.Y = bytes[byteIndex + 3];
                        bitmap.InnerWidth = bytes[byteIndex + 4];
                        bitmap.InnerHeight = bytes[byteIndex + 5];
                        bitmap.Height = (byte)info.Height;
                        bitmap.Width = bytes[byteIndex + 6];
                        //Int32 dataLength = (bitmap.InnerWidth * bitmap.InnerHeight * bpp + 7) >> 3;
                        Int32 dataLength = (bitmap.InnerWidth * bitmap.BitsPerPixel + 7)/8 * bitmap.InnerHeight;
                        bitmap.Data = new byte[dataLength];
                        if (dataLength > 0)
                        {
                            Buffer.BlockCopy(bytes, byteIndex + 7, bitmap.Data, 0, dataLength);
                        }
                        bitmaps[index] = bitmap;
                        byteIndex += (dataLength + 7);
                        length++;
                    }
                    Contract.Assert(byteIndex == byteSize);
                }
            }
            return length;
        }

        /// <summary>
        /// 获取字模
        /// </summary>
        /// <param name="character"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool GetFontBitmap(in ushort character, in Int32 fontSize, out FontBitmap bitmap)
        {
            Contract.Assert(fontSize <= MAX_FONT_SIZE);
            Int32 validSize = GetValidFontSize(fontSize);
            var existed = ExistedFont[validSize];
            Contract.Assert(existed != 0);
            if (existed == FONT_STATE_UNLOAD)//需要加载
            {
                var file = Path.Combine(FontDir, validSize + ExtName);

                //如果读取不到，就标记为不存在字号
                if (ReadFont(file, validSize, out BitmapCache[validSize]) == 0)
                {
                    ExistedFont[validSize] = FONT_STATE_NO;
                    //递归下
                    return GetFontBitmap(character, fontSize, out bitmap);
                }
                else
                {
                    ExistedFont[validSize] = FONT_STATE_READY;
                }
            }
            var font = BitmapCache[validSize];
            if (font != null && font.Length > 0)
            {
                var index = Charset.GetUnicodeIndex(character);
                bitmap = font[index];
                //Contract.Assert(character == bitmap.Unicode);//存在转码问题
                return true;
            }
            else
            {
                bitmap = new FontBitmap()
                {
                    BitsPerPixel = 0,
                    Data = new byte[0],
                    Unicode = 0,
                    InnerWidth = 0,
                    InnerHeight = 0,
                    FontSize = 0,
                    X = 0,
                    Y = 0,
                };
            }
            return false;
        }

        private static Int32  ReadFile(in string file, out byte[] bytes)
        {
            bytes = File.ReadAllBytes(file);
            return bytes == null ? 0 : bytes.Length;
        }

        public static Int32 GetValidFontSize(in Int32 fontSize)
        {
            Contract.Ensures(ExistedFont != null && DefaultSize > 0 && DefaultSize <= MAX_FONT_SIZE);
            if (ExistedFont[fontSize] > FONT_STATE_NO)
            {
                return fontSize;
            }
            //查找相近的小字号字号
            for (Int32 tempSize = fontSize - 1; tempSize > 0; tempSize--)
            {
                if (ExistedFont[tempSize] > FONT_STATE_NO)
                {
                    return tempSize;
                }
            }
            for (Int32 tempSize = fontSize + 1; tempSize <= MAX_FONT_SIZE; tempSize++)
            {
                if (ExistedFont[tempSize] > FONT_STATE_NO)
                {
                    return tempSize;
                }
            }
            return DefaultSize;
        }
    }
}
