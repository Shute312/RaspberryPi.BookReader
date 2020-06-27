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

    public struct CUIStyle
    {

        public UInt32 FontColor;//前景色,通常为文本颜色(高8位为0xFF时，因全透明而被忽略)
        public Int32 FontSize;//字号
        
        public UInt32 BackColor;//背景色(高8位为0xFF时，因全透明而被忽略)

        public Int32 BorderThickness;//边框线条粗细
        public UInt32 BorderColor;//边框线条颜色(高8位为0xFF时，因全透明而被忽略)
    }
}
