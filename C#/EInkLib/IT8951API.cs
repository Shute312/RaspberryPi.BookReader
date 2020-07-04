using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EInkLib
{
	public static class IT8951API
	{
		internal const string DLL_PATH = "IT8951Lib.dll";


		[DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte Open();


		[DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte GetDeviceInfo(UInt32[] info);


		//发送画面到设备内存
		[DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte SendImage(IntPtr image, Int32 address, Int32 x, Int32 y, Int32 w, Int32 h);


		//将设备内容中的画面渲染到屏幕上
		[DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte RenderImage(Int32 address, Int32 x, Int32 y, Int32 w, Int32 h);


		//发送并显示画面
		[DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte ShowImage(IntPtr image, Int32 address, Int32 x, Int32 y, Int32 w, Int32 h);


		[DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte Erase(Int32 address, Int32 size);

			
		[DllImport(DLL_PATH, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte Close();


	}
	public struct DeviceInfo
	{
		public DeviceInfo(UInt32[] info)
		{
			Contract.Assert(info != null && info.Length >= 28);
			StandardCmdNo = info[0];
			ExtendCmdNo = info[1];
			Signature = info[2];
			Version = info[3];
			Width = info[4];
			Height = info[5];
			UpdateBufBase = info[6];
			ImageBufBase = info[7];
			TemperatureNo = info[8];
			ModeNo = info[9];
			FrameCount = new UInt32[8];
			Buffer.BlockCopy(info, 10, FrameCount, 0, FrameCount.Length);
			NumImgBuf = info[18];
			WbfSFIAddr = info[19];
			Reserved = new UInt32[8];
			Buffer.BlockCopy(info, 19, Reserved, 0, Reserved.Length);
		}

		public UInt32 StandardCmdNo; // Standard command number2T-con Communication Protocol
		public UInt32 ExtendCmdNo; // Extend command number
		public UInt32 Signature; // 31 35 39 38h (8951)
		public UInt32 Version; // command table version
		public UInt32 Width; // Panel Width
		public UInt32 Height; // Panel Height
		public UInt32 UpdateBufBase; // Update Buffer Address
		public UInt32 ImageBufBase; // Image Buffer Address(default image buffer)
		public UInt32 TemperatureNo; // Temperature segment number
		public UInt32 ModeNo; // Display mode number
		public UInt32[] FrameCount; // Frame count for each mode(8).
		public UInt32 NumImgBuf;//Numbers of Image buffer
		public UInt32 WbfSFIAddr;//这里跟文档有些变化源文档是保留9个字节，这里是1+8
		public UInt32[] Reserved;
	}
}
