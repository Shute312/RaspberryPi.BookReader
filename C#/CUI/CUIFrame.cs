using CCommonLib;
using CGraphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public unsafe class CUIFrame
    {
        public UInt32 HashCode;//这个不应该被改变的
        public CUIFrameProperties Properties;//注意，这个是指针，
        public CUIFrameType FrameType;
        public CUILayer _Layer;

        //public CUIView[] Children;
        public CUIView[] Children;
        public Int32 ChildrenSize;

        public CSize Size;//宽高
        public CPoint Location;//位置
        public bool IsVisible;//是否显示，如果为false,则不参与显示
        public bool IsEnabled;//是否生效，为false时，不接收消息

        public UInt32 BackColor;//背景色(高8位为0xFF时，因全透明而被忽略)
        public UInt32 ForeColor;//前景色,通常为文本颜色(高8位为0xFF时，因全透明而被忽略)
        public UInt32 BorderThickness;//边框线条粗细
        public UInt32 BorderColor;//边框线条颜色(高8位为0xFF时，因全透明而被忽略)

        public UInt32 FocousedView;//聚焦的控件(注意，这是个指针)

    }
}
