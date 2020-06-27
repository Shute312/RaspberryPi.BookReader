using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCommonLib;
using CGraphic;

namespace CUI
{
    public struct CUILayer
    {
        //public Int32 Index;//图层位置，数值高的在上层
        public CBitmap Bitmap;//图像数据(指针)
        //public CRect Rect;//
        //public CSize Size;
        public CPoint Location;
        public CRect ModifyRect;//更新范围
    }
}
