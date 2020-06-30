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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //OnClickWorking(this, EventArgs.Empty);
            //OnClickGraphic(this, EventArgs.Empty);
        }

        private void ShowChild<T>(bool isMax = true, Orientation orientation = Orientation.Center) where T : new()
        {
            foreach (var item in this.MdiChildren)
            {
                if (item.GetType().Equals(typeof(T)))
                {
                    item.Focus();
                    return;
                }
            }
            var obj = new T();
            var form = obj as Form;

            if (isMax)
            {
                form.WindowState = FormWindowState.Maximized;
            }
            form.MdiParent = this;
            form.AllowDrop = true;
            if (orientation != Orientation.Auto)
            {
                Rectangle rect = Screen.GetWorkingArea(this);
                form.StartPosition = FormStartPosition.Manual;
                var hor = ((int)orientation) & 0x3;
                var ver = ((int)orientation) >> 2;
                int top;
                int left;
                switch (hor)
                {
                    case 1:
                        left = 0;
                        break;
                    case 2://center
                        left = (rect.Width - form.Width) >> 1;
                        break;
                    case 3:
                        left = rect.Width - form.Width - 5;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                switch (ver)
                {
                    case 1:
                        top = 0;
                        break;
                    case 2://center
                        top = (rect.Height - form.Height) >> 1;
                        break;
                    case 3:
                        top = rect.Height - form.Height - 5;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                form.Location = new System.Drawing.Point(left, top);

            }
            form.Show();
        }


        private void OnClickFont(object sender, EventArgs e)
        {
            ShowChild<FontForm>(true, Orientation.TopLeft);
        }

        private void OnClickGraphic(object sender, EventArgs e)
        {
            ShowChild<GraphicForm>(true, Orientation.TopLeft);
        }

        private void OnClickSingleFrame(object sender, EventArgs e)
        {
            ShowChild<SingleFrameForm>(true, Orientation.TopLeft);
        }

        private void OnClickDocument(object sender, EventArgs e)
        {
            ShowChild<DocumentForm>(true, Orientation.TopLeft);
        }
    }
    enum Orientation
    {
        /*
         左:01
         中:10
         右:11
         上:01
         下:11
             */
        Auto = 0,
        TopLeft = 5,//二进制0101
        Top = 6,//二进制0110
        TopRight = 7,//二进制0111
        Left = 9,//二进制1001
        Center = 10,//二进制1010
        Right = 11,//二进制1011
        BottomLeft = 13,//二进制1101
        Bottom = 14,//二进制1110
        BottomRight = 15//二进制1111
    }
}
