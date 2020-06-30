using CCommonLib;
using CGraphic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CUI
{
    /// <summary>
    /// 后续Frame、View通过函数注册的方式，模拟函数重载
    /// </summary>
    public static class CUIApp
    {
        internal static CUIFrame[] _Frames;
        internal static Int32 _Index;
        internal static Int32 _Length;

        public static void Init(in byte bitsPerPixel,in Int32 widthOfPixel,in Int32 heightofPixel,in float widthOfInch,in float heightOfInch)
        {
            CUIEnvironment.BitsPerPixel = bitsPerPixel;
            CUIEnvironment.WidthOfPixel = widthOfPixel;
            CUIEnvironment.HeightOfPixel = heightofPixel;
            CUIEnvironment.WidthOfInch = widthOfInch;
            CUIEnvironment.HeightOfInch = heightOfInch;
            var inch = Math.Sqrt(widthOfInch*widthOfInch+heightOfInch*heightOfInch);
            if (inch < 1)
            {
                throw new NotImplementedException();
            }
            else if (inch > 16)
            {
                CUIEnvironment.ScreenSize = ScreenSize.Huge;
            }
            else if (inch > 8)
            {
                CUIEnvironment.ScreenSize = ScreenSize.Large;
            }
            else if (inch > 4)
            {
                CUIEnvironment.ScreenSize = ScreenSize.Normal;
            }
            else if (inch > 2)
            {
                CUIEnvironment.ScreenSize = ScreenSize.Small;
            }
            CUIEnvironment.UpdateDefaultFontSize();
        }

        public static void Add(in CUIFrame frame)
        {
            Contract.Assert(frame.Properties != null);
            if (_Frames == null)
            {
                _Frames = new CUIFrame[1] { frame };
                _Length = 1;
                _Index = 0;
            }
            else
            {
                var old = _Frames;
                _Frames = new CUIFrame[_Length + 1];
                for (int i = 0; i < _Length; i++)
                {
                    _Frames[i] = old[i];
                    _Frames[i].IsEnabled = false;
                }
                _Frames[_Length] = frame;
                _Index = _Length;
                _Length++;
            }
            _Frames[_Index].IsEnabled = true;
            //Refresh();
        }

        public static void Remove(in CUIFrame frame)
        {
            var index = GetIndex(frame);
            if (index == -1)
            {
                return;
            }
            if (_Length == 1)
            {
                _Frames = null;
                _Length = 0;
                _Index = -1;
                return;
            }
            var newFrame = new CUIFrame[_Length - 1];
            for (int i = 0; i < index; i++)
            {
                newFrame[i] = _Frames[i];
            }
            for (int i = _Length - 1; i > index; i--)
            {
                newFrame[i-1] = _Frames[i];
            }
            _Frames = newFrame;
            _Length--;
            _Index--;
            if (_Index<0)
            {
                _Index = 0;
            }
        }

        public static bool Front(in CUIFrame frame)
        {
            var index = GetIndex(frame);
            if (index < 0) return false;
            if (index == _Length - 1) return true;
            Int32 max = _Length - 1;
            _Frames[_Length-2].IsEnabled = false;
            for (int i = index; i < max;)
            {
                _Frames[i] = _Frames[++i];
            }
            _Frames[max] = frame;
            _Frames[max].IsEnabled = true;
            Refresh();
            return true;
        }

        public static Int32 GetIndex(in CUIFrame frame)
        {
            for (int i = 0; i < _Length; i++)
            {
                if (_Frames[i].HashCode == frame.HashCode)
                {
                    return i;
                }
            }
            return -1;
        }

        //输出渲染效果
        public unsafe static bool Render(ref CBitmap context, in bool isFullRefresh)
        {
            bool isModify = false;
            for (int i = 0; i < _Length; i++)
            {
                if (_Frames[i].IsEnabled)
                {
                    isModify |= CUIFrameMethods.Render(_Frames[i],ref context, isFullRefresh);
                }
            }
            return isModify;
        }

        public static Int32 GetFrameCount()
        {
            return _Length;
        }

        public static bool GetActivedFrame(out CUIFrame frame)
        {
            if (_Index > -1 && _Index < _Length)
            {
                frame = _Frames[_Index];
                return true;
            }
            frame = new CUIFrame();//for C C中不需要new
            return false;
        }

        private static void Refresh()
        {
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            for (int i = 0; i < _Length; i++)
            {
                if (_Frames[i].IsEnabled)
                {
                    CUIFrameMethods.Refresh(ref _Frames[i]);
                }
            }
#if DEBUG
            var time = stopwatch.ElapsedMilliseconds;
#endif
        }

    }
}