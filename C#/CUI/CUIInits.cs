using CCommonLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CUI
{
    public static class CUIInits
    {
        private static UInt32 _FrameUniqueCode = 0;

        public static void InitFrame(out CUIFrame frame,in Int32 x, in Int32 y, in Int32 width, in Int32 height,in CPixelFormat format,in CUIFrameType type)
        {
            frame = new CUIFrame();
            frame.HashCode = ++_FrameUniqueCode;//从1开始
            frame.ChildrenSize = 0;
            frame.IsEnabled = true;
            frame.Location = new CPoint() { X = x, Y = y };
            frame.BackColor = 0xFF000000;
            frame.ForeColor = 0xFF000000;
            frame.BorderColor = 0xFF000000;
            frame.BorderThickness = 0;
            frame.FocousedView = 0;
            frame.IsVisible = true;
            frame.Size = new CSize() { Width = width, Height = height };
            frame._Layer = new CUILayer();

            CGraphic.CGraphicInits.InitBitmap(out frame._Layer.Bitmap, format, width, height);

            frame._Layer.ModifyRect = new CRect() { MinX = 0, MaxX = 0, MinY = 0, MaxY = 0 };



            switch (type)
            {
                case CUIFrameType.Frame:
                    throw new ArgumentException();
                    break;
                case CUIFrameType.Main:
                    var main = new CUIFrameMain();
                    main._Self = frame;
                    frame.Properties = main;
                    break;
                default:
                    break;
            }
        }

        public static void InitFrame0(out CUIFrame frame,in CUIFrameType type)
        {
            InitFrame(out frame, 0, 0, CUIEnvironment.WidthOfPixel, CUIEnvironment.HeightOfPixel, CPixelFormat.Format8bppIndexed, type);
        }
    }
}
