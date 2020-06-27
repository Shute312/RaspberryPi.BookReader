using CGraphic;
using CUI;
using EInkLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BookReader
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            const byte BPP = 4;
            //const Int32 MAX_WIDTH = 1872/2;//电脑屏幕分辨率没那么高，折一半
            //const Int32 MAX_HEIGHT = 1404/2;//电脑屏幕分辨率没那么高，折一半
            const Int32 MAX_WIDTH = 1872 * 2 / 3;//电脑屏幕分辨率没那么高，折一半
            const Int32 MAX_HEIGHT = 1404 * 2 / 3;//电脑屏幕分辨率没那么高，折一半

            CG.Init(EInkRender.GetValidFontSize,EInkRender.DrawText, EInkRender.MeasureText);
            CFont.Init(MakePath());
            CUIApp.Init(BPP, MAX_WIDTH, MAX_HEIGHT, 6.227f, 4.67f);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //Application.Run(new FontForm());
        }

        private static string MakePath()
        {

            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts");
            //dir = Path.Combine(dir, (Fonts.DefaultSize).ToString());
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
            //var path = Path.Combine(dir, 4.ToString() + ".font");
            //return path;
        }
    }
}
