using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public struct CUIDocumentStyle
    {
        public Int32 LineMarginTop;//行间距
        public Int32 LineMarginBottom;
        //public Int32 PageMarginTop;//页面间距
        //public Int32 PageMarginBottom;
        //public Int32 PageMarginLeft;
        //public Int32 PageMarginRight;
        public Int32 MarginTop;//字符间距
        public Int32 MarginBottom;
        public Int32 MarginLeft;
        public Int32 MarginRight;
        public Int32 ParagraphFirstLineMarginLeft;//每一段的首行的偏移量
        public byte RemainLines { get; set; }// 翻页时，保留上一页多少行
    }
}
