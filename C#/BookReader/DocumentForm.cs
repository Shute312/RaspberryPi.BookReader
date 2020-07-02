﻿using CCommonLib;
using CGraphic;
using CReader;
using CUI;
using EInkLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BookReader
{
    public partial class DocumentForm : Form
    {
        const Int32 BUFF_SIZE = 100 << 10;
        const Int32 PRE_SIZE = BUFF_SIZE / 10;//往前额外读多少
        
        public unsafe DocumentForm()
        {
            InitializeComponent();

            this.Load += DocumentForm_Load;
            this.FormClosed += DocumentForm_FormClosed;
        }

        private void DocumentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CUIFrame frame;
            if (CUIApp.GetActivedFrame(out frame)) {
                CUIApp.Remove(frame);
                //todo 回收Frame的额外内存
            }
        }

        private unsafe void DocumentForm_Load(object sender, EventArgs e)
        {

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.txt");
            Contract.Ensures(File.Exists(path));
            //应该做一次文档格式转换，比如，转为Unicode编码，文档的前128个字节固定空出来；

            //todo 加载时，应该往前一段距离加载，将起点落在10-90%之间
            CDocumentInfo documentInfo = new CDocumentInfo();
            //using (Stream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    Int32 buffSize = BUFF_SIZE;
            //    Int32 unicodeSize = BUFF_SIZE >> 1;
            //    documentInfo.FileSize = (Int32)stream.Length;
            //    documentInfo.Buffer = (ushort*)Marshal.AllocHGlobal(buffSize);
            //    documentInfo.BufferSize = unicodeSize;
            //    documentInfo.Path = path;
            //    documentInfo.StreamPositon = documentInfo.StreamPositon + buffSize;
            //    documentInfo.IsEnd = documentInfo.StreamPositon < documentInfo.FileSize;
            //}
            LoadDocument(ref documentInfo, path, 0);

            //采用8bits格式
            var image = new Bitmap(CUIEnvironment.WidthOfPixel, CUIEnvironment.HeightOfPixel, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            //配置调色板
            ColorPalette palette = image.Palette;//这里取得的似乎是复制体
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            image.Palette = palette;
            //for (int i = 0; i < byte.MaxValue; i++)
            //{
            //    var c = image.Palette.Entries[i];
            //    image.Palette.Entries[i] = Color.FromArgb(i, i, i);
            //}
            //刷成白色背景
            var bitmapData = image.LockBits(new Rectangle(0,0,image.Width,image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            var temp = Enumerable.Repeat<byte>(byte.MaxValue, image.Width*image.Height).ToArray();
            Marshal.Copy(temp, 0, bitmapData.Scan0, temp.Length);
            image.UnlockBits(bitmapData);

            Picture.Image = image;
            CUIFrameMethods.Create(out frame, CUIFrameType.Main);

            CPadding padding;
            CCommonLib.CCommonInits.InitPaddingAll(out padding, 12);
            var view = CUIViewMethods.Create(CUIViewType.Document);
            //view.FontSize = 12;
            CFontInfo fontInfo;
            CG.GetValidFontSize(view.Style.FontSize, out fontInfo);
            view.Size = new CSize() { Width = Picture.Width - padding.Left - padding.Right, Height = Picture.Height - padding.Top - padding.Bottom };
            view.Style.FontSize = fontInfo.FontSize;
            view.Style.FontColor = 0x00;
            view.Style.BackColor = 0xFF;
            view.Style.BorderColor = 0xFF000030;
            view.Style.BorderThickness = 0;
            //view.Text = (char*)documentInfo.Buffer;
            //view.TextSize = documentInfo.BufferSize;
            view.Location = new CPoint()
            {
                X = padding.Left,
                Y = padding.Top
            };
            documentView = (CUIDocumentView)view.Properties;
            documentView.Info = documentInfo;
            documentView.Style = new CUIDocumentStyle()
            {
                MarginLeft = 0,
                MarginRight = 0,
                MarginTop = 0,
                MarginBottom = 0,
                SectionFirstLineMarginLeft = fontInfo.Width * 2,
                //LineMarginTop = fontInfo.Height >> 1,
                LineMarginTop = 2,
                LineMarginBottom = 0,
                RemainLines = 1,
            };
            CUIFrameMethods.AddToFrame(ref frame, view);

            CUIApp.Add(in frame);

            Draw();
        }

        CUIFrame frame;
        CUIDocumentView documentView;

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool handle = true;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    {
                        ReloadFile(ref documentView);
                        handle = CUIDocumentViewMethods.PageDown(ref documentView);
                        break;
                    }
                case Keys.Up:
                    {
                        ReloadFile(ref documentView);
                        handle = CUIDocumentViewMethods.PageUp(ref documentView);
                        break;
                    }
                default:
                    handle = false;
                    break;
            }
            if (handle)
            {
                CUIFrameMethods.Refresh();
                RenderToPictureBox();
            }

            e.Handled = handle;

            var time = stopwatch.ElapsedMilliseconds;
        }

        private unsafe void Draw()
        {
            CUIFrameMethods.Refresh();
            RenderToPictureBox();

        }

        private unsafe void RenderToPictureBox()
        {
            //可以尝试直接将render内容指向 picturebox(这里要做好进制转换)、考虑将Bitmap改为32色，一次性写大量数据，加快速度(先看看是否有必要)
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            CBitmap context;
            CGraphicInits.InitBitmap(out context, CPixelFormat.Format8bppIndexed, CUIEnvironment.WidthOfPixel, CUIEnvironment.HeightOfPixel);
            CUIApp.Render(ref context, true);//渲染出来
            var renderTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var image = (Bitmap)Picture.Image;
            var modifyRect = new CRect()
            {
                MinX = 0,
                MinY = 0,
                MaxX = Picture.Width,
                MaxY = Picture.Height
            };
            //画到屏幕上
            EInkRender.DrawToBitmap(ref image, context, modifyRect);

            if (context.Buffer != null)
            {
                Marshal.FreeHGlobal((IntPtr)context.Buffer);
            }
            var drawToBitmapTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            Picture.Invalidate();
            Picture.Update();
            var updateTime = stopwatch.ElapsedMilliseconds;
        }

        private bool ReloadFile(ref CUIDocumentView view)
        {
            var info = view.Info;
            if (info.StreamPositon > info.DocumentOffset)
            {
                if (view.UnicodeStart << 1 < PRE_SIZE)
                {
                    //要向前加载
                    throw new NotImplementedException();
                }
            }
            else
            {
                if (view.UnicodeEnd > view.Info.UnicodeSize - (PRE_SIZE >> 1))
                {
                    //要向后加载
                    throw new NotImplementedException();
                }
            }

            return false;//不需要重新加载
        }

        private unsafe bool LoadDocument(ref CDocumentInfo info, in string path, Int32 startPosition)
        {
            string cachePath;
            bool isCacheName = CReader.CCacheReader.GetCachePath(path, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache"), out cachePath);
            Contract.Assert(isCacheName);
            if (!File.Exists(cachePath))
            {
                CReader.CCacheReader.WriteFormatCache(path, out cachePath);
            }

            CCacheHead head;
            CReader.CCacheReader.ReadHead(path, cachePath, out head);
            Contract.Assert(head.HeadSize > 0);
            info.DocumentOffset = head.HeadSize;
            if (startPosition > 0)
            {
                Contract.Ensures(head.HeadSize <= startPosition);
                info.StreamPositon = startPosition;
            }
            else
            {
                info.StreamPositon = head.HeadSize;
            }

            using (Stream stream = File.Open(cachePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Int32 buffSize = BUFF_SIZE;
                var buff = new byte[buffSize];
                stream.Position = info.StreamPositon;
                int length = stream.Read(buff, 0, buff.Length);
                if ((length & 1) == 1)
                {
                    length--;
                }

                info.DocSize = (Int32)stream.Length;
                if (info.Unicodes != null)
                {
                    Marshal.FreeHGlobal((IntPtr)info.Unicodes);
                }
                info.Unicodes = (ushort*)Marshal.AllocHGlobal(buffSize);
                info.UnicodeSize = length>>1;
                info.Path = path;
                info.IsEnd = info.StreamPositon + buffSize >= info.DocSize;
                stream.Position = info.StreamPositon;
                byte* p = (byte*)info.Unicodes;
                for (int i = 0; i < length; i++)
                {
                    *(p + i) = buff[i];
                }
//#if DEBUG
//                var text = Encoding.UTF8.GetString(buff);
//#endif
            }
            return true;
        }

        private void SaveProcgress()
        {
            //if (documentView != null &&  !string.IsNullOrEmpty(documentView.Info.Path))
            //{
            //    string cachePath;
            //    bool isCacheName = CReader.CCacheReader.GetCachePath(documentView.Info.Path, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache"), out cachePath);
            //    if (!File.Exists(cachePath))
            //    {
            //        CCacheHead head;
            //        if (CReader.CCacheReader.ReadHead(documentView.Info.Path,cachePath, out head)) {
            //        }
            //        Contract.Assert(head.HeadSize > 0);

            //        using (var stream = File.Open(cachePath, FileMode.Open, FileAccess.Write, FileShare.None))
            //        {
            //            head.HeadBuffer[];
            //            CReader.CCacheReader.WriteHead(cachePath,head);
            //        }
            //    }

                

            //}
        }
    }
}
