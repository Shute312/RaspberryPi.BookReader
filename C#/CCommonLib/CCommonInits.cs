using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCommonLib
{
    public static class CCommonInits
    {
        public static void InitRect(out CRect rect, in Int32 minX, Int32 minY, Int32 maxX, Int32 maxY)
        {
            rect = new CRect();
            rect.MinX = minX;
            rect.MinY = minY;
            rect.MaxX = maxX;
            rect.MaxY = maxY;
        }
        public static void InitRect0(out CRect rect)
        {
            rect = new CRect();
            rect.MinX = 0;
            rect.MinY = 0;
            rect.MaxX = 0;
            rect.MaxY = 0;
        }
        public static void InitPoint(out CPoint point, in Int32 x, Int32 y)
        {
            point = new CPoint();
            point.X = x;
            point.Y = y;
        }
        public static void InitPoint0(out CPoint point)
        {
            point = new CPoint();
            point.X = 0;
            point.Y = 0;
        }
        public static void InitSize(out CSize size, in Int32 width, Int32 height)
        {
            size = new CSize();
            size.Width = width;
            size.Height = height;
        }
        public static void InitSize0(out CSize size)
        {
            size = new CSize();
            size.Width = 0;
            size.Height = 0;
        }
        public static void InitPadding(out CPadding padding, in Int32 left, Int32 right, in Int32 top, Int32 bottom)
        {
            padding = new CPadding();
            padding.Left = left;
            padding.Right = right;
            padding.Top = top;
            padding.Bottom = bottom;
        }
        public static void InitPaddingAll(out CPadding padding, in Int32 value)
        {
            padding = new CPadding();
            padding.Left = value;
            padding.Right = value;
            padding.Top = value;
            padding.Bottom = value;
        }
        public static void InitPadding0(out CPadding padding)
        {
            padding = new CPadding();
            padding.Left = 0;
            padding.Right = 0;
            padding.Top = 0;
            padding.Bottom = 0;
        }
    }
}
