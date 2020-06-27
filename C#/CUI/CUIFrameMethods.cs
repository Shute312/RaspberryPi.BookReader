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
    public static class CUIFrameMethods
    {
        public static void Create(out CUIFrame frame,in CUIFrameType type)
        {
            CUIInits.InitFrame0(out frame,type);
        }

        public static void AddToFrame(ref CUIFrame frame, in CUIView view)
        {
            if (frame.Children == null)
            {
                frame.Children = new CUIView[] { view };
            }
            else
            {
                var old = frame.Children;
                frame.Children = new CUIView[frame.ChildrenSize + 1];
                for (int i = 0; i < frame.ChildrenSize; i++)
                {
                    frame.Children[i] = old[i];
                }
                frame.Children[frame.ChildrenSize] = view;
            }
            frame.ChildrenSize++;
        }
        public static bool RemoveFromFrame(ref CUIFrame frame, in CUIView view)
        {
            Int32 index = GetChildViewIndex(frame, view);
            if (index > -1)
            {
                //for C要重新申请指针数组空间()
                if (frame.ChildrenSize == 1)
                {
                    frame.Children = null;
                }
                else
                {
                    var temp = new CUIView[frame.ChildrenSize - 1];
                    for (int i = 0; i < index; i++)
                    {
                        temp[i] = frame.Children[i];
                    }
                    for (int i = index; i < frame.ChildrenSize; i++)
                    {
                        temp[i-1] = frame.Children[i];
                    }
                    frame.Children = temp;
                    //for c 注意要释放旧的那一段
                }
                frame.ChildrenSize--;
            }
            return index > -1;
        }

        public static void GetChildView(in CUIFrame frame, in Int32 index,out CUIView child)
        {
            Contract.Assert(index > -1 && index < frame.ChildrenSize);
            child = frame.Children[index];
        }

        public static void SetSize(ref CUIFrame frame, in CSize size)
        {
            Contract.Assert(size.Width > 0 && size.Height > 0);
            frame.Size = size;
            if (frame._Layer.Bitmap.Width != size.Width || frame._Layer.Bitmap.Height != size.Height)
            {
                //todo C:要处理好内存释放(frame._Layer.Bitmap.Buffer)
                CGraphicInits.InitBitmap(out frame._Layer.Bitmap, frame._Layer.Bitmap.Format, size.Width, size.Height);
                frame._Layer.ModifyRect = new CRect() { MinX = 0, MaxX = 0, MinY = 0, MaxY = 0 };
            }
        }

        public static Int32 GetChildViewIndex(in CUIFrame frame, in CUIView view)
        {

            Int32 index = -1;
            //todo 释放内存
            for (int i = 0; i < frame.ChildrenSize; i++)
            {
                if (frame.Children[i].HashCode == view.HashCode)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public unsafe static bool GetFocusedView(in CUIFrame frame, out CUIView view)
        {

            for (int i = 0; i < frame.ChildrenSize; i++)
            {
                if (frame.Children[i].HashCode == frame.FocousedView)
                {
                    view = frame.Children[i];
                    return true;
                }
            }
            //for c:view为NULL
            view = new CUIView();
            return false;
        }

        public static Int32 GetChildViewIndexByHashCode(in CUIFrame frame, in UInt32 hashcode)
        {

            Int32 index = -1;
            //todo 释放内存
            for (int i = 0; i < frame.ChildrenSize; i++)
            {
                if (frame.Children[i].HashCode == hashcode)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static void SetLocation(ref CUIFrame frame, in CPoint location,in CSize size)
        {
            frame.Location = location;
            frame._Layer.Location = location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame">frame内部的绘制区域，已忽略偏移</param>
        /// <param name="frameRect"></param>
        public static void Refresh(ref CUIFrame frame, in CRect frameRect)
        {
            for (int i = 0; i < frame.ChildrenSize; i++)
            {
                //如果有改动
                if (frame.Children[i].IsVisible)
                {
                    RefreshChild(ref frame.Children[i], ref frame._Layer, frameRect);
                }
            }
        }

        private unsafe static void RefreshChild(ref CUIView child, ref CUILayer layer ,in CRect rect)
        {
            fixed (CRect* modifyRect = &layer.ModifyRect)
            {
                if (child.IsVisible)
                {
                    CRect innerRect = new CRect();

                    innerRect.MinX = child.Location.X + child.Padding.Left;
                    innerRect.MinY = child.Location.Y + child.Padding.Top;
                    innerRect.MaxX = child.Location.X + child.Size.Width - child.Padding.Right;
                    innerRect.MaxY = child.Location.Y + child.Size.Height - child.Padding.Bottom;
                    //ValueFuns.SetToMaxInt32(ref innerRect.MinX, 0);
                    //ValueFuns.SetToMaxInt32(ref innerRect.MinY, 0);
                    //ValueFuns.SetToMinInt32(ref innerRect.MaxX, child.Location.X + child.Size.Width);
                    //ValueFuns.SetToMinInt32(ref innerRect.MaxY, child.Location.Y + child.Size.Height);

                    if (!ValueFuns.IsValidRect(innerRect))
                    {
                        return;
                    }
                    //如果没有交集，则不用绘制；如果有交集，则先绘制后，再根据rect在输出时做裁切
                    if (!ValueFuns.MinRect(rect, innerRect, ref innerRect))
                    {
                        return;
                    }

                    //背景色(目前半透明当不透明来处理)
                    if (child.BackColor >> 24 != 0xFF)
                    {
                        CG.FillRect(ref layer.Bitmap, innerRect, child.BackColor);
                    }
                    //边框
                    if (child.BorderColor >> 24 != 0xFF && child.BorderThickness > 0)
                    {
                        if (child.IsFocused)
                        {
                            //边框色取反
                            CG.DrawRectangle(ref layer.Bitmap, child.BorderThickness, innerRect, (~child.BorderColor)&0x00FFFFFF);
                        }
                        else
                        {
                            CG.DrawRectangle(ref layer.Bitmap, child.BorderThickness, innerRect, child.BorderColor);
                        }
                    }

                    CRect contentRect = new CRect()
                    {
                        MinX = innerRect.MinX + child.BorderThickness,
                        MinY = innerRect.MinY + child.BorderThickness,
                        MaxX = innerRect.MaxX - child.BorderThickness,
                        MaxY = innerRect.MaxY - child.BorderThickness
                    };
                    //CGraphic.CGraphic.DrawBitmap();
                    switch (child.UIType)
                    {
                        case CUIViewType.View:
                            break;
                        case CUIViewType.Panel:
                            break;
                        case CUIViewType.Button:
                            if (ValueFuns.IsValidRect(contentRect))
                            {
                                if (child.TextSize > 0 && child.ForeColor >> 24 != 0xFF)
                                {
                                    CTextInfo textInfo = new CTextInfo()
                                    {
                                        Start = 0,
                                        End = child.TextSize,
                                        Length = child.TextSize,
                                        Text = child.Text
                                    };
                                    CPoint startPosition = new CPoint() { X = 0, Y = 0 };
                                    CFontInfo fontInfo;
                                    CG.GetValidFontSize(child.FontSize, out fontInfo);
                                    CRect textRect = new CRect();
                                    var renderTextLength = CG.MeasureText(textInfo, fontInfo, startPosition, contentRect, out textRect);
                                    if (renderTextLength > 0)
                                    {
                                        if (renderTextLength == child.TextSize)//可以完整显示所有内容
                                        {
                                            //获得绘制需要的空间后，居中显示
                                            startPosition.X = (contentRect.MaxX - textRect.MaxX) / 2;
                                            startPosition.Y = (contentRect.MaxY - textRect.MaxY) / 2;
                                            CG.DrawText(ref layer.Bitmap, textInfo, fontInfo, contentRect, startPosition, out textRect);
                                        }
                                        else
                                        {
                                            //显示不全，而且只能显示一行，则垂直方向上居中
                                            if (textRect.MaxY < contentRect.MaxY)
                                            {
                                                startPosition.Y = (contentRect.MaxY - textRect.MaxY) / 2;
                                            }
                                            //而因为不够显示的话，所以水平方向上，左对齐
                                            CG.DrawText(ref layer.Bitmap, textInfo, fontInfo, contentRect, startPosition, out textRect);
                                        }
                                    }
                                }
                            }
                            break;
                        case CUIViewType.LinkButton:
                            break;
                        case CUIViewType.Label:
                            break;
                        case CUIViewType.Document:
                            CUIDocumentViewMethods.RenderDocumentView(ref child, ref layer,contentRect);
                            break;
                        default:
                            break;
                    }
                    ValueFuns.MaxRect(*modifyRect, innerRect, ref *modifyRect);
                }
            }
        }
        public static void Refresh() {
            for (int i = 0; i < CUIApp._Length; i++)
            {
                if(CUIApp._Frames[i].IsVisible)
                {
                    Refresh(ref CUIApp._Frames[i], new CRect() { MinX = 0, MinY = 0, MaxX = CUIApp._Frames[i].Size.Width, MaxY = CUIApp._Frames[i].Size.Height });
                }
            }
        }

        public static void Refresh(ref CUIFrame frame)
        {
            //刷新整个可视区域
            Refresh(ref frame, new CRect() { MinX = 0, MinY = 0, MaxX = frame.Size.Width, MaxY = frame.Size.Height });
        }

        public static bool Render(in CUIFrame frame, ref CBitmap context, bool isFullRefresh)
        {
            bool isModify = false;
            CRect rect = isFullRefresh ? new CRect() { MinX = 0, MinY = 0, MaxX = frame.Size.Width, MaxY = frame.Size.Height } : frame._Layer.ModifyRect;
            if (ValueFuns.IsValidRect(rect))
            {
                CG.DrawBitmap(frame._Layer.Bitmap, rect, ref context, new CRect() { MinX = 0, MinY = 0, MaxX = context.Width, MaxY = context.Height }, frame.Location);
                isModify = true;
            }
            return isModify;
        }

        public static bool SetFocus(ref CUIFrame frame, in CUIView view)
        {
            Int32 index = GetChildViewIndexByHashCode(frame, view.HashCode);
            if (index>-1)
            {
                if (frame.FocousedView == view.HashCode)
                {
                    return true;
                }
                if (frame.FocousedView != 0)
                {
                    for (int i = 0; i < frame.ChildrenSize; i++)
                    {
                        //if (frame.Children[i].HashCode == frame.FocousedView)
                        //{
                        //    frame.Children[i].IsFocused = false;
                        //    break;
                        //}
                        frame.Children[i].IsFocused = false;
                    }
                }
                frame.FocousedView = view.HashCode;
                frame.Children[index].IsFocused = true;
            }
            return index > -1;
        }
    }
}
