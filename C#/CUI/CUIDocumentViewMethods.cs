using CCommonLib;
using CGraphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public static class CUIDocumentViewMethods
    {
        public static bool Begin(ref CUIDocumentView document, in Charset charset, in Int32 position)
        {
            return false;
        }

        public static bool PageDown(ref CUIDocumentView document)
        {
            //document.Info.StreamPositon += (document.BuffEnd - document.BuffStart) << 1;
            //这里不对，会导致无法实现保留n行的逻辑
            document.BuffStart = document.BuffEnd;
            return true;
        }

        public static bool PageUp(ref CUIDocumentView document)
        {
            return false;
        }

        public static bool IsEnablePageDown(ref CUIDocumentView document)
        {
            return true;
        }
        public static bool IsEnablePageUp(ref CUIDocumentView document)
        {
            return true;
        }

        public static unsafe void RenderDocumentView(ref CUIView child, ref CUILayer layer, in CRect contentRect)
        {
            if (ValueFuns.IsValidRect(contentRect))
            {
                CUIDocumentView document = (CUIDocumentView)child.Properties;
                if (!document.Info.IsEnd && child.ForeColor >> 24 != 0xFF)
                {
                    CTextInfo textInfo = new CTextInfo()
                    {
                        Start = document.BuffStart,
                        End = document.Info.UnicodeSize,
                        Length = document.Info.UnicodeSize,
                        Text = (char*)document.Info.Unicodes//todo 这里要从buff中读取转码

                    };
                    CFontInfo fontInfo;
                    CG.GetValidFontSize(child.FontSize, out fontInfo); ;
                    CPoint startPosition;
                    //todo未完成的段落，不应该有首行空两格的逻辑
                    if (document.Style.ParagraphFirstLineMarginLeft > 0)
                    {
                        if (document.BuffStart > 4)
                        { 
                        }
                        startPosition = new CPoint() { X = document.Style.ParagraphFirstLineMarginLeft, Y = 0 };
                    }
                    else
                    {
                        startPosition = new CPoint() { X = 0, Y = 0 };
                    }
                    CRect textRect = new CRect();
                    var renderTextLength = CG.MeasureText(textInfo, fontInfo, startPosition, contentRect, out textRect);
                    if (renderTextLength > 0)
                    {
                        if (renderTextLength == child.TextSize)//可以完整显示所有内容
                        {
                            //获得绘制需要的空间后，居中显示
                            CG.DrawText(ref layer.Bitmap, textInfo, fontInfo, contentRect, startPosition, out textRect);
                        }
                        else
                        {
                            //显示不全，而且只能显示一行，则垂直方向上居中
                            //而因为不够显示的话，所以水平方向上，左对齐
                            int textLength = CG.DrawText(ref layer.Bitmap, textInfo, fontInfo, contentRect, startPosition, out textRect);
                            document.BuffEnd = document.BuffStart + textLength;
                        }
                    }
                }
            }
        }
    }
}
