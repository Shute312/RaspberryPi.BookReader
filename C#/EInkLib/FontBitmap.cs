using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EInkLib
{
    public struct FontBitmap
    {
        public ushort Unicode;
        //字模数据
        public byte[] Data;
        //实际绘制的内容：位置与大小
        public byte X;
        public byte Y;
        public byte InnerWidth;
        public byte InnerHeight;
        public byte BitsPerPixel;
        public Int32 FontSize;
    }
}
