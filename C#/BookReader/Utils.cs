using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BookReader
{
    public unsafe static class Utils
    {
        private static Dictionary<string, IntPtr> dic = new Dictionary<string, IntPtr>();

        public static  ushort[] StringToUshortArray(string text)
        {
            if (text == null) return null;
            var array = new ushort[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                array[i] = text[i];
            }
            return array;
        }

        public static char* ToCharPointer(string text)
        {
            IntPtr p = IntPtr.Zero;
            if (dic.ContainsKey(text))
            {
                p = dic[text];
            }
            else
            {
                //    byte[] bytes = Encoding.Unicode.GetBytes(text.ToArray());
                //    byte* buff;
                //    //申请字节内存
                //    buff = (byte*)Marshal.AllocHGlobal(bytes.Length);
                //    for (int i = 0; i < bytes.Length; i++)
                //    {
                //        *(buff + i) = bytes[i];
                //    }
                //    dic.Add(text, (IntPtr)buff);
                //    p = (IntPtr)buff;
                IntPtr ptr = Marshal.StringToHGlobalUni(text);
                dic.Add(text, ptr);
                p = ptr;
                //Marshal.FreeHGlobal((IntPtr)buff);/*释放申请的内存*/

            }
            return (char*)p;
        }
    }
}
