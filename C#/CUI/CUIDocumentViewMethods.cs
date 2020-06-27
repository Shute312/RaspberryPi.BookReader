using CCommonLib;
using CGraphic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            document.UnicodeStart = document.NextPageUnicodeStart;
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
                    Contract.Assert(document.Style.RemainLines < 3);

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
                    //读取一段文本，然后一直显示到无法显示      
                    Int32 sectionEnd = document.UnicodeStart;
                    document.UnicodeEnd = document.UnicodeStart;
                    Int32 sectionStart = sectionEnd;
                    Int32 limited = document.Info.UnicodeSize - 1;
                    ushort* pUnicodes = document.Info.Unicodes;

                    Int32 currentY = contentRect.MinX;//用于逐行渲染
                    Int32 lineCount = 0;
                    document.PrePageUnicodeEnd = document.UnicodeStart;
                    Int32[] lastTwoPosition = new Int32[2] { document.UnicodeStart, document.UnicodeStart };//记录最后两行的位置，用于翻页显示上一页的一两行
                    for (int i = document.UnicodeStart; i < document.Info.UnicodeSize; i++)
                    {
                        ushort unicode = pUnicodes[i];
                        sectionEnd++;
                        if (unicode == '\n' || (unicode == '\r' && i < limited && pUnicodes[i + 1] == '\n') || i == limited)
                        {
                            if (unicode == '\r' && i < limited && pUnicodes[i + 1] == '\n')
                            {
                                i++;
                                sectionEnd++;
                            }
                            if (sectionEnd > sectionStart)
                            {
                                //行首空两格
                                CPoint startPosition = new CPoint() { X = 0, Y = 0 };
                                bool isSectionStart = false;//是不是段落的起始位置
                                if (document.UnicodeStart == 0 || document.Info.Unicodes[sectionStart - 1] == '\n')
                                {
                                    isSectionStart = true;
                                }
                                if (isSectionStart)
                                {
                                    startPosition.X = document.Style.SectionFirstLineMarginLeft;
                                    //currentY += document.Style.SectionMarginTop;
                                }

                                while (true)
                                {
                                    validRect.MinY = currentY + document.Style.LineMarginTop;
                                    validRect.MaxY = validRect.MinY + fontInfo.Height + 1;//逐行绘制
                                    currentY = currentY + document.Style.LineMarginTop + fontInfo.Height + document.Style.LineMarginBottom;
                                    if (validRect.MaxY > contentRect.MaxY)
                                    {
                                        break;
                                    }
                                    lineCount++;
                                    CRect textRect = new CRect();
                                    textInfo.Start = sectionStart;
                                    textInfo.End = sectionEnd;
                                    int textLength = CG.DrawText(ref layer.Bitmap, textInfo, fontInfo, validRect, startPosition, out textRect);
                                    if (textLength == 0)
                                    {
                                        break;
                                    }
                                    validRect.MinY = textRect.MaxY;
                                    if (validRect.MinY >= contentRect.MaxY)
                                    {
                                        break;
                                    }
                                    lastTwoPosition[1] = lastTwoPosition[0];
                                    lastTwoPosition[0] = document.UnicodeEnd;
                                    document.UnicodeEnd += textLength;
                                    sectionStart += textLength;
                                    if (sectionStart >= sectionEnd)
                                    {
                                        break;
                                    }
                                    startPosition.X = 0;//只有第一行要空2个格子
                                    if (lineCount == document.Style.RemainLines)
                                    {
                                        document.PrePageUnicodeEnd = document.UnicodeEnd;
                                    }
                                }

                                if (currentY >= contentRect.MaxY)
                                {
                                    //var arr = new char[textInfo.End - textInfo.Start];
                                    //for (int m = 0; m < arr.Length; m++)
                                    //{
                                    //    arr[m] = textInfo.Text[m + textInfo.Start];
                                    //}
                                    //var text = new string(arr);
                                    break;
                                }
                            }
                            sectionStart = sectionEnd;
                        }
                    }
                    if (document.Style.RemainLines > 0)
                    {
                        document.NextPageUnicodeStart = lastTwoPosition[document.Style.RemainLines - 1];
                    }
                    else
                    {
                        document.NextPageUnicodeStart = document.UnicodeEnd;
                    }
                }
            }
        }
    }
}
