using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CReader
{
    public struct CCacheHead
    {
        public byte[] FileType;//4字节
        public ushort Version;//2字节，编码版本
        public ushort HeadSize;//2字节，头信息长度，亦表示内容所在位置的偏移量(2字节)
        public Int32 SourceModifyTime;//4字节，原文件的修改时间，如果原文件被修改，则格式化缓存也要更新
        public Int32 SourceLength;//4字节，原文件的长度

        public byte[] HeadBuffer;//整个头的数据
    }
}
