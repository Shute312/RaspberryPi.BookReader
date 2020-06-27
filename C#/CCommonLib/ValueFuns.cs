using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace CCommonLib
{
    public static class ValueFuns
    {
        //这个可以写成define
        public static void SetToMinInt32(ref Int32 target, in Int32 value)
        {
            if (target > value)
            {
                target = value;
            }
        }
        //这个可以写成define
        public static void SetToMaxInt32(ref Int32 target, in Int32 value)
        {
            if (target < value)
            {
                target = value;
            }
        }
        //这个可以写成define
        public static void SetToMaxUInt32(ref UInt32 target, in UInt32 value)
        {
            if (target < value)
            {
                target = value;
            }
        }
        //这个可以写成define
        public static void SetToMinUInt32(ref UInt32 target, in UInt32 value)
        {
            if (target > value)
            {
                target = value;
            }
        }

        public static void MaxRect(in CRect v1, in CRect v2, ref CRect value)
        {
            value.MinX = v1.MinX < v2.MinX ? v1.MinX : v2.MinX;
            value.MinY = v1.MinY < v2.MinY ? v1.MinY : v2.MinY;
            value.MaxX = v1.MaxX > v2.MaxX ? v1.MaxX : v2.MaxX;
            value.MaxY = v1.MaxY > v2.MaxY ? v1.MaxY : v2.MaxY;
        }
        /// <summary>
        /// 并集
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="value"></param>
        /// <returns>是否存在并集</returns>
        public static bool MinRect(in CRect v1, in CRect v2, ref CRect value)
        {
            Int32 minX, minY;
            if (v1.MinX < v2.MinX)
            {
                if (v1.MaxX <= v2.MinX)
                {
                    return false;
                }
                minX = v2.MinX;
            }
            else
            {
                if (v1.MinX >= v2.MaxX)
                {
                    return false;
                }
                minX = v1.MinX;
            }
            if (v1.MinY < v2.MinY)
            {
                if (v1.MaxY <= v2.MinY)
                {
                    return false;
                }
                minY = v2.MinY;
            }
            else
            {
                if (v1.MinY >= v2.MaxY)
                {
                    return false;
                }
                minY = v1.MinY;
            }
            //这种写法是为了防止在return false的情况，额外改动了value的MinX跟MinY
            value.MinX = minX;
            value.MinY = minY;
            value.MaxX = Math.Min(v1.MaxX, v2.MaxX);
            value.MaxY = Math.Min(v1.MaxY, v2.MaxY);
            return true;
        }

        public static bool IsValidRect(in CRect rect)
        {
            return rect.MaxX > rect.MinX && rect.MaxY > rect.MinY;
        }

        public static Int32 BytesToInt32(in byte[] bytes,in Int32 start,in Int32 length)
        {
            Contract.Assert(length>0);
            Int32 value = 0;
            for (int i = 0; i < length; i++)
            {
                value <<= 8;
                value |= bytes[start + i];
            }
            return value;
        }
    }
}
