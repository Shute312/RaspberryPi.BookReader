using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCommonLib
{
    public struct CPoint
    {
        public Int32 X;
        public Int32 Y;
    }

    public struct CSize
    {
        public Int32 Width;
        public Int32 Height;
    }
    public struct CRect
    {
        public Int32 MinX;
        public Int32 MinY;

        //最大值不能为MaxX\MaxY
        public Int32 MaxX;
        public Int32 MaxY;
    }

    public struct CPadding
    {
        public Int32 Top;
        public Int32 Left;
        public Int32 Right;
        public Int32 Bottom;
    }

    public unsafe struct DocumentInfo
    {
        public Int32 DocumentOffset;//文档从第几个字节才是正式内容
        /// <summary>
        /// 当前显示的内容，是在第几个字节
        /// </summary>
        public Int32 StreamPositon;

        public Int32 BufferSize;//当前Buffer中的有效字符(ushort)个数

        public ushort* Buffer;//字符

        /// <summary>
        /// 文件长度(字节),-1表示未知长度
        /// </summary>
        public Int32 FileSize;

        //文件是否到底了
        public bool IsEnd;

        public string Path;//文件路径
    }
}
