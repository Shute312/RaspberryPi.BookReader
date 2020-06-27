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
            document.UnicodeStart = document.UnicodeEnd;
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
                        Start = document.UnicodeStart,
                        End = document.Info.UnicodeSize,
                        Length = document.Info.UnicodeSize,
                        Text = (char*)document.Info.Unicodes//todo 这里要从buff中读取转码

                    };
                    CFontInfo fontInfo;
                    CG.GetValidFontSize(child.Style.FontSize, out fontInfo); ;

                    var validRect = new CRect
                    {
                        MinX = contentRect.MinX,
                        MaxX = contentRect.MaxX,
                        MinY = contentRect.MinY,
                        MaxY = contentRect.MaxY
                    };
                    //读取一行行文本，然后一直显示到无法显示      
                    Int32 lineEnd = document.UnicodeStart;
                    document.UnicodeEnd = document.UnicodeStart;
                    Int32 lineStart = lineEnd;
                    Int32 limited = document.Info.UnicodeSize - 1;
                    ushort* pUnicodes = document.Info.Unicodes;
                    for (int i = document.UnicodeStart; i < document.Info.UnicodeSize; i++)
                    {
                        ushort unicode = pUnicodes[i];
                        lineEnd++;
                        if (unicode == '\n' || (unicode == '\r' && i < limited && pUnicodes[i + 1] == '\n') || i == limited)
                        {
                            if (unicode == '\r' && i < limited && pUnicodes[i + 1] == '\n')
                            {
                                i++;
                                lineEnd++;
                            }
                            if (lineEnd > lineStart)
                            {
                                //行首空两格
                                CPoint startPosition = new CPoint() { X = 0, Y = 0 };
                                if (document.Style.ParagraphFirstLineMarginLeft > 0)
                                {
                                    if (document.UnicodeStart == 0)
                                    {
                                        startPosition = new CPoint() { X = document.Style.ParagraphFirstLineMarginLeft, Y = 0 };
                                    }
                                    else if (document.Info.Unicodes[lineStart - 1] == '\n')
                                    {
                                        startPosition = new CPoint() { X = document.Style.ParagraphFirstLineMarginLeft, Y = 0 };
                                    }
                                }
                                //todo读到一段后，这里应该一行行的绘制，更容易实现各种效果
                                //向上翻页也可以借鉴这个思路，逆向操作


                                //显示不全，而且只能显示一行，则垂直方向上居中
                                //而因为不够显示的话，所以水平方向上，左对齐
                                CRect textRect = new CRect();
                                textInfo.Start = lineStart;
                                textInfo.End = lineEnd;
                                int textLength = CG.DrawText(ref layer.Bitmap, textInfo, fontInfo, validRect, startPosition, out textRect);



                                validRect.MinY = textRect.MaxY;
                                if (validRect.MinY >= contentRect.MaxY)
                                {
                                    var arr = new char[textInfo.End - textInfo.Start];
                                    for (int m = 0; m < arr.Length; m++)
                                    {
                                        arr[m] = textInfo.Text[m + textInfo.Start];
                                    }
                                    var text = new string(arr);
                                    break;
                                }
                                document.UnicodeEnd += textLength;
                            }
                            lineStart = lineEnd;
                        }
                    }
                }
            }
        }
    }
}
