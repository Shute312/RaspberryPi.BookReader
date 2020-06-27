using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCommonLib;

namespace CGraphic
{
    ////实际绘制点操作
    //public interface CRenderInterface
    //{
    //    void DrawPixel(in byte[] graphics, in Int32 width, in Int32 height, in byte bbp, in Int32 x,in Int32 y, UInt32 color);

    //    bool MeasureText(in char[] text, in Int32 start, Int32 length,in Int32 fontSize, out Int32 width, out Int32 height);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="graphics"></param>
    //    /// <param name="width"></param>
    //    /// <param name="height"></param>
    //    /// <param name="bbp"></param>
    //    /// <param name="text"></param>
    //    /// <param name="start"></param>
    //    /// <param name="length"></param>
    //    /// <param name="x"></param>
    //    /// <param name="y"></param>
    //    /// <returns>实际渲染个数</returns>
    //    Int32 DrawText(in byte[] graphics, in Int32 width, in Int32 height,in byte bbp, in char[] text, in Int32 start, in Int32 length, in Int32 x, in Int32 y);
    //}

    public delegate void DrawPixelDelegate(in CBitmap bitmap, in Int32 x, in Int32 y, UInt32 color);

    public delegate bool GetValiedFontSizeDelegate(in Int32 fontSize,out CFontInfo fontInfo);

    public delegate Int32 MeasureTextDelegate(in CTextInfo textInfo, in CFontInfo fontInfo, in CPoint startPosition, in CRect rect, out CRect renderRect);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bitmap"></param>
    /// <param name="textInfo"></param>
    /// <param name="fontInfo"></param>
    /// <param name="rect"></param>
    /// <param name="startPosition">相对于rect的，startPosition为0,0时，实际渲染在rect.MinX,MinY位置</param>
    /// <param name="renderRect"></param>
    /// <returns></returns>
    public delegate Int32 DrawTextDelegate(ref CBitmap bitmap, in CTextInfo textInfo, CFontInfo fontInfo, in CRect rect, in CPoint startPosition, out CRect renderRect);
}
