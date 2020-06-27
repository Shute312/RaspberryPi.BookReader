using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CReader
{
    public struct CFileCache
    {
        public byte[] FileType;//4字节
        public byte[] Version;//2字节，编码版本
        public byte[] HeadSize;//2字节，头信息长度，亦表示内容所在位置的偏移量(2字节)
        public byte[] SourceModifyTime;//4字节，原文件的修改时间，如果原文件被修改，则格式化缓存也要更新
        public byte[] SourceLength;//4字节，原文件的长度

        public string Path;
        public string SourcePath;
    }
}
