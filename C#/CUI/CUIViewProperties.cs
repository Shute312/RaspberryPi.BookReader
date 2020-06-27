using CGraphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public interface CUIViewProperties
    {
        CUIView _Self { get; set; }
        CUIView _Parent { get; set; }
    }
}
