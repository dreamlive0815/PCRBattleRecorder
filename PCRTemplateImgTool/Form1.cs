﻿using System;
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

namespace PCRTemplateImgTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

        void RefreshTitle()
        {

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

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"{e.X},{e.Y} {GetPointRate().Format()} {GetRectRate().Format()}";
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
                var s = rectRate.Format();
                Clipboard.SetText(s);
            }
            else
            {
                var pointRate = GetPointRate();
                var s = pointRate.Format();
                Clipboard.SetText(s);
            }
            Text = $"{GetPointRate().Format()} {GetRectRate().Format()}";
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

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshImageByViewportCapture();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                var pwid = pictureBox1.Width;
                var phei = pictureBox1.Height;
                var mat = new Bitmap(pictureBox1.Image).ToOpenCvMat();
                var isPartial = !e.Alt;
                var rect = isPartial ? rectangle : new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                if (rect.Width == 0 || rect.Height == 0)
                    return;
                var rectRate = GetRectRate();
                var childMat = mat.GetChildMatByRectRate(rectRate);
                var saveDialog = new SaveFileDialog();
                saveDialog.Title = "选择图片保存路径(" + (isPartial ? "部分" : "完整");
                saveDialog.Filter = "*.png|*.png";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    childMat.SaveImage(saveDialog.FileName);
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                RefreshImageByViewportCapture();
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

        public static string Format(this Vec4f rectRate)
        {
            var s = string.Format("new Vec4f({0}f, {1}f, {2}f, {3}f)",
                rectRate.Item0.Format(), rectRate.Item1.Format(),
                rectRate.Item2.Format(), rectRate.Item3.Format());
            return s;
        }
    }
}
