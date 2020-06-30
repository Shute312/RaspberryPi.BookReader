using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class GraphicForm : Form
    {
        public GraphicForm()
        {
            InitializeComponent();
            Picture.Image = new Bitmap(this.Width, this.Height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Draw();

            FormClosed += Form_FormClosed;
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

        private void OnClickRefresh(object sender, EventArgs e)
        {
            Draw();
        }

        unsafe void Draw()
        {
            CBitmap bitmap;
            CGraphic.CGraphicInits.InitBitmap(out bitmap, CPixelFormat.Format8bppIndexed, CUIEnvironment.WidthOfPixel, CUIEnvironment.HeightOfPixel);

            var rectangle = new CRect()
            {
                MinX = 10,
                MinY = 10,
                MaxX = 480,
                MaxY = 310
            };
            var modifyRect = rectangle;
            CG.DrawRectangle2(ref bitmap,2, rectangle.MinX, rectangle.MinY, rectangle.MaxX, rectangle.MaxY, 0xF);


            CFontInfo fontInfo;
            CG.GetValidFontSize(CFont.DefaultSize,out fontInfo);
            var text = @"电子墨水屏采用“微胶囊电泳显示”技术进行图像显示，其工作原理是悬浮在液体中的带电纳米粒子受到电场作用而产生迁移， 在环境光下通过反射形成接近传统印刷纸张的显示效果。电子墨水屏能够在灯光，自然光等环境光下清晰地显示画面，无需背光，可视角度几乎达到180°。因其媲美传统纸张的显示效果， 常被用于阅读器之类的应用。";
            text = String.Intern(text);//放到常量
            CTextInfo textInfo = new CTextInfo() { Start = 0, End = text.Length, Length = text.Length };
            textInfo.Text = Utils.ToCharPointer(text);
            CRect renderRect;
            CG.DrawText(ref bitmap, textInfo, fontInfo, rectangle, new CPoint() { X = fontInfo.Width * 2, Y = 8 }, out renderRect);

            var image = (Bitmap)Picture.Image;
            EInkRender.DrawToBitmap(ref image, bitmap, modifyRect);
        }
    }
}
