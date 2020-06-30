using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCommonLib;
using CGraphic;
using CUI;
using EInkLib;

namespace BookReader
{
    public partial class SingleFrameForm : Form
    {
        public SingleFrameForm()
        {
            this.InitializeComponent();
            Picture.Image = new Bitmap(CUIEnvironment.WidthOfPixel, CUIEnvironment.HeightOfPixel,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Picture.Click += (o, e) => Draw();
            Draw();

            FormClosed += Form_FormClosed;
        }

        const int COLS = 4;
        const int ROWS = 3;

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            CUIFrame frame;
            CUIApp.GetActivedFrame(out frame);
            CUIView view;
            if (CUIFrameMethods.GetFocusedView(frame, out view))
            {
                int index = CUIFrameMethods.GetChildViewIndex(frame, view);
                int col = index % COLS;
                int row = index / COLS;
                bool handle = true;
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        {
                            if (row > 0)
                            {
                                row--;
                            }
                            else {
                                row = ROWS - 1;
                            }
                            break;
                        }
                    case Keys.Down:
                        {
                            if (row < ROWS - 1)
                            {
                                row++;
                            }
                            else {
                                row = 0;
                            }
                            break;
                        }
                    case Keys.Left:
                        {
                            if (col > 0)
                            {
                                col--;
                            }
                            else {
                                col = COLS - 1;
                            }
                            break;
                        }
                    case Keys.Right:
                        {
                            if (col < COLS - 1)
                            {
                                col++;
                            }
                            else
                            {
                                col = 0;
                            }
                            break;
                        }
                    default:
                        handle = false;
                        break;
                }
                if (handle)
                {
                    CUIView next;
                    CUIFrameMethods.GetChildView(frame, row * COLS + col, out next);
                    CUIFrameMethods.SetFocus(ref frame, next);
                    CUIFrameMethods.Refresh(ref frame);

                    RenderToPictureBox();
                }
                e.Handled = handle;
            }
            var time = stopwatch.ElapsedMilliseconds;

        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            CUIFrame frame;
            if (CUIApp.GetActivedFrame(out frame))
            {
                CUIApp.Remove(frame);
                //todo 回收Frame的额外内存
            }
        }

        private unsafe void Draw()
        {
            CUIFrame frame;
            if (CUIApp.GetFrameCount() == 0)
            {
                CUIFrameMethods.Create(out frame, CUIFrameType.Main);

                CPadding padding;
                CCommonLib.CCommonInits.InitPaddingAll(out padding, 12);
                for (int row = 0; row < ROWS; row++)
                {
                    for (int col = 0; col < COLS; col++)
                    {
                        var text = string.Format("({0},{1})", col, row);
                        var btn = CUIViewMethods.Create(CUIViewType.Button);
                        //btn.Text = Utils.StringToUshortArray(text);
                        btn.Text = Utils.ToCharPointer(text);
                        btn.TextSize = text.Length;
                        btn.Size = new CSize() { Width = 190, Height = 50 };
                        btn.Size.Height = 50;
                        btn.Style.FontColor = 0xFF;
                        btn.Style.BackColor = 0x30;
                        btn.Style.BorderColor = 0xC0;
                        btn.Style.BorderThickness = 1;

                        btn.ActiveStyle.FontColor = 0xFF;
                        btn.ActiveStyle.BackColor = 0x10;
                        btn.ActiveStyle.BorderColor = 0x33;
                        btn.ActiveStyle.BorderThickness = 1;
                        btn.Location = new CPoint() { X = padding.Left + (padding.Left + btn.Size.Width) * col,
                            Y = padding.Top + (padding.Top + btn.Size.Height) * row };
                        CUIFrameMethods.AddToFrame(ref frame, btn);
                    }
                }
                CUIView button;
                CUIFrameMethods.GetChildView(frame, 0, out button);
                CUIFrameMethods.SetFocus(ref frame, button);

                CUIApp.Add(in frame);
                CUIFrameMethods.Refresh(ref frame);
            }
            else {
                CUIApp.GetActivedFrame(out frame);
                CUIFrameMethods.Refresh(ref frame);
            }
            RenderToPictureBox();

        }

        private void RenderToPictureBox()
        {
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
            var drawToBitmapTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            Picture.Invalidate();
            Picture.Update();
            var updateTime = stopwatch.ElapsedMilliseconds;
        }
    }
}
