using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EInkLib
{
    public struct FontMargin
    {
        public short LineMarginTop;//行间距
        public short LineMarginBottom;
        public short PageMarginTop;
        public short PageMarginBottom;
        public short PageMarginLeft;
        public short PageMarginRight;
        public short MarginTop;
        public short MarginBottom;
        public short MarginLeft;
        public short MarginRight;
        public short ParagraphFirstLineMarginLeft;//一段的首行是否要偏移
    }
}
