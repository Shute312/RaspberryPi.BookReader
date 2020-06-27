using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CUI
{
    public enum CUIViewType
    {
        View = 0,
        Panel = 1,
        Button = 10,
        LinkButton = 11,
        Label = 20,

        Document = 100,
    }

    public enum CUIFrameType
    {
        Frame=0,
        Main=1,
    }

    public enum ScreenSize
    {
        //2-4英寸(太小就不做支持)
        Small = 4,
        //4-8英寸
        Normal = 8,
        //8-16英寸
        Large = 16,
        //16-32英寸
        Huge = 32,
    }
}
