using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public struct CUIButton : CUIViewProperties
    {

        public CUIView _Self { get; set; }
        public CUIView _Parent { get; set; }
    }
}
