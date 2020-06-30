using CCommonLib;
using CGraphic;
using CUI;
using EInkLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BookReader
{

    public partial class FontForm : Form
    {
        public unsafe FontForm()
        {
            InitializeComponent();

            var bitmap = new Bitmap(Picture.Width, Picture.Height);
            CmbBpp.SelectedIndex = 2;
            Picture.Image = bitmap;

            var view = new View();
        }
        private string MakePath()
        {

            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts");
            //dir = Path.Combine(dir, ((int)NudFontSize.Value).ToString());
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
            //var path = Path.Combine(dir, GetBpp().ToString() + ".font");
            //return path;
        }

        private string MakeDir()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        private void BtnGeneration_Click(object sender, EventArgs e)
        {
            char[] text = EInkLib.Charset.Content;
            //char[] text = new char[] { '电'};
            var path = MakeDir();
            int fontSize = (int)NudFontSize.Value;
            Font font = new Font("微软雅黑", fontSize, GraphicsUnit.Pixel);
            CFont.WriteFont(text, text.Length, fontSize, path, font, GetBpp());
        }

        private unsafe void BtnRender_Click(object sender, EventArgs e)
        {
            int fontSize = (int)NudFontSize.Value;
            CFont.Init(MakePath());
            Bitmap image = new Bitmap(Picture.Width, Picture.Height,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Picture.Image = image;
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            g.Dispose();
            var text = string.Intern(Txt.Text);//弄成常量
            //var text = Charset.Content;
            CFontInfo fontInfo;
            CG.GetValidFontSize((int)NudFontSize.Value,out fontInfo);
            CBitmap bitmap;
            CGraphicInits.InitBitmap(out bitmap, CPixelFormat.Format8bppIndexed, image.Width, image.Height);
            CTextInfo textInfo = new CTextInfo()
            {
                Start = 0,
                End = text.Length,
                Length = text.Length
            };
            textInfo.Text = Utils.ToCharPointer(text);
            var modifyRect = new CRect();
            var rect = new CRect() { MinX = 0, MinY = 0, MaxX = 400, MaxY = 300 };
            CG.DrawText(ref bitmap, textInfo, fontInfo, rect, new CPoint() { X = 0, Y = 0 }, out modifyRect);
            EInkRender.DrawToBitmap(ref image, bitmap, rect);

            ////注意：设置负数的缩进，会有文本绘制不全的问题
            //FontMargin margin = new FontMargin();
            //margin.PageMarginLeft = 12;
            //margin.PageMarginRight = margin.PageMarginLeft;
            //margin.PageMarginTop = margin.PageMarginLeft;
            //margin.PageMarginRight = margin.PageMarginLeft;
            //margin.LineMarginTop = 4;
            //margin.LineMarginBottom = 2;
            //margin.MarginLeft = -2;
            //margin.MarginRight = -1;
            //margin.MarginTop = 0;
            //margin.MarginBottom = 0;
            //margin.ParagraphFirstLineMarginLeft = (short)((info.Width + margin.MarginLeft + margin.MarginRight) * 2);//空2个字符

            //int startX = margin.PageMarginLeft + margin.MarginLeft;
            //int x = startX + margin.ParagraphFirstLineMarginLeft;
            //int y = margin.PageMarginTop + margin.MarginTop;
            //FontBitmap matrix;
            //int maxX = bitmap.Width - margin.PageMarginRight - margin.MarginRight - 1;
            //int maxY = bitmap.Height - margin.PageMarginBottom - margin.LineMarginBottom - margin.MarginBottom - 1;
            //int lineHeight = info.Height + margin.LineMarginTop + margin.LineMarginBottom + margin.MarginTop + margin.MarginBottom;
            ////注意
            //for (int i = 0; i < text.Length; i++)
            //{
            //    var character = text[i];
            //    //强制换行
            //    if (character == '\n' || (character == '\r' && i < text.Length - 1) && character == '\n')
            //    {
            //        x = startX + margin.ParagraphFirstLineMarginLeft;
            //        y += lineHeight;
            //        //跳过\n
            //        if (character == '\r')
            //        {
            //            i++;
            //        }
            //        continue;
            //    }

            //    if (Fonts.ePageGetFontMatrix(character, fontSize, out matrix))
            //    {
            //        int width = info.Width;
            //        //新起一行
            //        if (x + width > maxX)
            //        {
            //            x = startX;
            //            y += lineHeight;
            //        }
            //        if (y > maxY)
            //        {
            //            break;
            //        }
            //        Fonts.ePageDrawFontMatrix(bitmap, x, y, matrix, info);

            //        x += width + margin.MarginLeft + margin.MarginRight;
            //    }
            //}
        }

        int GetBpp() {
            int bpp;
            int.TryParse(CmbBpp.SelectedItem.ToString(), out bpp);
            return bpp;
        }
    }
}
