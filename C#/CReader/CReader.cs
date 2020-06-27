using CCommonLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace CReader
{
    public static class CReader
    {
        const Int32 FormatCacheHeadSize = 128;
        public static bool ReadHead(in string srcPath,in string cachePath, out CCacheHead head)
        {
            Contract.Assert(!string.IsNullOrEmpty(cachePath) && !string.IsNullOrEmpty(srcPath));
            head = new CCacheHead();
            if (!File.Exists(srcPath) || !File.Exists(cachePath))
            {
                return false;
            }
            using (Stream stream = File.Open(cachePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buff = new byte[16];
                if (stream.Read(buff, 0, buff.Length) != buff.Length)
                {
                    return false;
                }
                Int32 length = ValueFuns.BytesToInt32(buff,12,4);
                if (length < 0)
                {
                    return false;
                }
                using (Stream dstStream = File.Open(srcPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (dstStream.Length != length)
                    {
                        return false;
                    }
                }
                DateTime modifyTime = File.GetLastWriteTime(srcPath);
                if (modifyTime.Day != buff[8] ||
                    modifyTime.Hour != buff[9] ||
                    modifyTime.Minute != buff[10] ||
                    modifyTime.Second != buff[11])
                {
                    return false;
                }
                head.HeadSize = (ushort)((((ushort)buff[6])<<8 )| buff[7]);
                if (stream.Length < head.HeadSize)
                {
                    return false;
                }
                head.HeadBuffer = new byte[head.HeadSize];
                Buffer.BlockCopy(buff, 0, head.HeadBuffer, 0, buff.Length);
                if (head.HeadSize > buff.Length)
                {
                    stream.Read(head.HeadBuffer, buff.Length, head.HeadSize - buff.Length);
                }
                head.FileType = new byte[4] { buff[0], buff[1], buff[2], buff[3] };
                head.Version = (ushort)((((ushort)buff[4]) << 8) | buff[5]);
                head.SourceModifyTime = ValueFuns.BytesToInt32(buff,8,4);
                head.SourceLength = ValueFuns.BytesToInt32(buff, 12, 4);;
                return true;
            }
        }

        public static bool CreateHead(in string fromPath, in ushort headSize, out CCacheHead head)
        {
            Contract.Assert(!string.IsNullOrEmpty(fromPath) && headSize > 15);
            head = new CCacheHead();
            if (!File.Exists(fromPath))
            {
                return false;
            }
            using (Stream stream = File.Open(fromPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buff = new byte[headSize];
                long length = stream.Length;
                if (stream.Read(buff, 0, buff.Length) != buff.Length)
                {
                    return false;
                }
                head.FileType = new byte[4] { 0x54, 0x45, 0x58, 0x54 };//TEXT，先写死了，后续再考虑其他不同类型
                head.Version = 0;
                head.HeadSize = headSize;
                var modifyTime = File.GetLastWriteTime(fromPath);
                head.SourceModifyTime = (((Int32)modifyTime.Day) << 24) | (((Int32)modifyTime.Hour) << 16) | (((Int32)modifyTime.Minute) << 8) | modifyTime.Second;
                head.SourceLength = (Int32)length;
                head.HeadBuffer = new byte[headSize];//for c 要初始化好内存
                Buffer.BlockCopy(head.FileType, 0, head.HeadBuffer, 0, 4);
                head.HeadBuffer[4] = (byte)(head.Version >> 8);//Version
                head.HeadBuffer[5] = (byte)(head.Version & 0xFF);
                head.HeadBuffer[6] = (byte)(head.HeadSize >> 8);//HeadSize
                head.HeadBuffer[7] = (byte)(head.HeadSize & 0xFF);
                head.HeadBuffer[8] = (byte)modifyTime.Day;//SourceModifyTime
                head.HeadBuffer[9] = (byte)modifyTime.Hour;
                head.HeadBuffer[10] = (byte)modifyTime.Minute;
                head.HeadBuffer[11] = (byte)modifyTime.Second;
                head.HeadBuffer[12] = (byte)(length >> 24);
                head.HeadBuffer[13] = (byte)(length >> 16);
                head.HeadBuffer[14] = (byte)(length >> 8);
                head.HeadBuffer[15] = (byte)(length & 0xFF);
                return true;
            }
        }

        public static Int32 ReadFormatCache(in CCacheHead head, out CFormatCache cache)
        {
            throw new NotImplementedException();
        }

        public unsafe static bool WriteFormatCache(in string srcPath, out string dstPath)
        {
            if (!GetCachePath(srcPath,Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Cache"), out dstPath))
            {
                return false;
            }
            CCacheHead head = new CCacheHead();
            if (!CreateHead(srcPath, FormatCacheHeadSize, out head))
            {
                return false;
            }

            if (File.Exists(dstPath))
            {
                File.Delete(dstPath);
            }
            using (Stream dstStream = File.Open(dstPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                dstStream.Write(head.HeadBuffer, 0, head.HeadSize);
                Int32 Remaind = 0;//上一次解析剩余多少个字节
                Int32 buffSize = 100 << 10;//每次读一百K
                                           //for c 要处理内存申请与释放
                byte[] utf8Buff = new byte[buffSize];
                byte[] unicodeBuff = new byte[buffSize * 2];
                fixed (byte* unicodeBuffPointer = unicodeBuff)
                {
                    ushort* p = (ushort*)unicodeBuffPointer;
                    fixed (byte* utf8BuffPointer = utf8Buff)
                    {
                        using (Stream srcStream = File.Open(srcPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            while (true)
                            {
                                int srcLength = srcStream.Read(utf8Buff, Remaind, utf8Buff.Length - Remaind);
                                if (srcLength > 0)
                                {
                                    Int32 utf8Length = srcLength + Remaind;
                                    Int32 utf8ReadSize = 0;
                                    Int32 unicodeWriteSize = 0;
                                    DecodeUtf8(utf8BuffPointer, 0, utf8Length, out utf8ReadSize, ref p, buffSize, out unicodeWriteSize);

                                    //todo 剩下一两个字节，要放到头部，
                                    if (utf8ReadSize < utf8Length)
                                    {
                                        Remaind = utf8Length - utf8ReadSize;
                                        for (int i = 0; i < Remaind; i++)
                                        {
                                            utf8Buff[i] = utf8Buff[utf8ReadSize + i];
                                        }
                                    }
                                    dstStream.Write(unicodeBuff, 0, unicodeWriteSize << 1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static bool ModifyFormatCacheHead(in string srcPath,in CCacheHead head)
        {
            //todo 注意要重新读一次头信息，要确保Offset想匹配，否则要重新写缓存内容
            throw new NotImplementedException();
        }

        public static bool GetCachePath(in string srcPath, in string dstDir,out string dstPath) {
            var buff = Encoding.UTF8.GetBytes(srcPath);
            var fileName = Crc16.GetCRC(buff, 0);
            var index = srcPath.LastIndexOf("/");
            if (index < 0)
            {
                index = srcPath.LastIndexOf("\\");
            }
            Contract.Assert(index > 0);
            //最后保留2个字符，用于后续实现缓存冲突时的Bucket
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }
            dstPath = Path.Combine(dstDir, fileName.ToString("X4") + "00");
            return true;
        }
        /// <summary>
        /// 只支持无BOM 大头UTF8
        /// </summary>
        /// <param name="utf8"></param>
        /// <param name="utf8Start"></param>
        /// <param name="utf8End"></param>
        /// <param name="utf8ReadSize">读取了utf8多少个字节</param>
        /// <param name="unicode"></param>
        /// <param name="unicodeEnd"></param>
        /// <param name="unicodeWriteSize">解读出多少个Unicode码</param>
        /// <returns></returns>
        public static unsafe void DecodeUtf8(in byte* utf8, in Int32 utf8Start, in Int32 utf8End, out Int32 utf8ReadSize,
            ref ushort* unicode, in Int32 unicodeEnd, out Int32 unicodeWriteSize)
        {
            unicodeWriteSize = 0;
            utf8ReadSize = 0;
            int u = utf8Start;
            for (; u < utf8End && unicodeWriteSize < unicodeEnd; u++)
            {
                byte b = *(utf8 + u);
                if (b > 0xDF)//从E0开始，按3个字节来算
                {
                    if (u + 2 < utf8End)
                    {
                        Int32 value = 0;
                        value = (b & 0xF) << 12;
                        value |= ((*(utf8 + u + 1)) & 0x3F) << 6;
                        value |= (*(utf8 + u + 2)) & 0x3F;

                        unicode[unicodeWriteSize] = (ushort)value;
                        u += 2;
                        unicodeWriteSize++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    unicode[unicodeWriteSize] = b;
                    unicodeWriteSize++;
                }
            }
            utf8ReadSize = u;
        }
    }
}
