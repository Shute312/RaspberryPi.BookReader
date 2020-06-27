using CCommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGraphic
{
    public unsafe struct CBitmap
    {
        public UInt32 HashCode;
        public byte* Buffer;
        public Int32 Length;//Buff的长度(兼容C的写法)
        public byte BitsPerPixel;
        public CPixelFormat Format;
        public Int32 Stride;//一行多少Byte，需要字节对齐
        public Int32 Width;
        public Int32 Height;
    }

    public struct CFontInfo
    {
        public Int32 FontSize;//字号
        public Int32 Width;//像素宽
        public Int32 Height;//像素高
    }

    public unsafe struct CTextInfo {
        public char* Text;//for C 要改为ushort*
        public Int32 Length;//总长度，如果总长度未知，则设置为-1
        public Int32 Start;
        public Int32 End;
    }

    public struct TextStyle
    {
        public Int32 WorkSpacing;//字间距(设置为1时，一个字的左右多一个像素空间)
        public Int32 LineSpacing;//行间距(设置为1时，一行字的上下多一个像素空间)

        public short LineMarginTop;//行间距
        public short LineMarginBottom;
        public short PageMarginTop;//页面间距
        public short PageMarginBottom;
        public short PageMarginLeft;
        public short PageMarginRight;
        public short MarginTop;//字间距
        public short MarginBottom;
        public short MarginLeft;
        public short MarginRight;
        public short ParagraphFirstLineMarginLeft;//一段的首行是否要偏移
    }
}
