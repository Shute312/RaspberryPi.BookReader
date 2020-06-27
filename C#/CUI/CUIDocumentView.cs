using CCommonLib;
using CGraphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public unsafe class CUIDocumentView : CUIViewProperties
    {
        public CUIView _Self { get; set; }
        public CUIView _Parent { get; set; }

        public CUIDocumentStyle Style { get; set; }

        public CDocumentInfo Info { get; set; }

        public Charset Charset;

        /// <summary>
        /// 用到Info中，缓存的哪个位置
        /// </summary>
        public Int32 UnicodeStart { get; set; }
        
        /// <summary>
        /// 向上翻页时，终点的位置
        /// </summary>
        public Int32 PrePageUnicodeEnd { get; set; }

        /// <summary>
        /// 向下翻页时，起点的位置
        /// </summary>
        public Int32 NextPageUnicodeStart { get; set; }
        public Int32 UnicodeEnd { get; set; }

        ////当前页的每一行都标记出来，方便上下翻页
        //public CTextInfo[] Lines;
        //public Int32 LineSize;
        //public Int32 StartLineIndex;
        //public Int32 EndLineIndex;
    }
}
