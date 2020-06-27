using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCommonLib
{
    //参考C# PixelFormat 枚举
    public enum CPixelFormat
    {
        //
        // 摘要:
        //     指定格式为每像素 24 位；红色、绿色和蓝色分量各使用 8 位。
        Format24bppRgb = 137224,
        //
        // 摘要:
        //     指定像素格式为每像素 1 位，并指定它使用索引颜色。 因此颜色表中有两种颜色。
        Format1bppIndexed = 196865,
        //
        // 摘要:
        //     指定格式为每像素 4 位而且已创建索引。
        Format4bppIndexed = 197634,
        //
        // 摘要:
        //     指定格式为每像素 8 位而且已创建索引。 因此颜色表中有 256 种颜色。
        Format8bppIndexed = 198659,
        //
        // 摘要:
        //     指定格式为每像素 32 位；alpha、红色、绿色和蓝色分量各使用 8 位。
        Format32bppArgb = 2498570,
    }

    public enum Charset
    {
        Unknown=0,
        Unicode=2,
        Utf8=3,
    }
}
