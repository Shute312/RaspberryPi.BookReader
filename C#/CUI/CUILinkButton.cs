using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public struct CUILinkButton: CUIViewProperties
    {
        public CUIView _Self { get; set; }
        public CUIView _Parent { get; set; }
    }
}
