using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCommonLib;
using CGraphic;

namespace CUI
{
    public unsafe class CUIView
    {
        public UInt32 HashCode;//这个不应该被改变的
        public CUIViewProperties Properties;//注意，这个是指针，

        public CSize Size;//宽高
        public CPoint Location;//位置
        public CPadding Padding;//偏移位置
        public bool IsVisible;//是否显示，如果为false,则不参与显示
        public bool IsEnabled;//是否生效，为false时，不接收消息
        public bool IsFocused;//是否焦点控件

        //public CUIView[] Children;//二级指针
        //public Int32 ChildrenSize;//有多少Children
        public bool IsPanel;//是否容器

        public CUIViewType UIType;

        public UInt32 ForeColor;//前景色,通常为文本颜色(高8位为0xFF时，因全透明而被忽略)
        //public string Text;//for C 改为ushort*
        public char* Text;//for C 改为ushort*
        public Int32 TextSize;//文字的长度，注意：采用char可能不能正确显示，要用ushort才能显示
        
        //todo 抽象出Style的概念，空间激活(focus)时，有额外的Style
        public CUIStyle Style;
        public CUIStyle ActiveStyle;//激活、Focused时展示的样式

    }
}
