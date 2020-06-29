using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using RawSize = System.Drawing.Size;
using PCRBattleRecorder;
using System.IO;

namespace PCRTemplateImgTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //RefreshImageByViewportCapture();
        }

        public void RefreshImageByViewportCapture()
        {
            var capture = MumuTools.GetInstance().DoCaptureViewport();
            LoadImage(capture);
        }

        public void LoadImage(Image img)
        {
            pictureBox1.Image = img;
            Size = pictureBox1.Size + new RawSize(0, 50);
            press = false;
            rectangle = new Rectangle();
        }

        bool press = false;
        int startX, startY;
        Rectangle rectangle;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            rectangle = new Rectangle();
            press = true;
            startX = e.X;
            startY = e.Y;
        }

        void RefreshTitle(int x, int y)
        {
            if (pictureBox1.Image == null)
                return;
            var head = $"Image:{pictureBox1.Image.Width},{pictureBox1.Image.Height} PicBox:{pictureBox1.Width},{pictureBox1.Height} ";
            Text = $"{head}{x},{y} {GetPointRate().Format()} {GetRectRate().Format()}";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshTitle(e.X, e.Y);
            if (!press) return;
            pictureBox1.Refresh();
            var g = pictureBox1.CreateGraphics();
            var pen = new Pen(Color.Red, 2);
            var x1 = Math.Min(startX, e.X);
            var y1 = Math.Min(startY, e.Y);
            var x2 = Math.Max(startX, e.X);
            var y2 = Math.Max(startY, e.Y);
            rectangle = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            g.DrawRectangle(pen, rectangle);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            press = false;
            var width = pictureBox1.Width;
            var height = pictureBox1.Height;
            var rect = rectangle;
            if (rect.Width > 5 && rect.Height > 5)
            {
                var rectRate = GetRectRate();
                var s = $"\"{fileName}\": {GetRectRate().FormatAsJsonArray()},";
                Clipboard.SetText(s);
            }
            else
            {
                if (rectangle.X == 0 || rectangle.Y == 0)
                {
                    rectangle = new Rectangle(e.X, e.Y, 0, 0);
                }
                var pointRate = GetPointRate();
                var s = pointRate.FormatAsJsonArray();
                Clipboard.SetText(s);
            }
            RefreshTitle(e.X, e.Y);
        }

        string FormatFloat(double f)
        {
            return f.ToString("f4");
        }

        Vec2f GetPointRate()
        {
            var width = pictureBox1.Width;
            var height = pictureBox1.Height;
            var midrx = 1.0f * (rectangle.X + 0.5f * rectangle.Width) / width;
            var midry = 1.0f * (rectangle.Y + 0.5f * rectangle.Height) / height;
            return new Vec2f(midrx, midry);
        }

        Vec4f GetRectRate()
        {
            var width = pictureBox1.Width;
            var height = pictureBox1.Height;
            var rx1 = 1.0f * rectangle.X / width;
            var ry1 = 1.0f * rectangle.Y / height;
            var rx2 = 1.0f * (rectangle.X + rectangle.Width) / width;
            var ry2 = 1.0f * (rectangle.Y + rectangle.Height) / height;
            return new Vec4f(rx1, ry1, rx2, ry2);
        }

        void SaveImageByRectRate(Vec4f rectRate)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("请先捕获截图");
                return;
            }

            var mat = new Bitmap(pictureBox1.Image).ToOpenCvMat();
            var childMat = mat.GetChildMatByRectRate(rectRate);
            var isPartial = Math.Abs(rectRate.Item0 - rectRate.Item2) < 1
                || Math.Abs(rectRate.Item1 - rectRate.Item3) < 1;
            var saveDialog = new SaveFileDialog();
            saveDialog.Title = "选择图片保存路径(" + (isPartial ? "部分" : "完整");
            saveDialog.Filter = "*.png|*.png";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                childMat.SaveImage(saveDialog.FileName);
                var name = new FileInfo(saveDialog.FileName).Name;
                fileName = name;
                var s = $"\"{fileName}\": {GetRectRate().FormatAsJsonArray()},";
                Clipboard.SetText(s);
            }
        }

        string fileName = "";

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                var pwid = pictureBox1.Width;
                var phei = pictureBox1.Height;
                var isPartial = !e.Alt;
                var rect = isPartial ? rectangle : new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                if (rect.Width == 0 || rect.Height == 0)
                    return;
                var rectRate = GetRectRate();
                SaveImageByRectRate(rectRate);
            }
            else if (e.KeyCode == Keys.F5)
            {
                RefreshImageByViewportCapture();
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                var dialog = new InputDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var rectRate = dialog.GetVec4f();
                    if (Math.Abs(rectRate.Item2 - rectRate.Item0) < 0.001f || Math.Abs(rectRate.Item3 - rectRate.Item1) < 0.001f)
                        return;
                    SaveImageByRectRate(rectRate);
                }
            }
        }
    }

    static class FloatExtension
    {
        public static string Format(this float f)
        {
            return f.ToString("f4");
        }
    }

    static class VecFormatExtension
    {
        public static string Format(this Vec2f pointRate)
        {
            var s = string.Format("new Vec2f({0}f, {1}f)", pointRate.Item0.Format(), pointRate.Item1.Format());
            return s;
        }

        public static string FormatAsJsonArray(this Vec2f pointRate)
        {
            var s = string.Format("[{0}, {1}]",
                pointRate.Item0.Format(), pointRate.Item1.Format());
            return s;
        }

        public static string Format(this Vec4f rectRate)
        {
            var s = string.Format("new Vec4f({0}f, {1}f, {2}f, {3}f)",
                rectRate.Item0.Format(), rectRate.Item1.Format(),
                rectRate.Item2.Format(), rectRate.Item3.Format());
            return s;
        }

        public static string FormatAsJsonArray(this Vec4f rectRate)
        {
            var s = string.Format("[{0}, {1}, {2}, {3}]",
                rectRate.Item0.Format(), rectRate.Item1.Format(),
                rectRate.Item2.Format(), rectRate.Item3.Format());
            return s;
        }
    }
}
