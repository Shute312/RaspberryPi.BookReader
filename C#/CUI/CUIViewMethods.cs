using CCommonLib;
using CGraphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public static class CUIViewMethods
    {
        private static UInt32 _ViewUniqueCode = 0;

        public static unsafe CUIView Create(in CUIViewType type)
        {
            CUIView view = new CUIView();
            view = new CUIView();
            view.UIType = type;
            view.HashCode = ++_ViewUniqueCode;//从1开始
            CFontInfo fontInfo;
            CG.GetValidFontSize(CUIEnvironment.FontSize, out fontInfo);
            view.Style.FontSize = fontInfo.FontSize;
            view.Style.FontColor = 0x000000FF;//文本颜色
            view.Style.BackColor = 0xFF0000EE;//无背景色
            view.Style.BorderColor = 0xFF000033;//无边框
            view.Style.BorderThickness = 0;//无边框

            view.ActiveStyle.BackColor =  view.Style.BackColor;
            view.ActiveStyle.BorderColor = view.Style.FontColor;
            view.ActiveStyle.BorderThickness = view.Style.BorderThickness;
            view.ActiveStyle.FontSize = view.Style.FontSize;
            view.ActiveStyle.FontColor = view.Style.FontColor;

            view.IsVisible = true;
            Int32 charWidth = fontInfo.Width;//一个字符要占多大空间
            Int32 baseWidth = charWidth * 8;//至少放n个字符;
            Int32 baseHeight = charWidth;
            ValueFuns.SetToMinInt32(ref baseWidth, CUIEnvironment.WidthOfPixel);
            ValueFuns.SetToMinInt32(ref baseHeight, CUIEnvironment.HeightOfPixel);
            view.Size = new CSize() { Width = baseWidth, Height = baseHeight };

            switch (type)
            {
                case CUIViewType.View:
                    throw new ArgumentException();
                    break;
                case CUIViewType.Panel:
                    break;
                case CUIViewType.Button:
                    CUIButton button = new CUIButton();
                    view.Properties = button;
                    break;
                case CUIViewType.LinkButton:
                    CUILinkButton link = new CUILinkButton();
                    view.Properties = link;
                    break;
                case CUIViewType.Label:
                    break;
                case CUIViewType.Document:
                    var properties = new CUIDocumentView();
                    properties._Self = view;
                    view.Properties = properties;

                    break;
                default:
                    break;
            }
            return view;
        }
    }
}
