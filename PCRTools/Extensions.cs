using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RawPoint = System.Drawing.Point;
using EmulatorPoint = System.Drawing.Point;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PCRBattleRecorder.Config;
using System.Collections.Generic;

namespace PCRBattleRecorder
{
    public static class RichTextBoxExtension
    {

        public static void ScrollToEnd(this RichTextBox richTextBox)
        {
            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.ScrollToCaret();
        }

        public static void AppendLineThreadSafe(this RichTextBox richTextBox, string s)
        {
            AppendLineThreadSafe(richTextBox, s, Color.Black);
        }

        public static void AppendLineThreadSafe(this RichTextBox richTextBox, string s, Color color)
        {
            AppendTextThreadSafe(richTextBox, s + Environment.NewLine, color);
        }

        public static void AppendTextThreadSafe(this RichTextBox richTextBox, string s)
        {
            AppendTextThreadSafe(richTextBox, s, Color.Black);
        }

        public static void AppendTextThreadSafe(this RichTextBox richTextBox, string s, Color color)
        {
            if (richTextBox == null)
            {
                return;
            }
            if (richTextBox.InvokeRequired)
            {
                if (richTextBox.IsDisposed) return;
                richTextBox.Invoke(new Action<RichTextBox, string, Color>(AppendTextThreadSafe), richTextBox, s, color);
            }
            else
            {   
                if (richTextBox.IsDisposed) return;
                richTextBox.SelectionColor = color;
                //richTextBox.AppendText(s);
                //ScrollToEnd(richTextBox);

                int savedVpos = Win32Api.GetScrollPos(richTextBox.Handle, Win32Api.SB_VERT);
                richTextBox.AppendText(s);
                if (ConfigMgr.GetInstance().OutputAutoScroll)
                {
                    int VSmin, VSmax;
                    Win32Api.GetScrollRange(richTextBox.Handle, Win32Api.SB_VERT, out VSmin, out VSmax);
                    int sbOffset = (int)((richTextBox.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight) / (richTextBox.Font.Height));
                    savedVpos = VSmax - sbOffset;
                }
                Win32Api.SetScrollPos(richTextBox.Handle, Win32Api.SB_VERT, savedVpos, true);
                Win32Api.PostMessageA(richTextBox.Handle, Win32Api.WM_VSCROLL, Win32Api.SB_THUMBPOSITION + 0x10000 * savedVpos, 0);
            }
        }
    }

    public static class OpenCvExtension
    {
        public static Mat ToOpenCvMat(this Bitmap bitmap)
        {
            return bitmap.ToMat();
        }

        public static Bitmap ToRawBitmap(this Mat mat)
        {
            return mat.ToBitmap();
        }

        public static Color GetPixel(this Mat mat, int r, int c)
        {
            var channels = mat.Channels();
            Color clr;
            if (mat.Channels() == 1)
            {
                var v = mat.Get<byte>(r, c);
                clr = Color.FromArgb(v, v, v);
            }
            else
            {
                var vec3b = mat.Get<Vec3b>(r, c);
                clr = Color.FromArgb(vec3b.Item0, vec3b.Item1, vec3b.Item2);
            }
            return clr;
        }

        public static void SetPixel(this Mat mat, int r, int c, params byte[] rbga)
        {
            var channels = mat.Channels();
            var getV = new Func<int, byte>((int index) =>
            {
                return index < rbga.Length ? rbga[index] : (byte)0;
            });
            if (mat.Channels() == 1)
            {
                var v = mat.Get<byte>(r, c);
                mat.Set(r, c, getV(0));
            }
            else
            {
                var vec3b = new Vec3b(getV(0), getV(1), getV(2));
                mat.Set(r, c, vec3b);
            }
        }

        public static void SetPixel(this Mat mat, int r, int c, params int[] rbga)
        {
            var getV = new Func<int, byte>((int index) =>
            {
                return index < rbga.Length ? (byte)rbga[index] : (byte)0;
            });
            SetPixel(mat, r, c, getV(0), getV(1), getV(2));
        }

        public static RECT GetRect(this Mat mat)
        {
            return new RECT() { x1 = 0, y1 = 0, x2 = mat.Width, y2 = mat.Height };
        }

        public static Mat GetChildMatByRect(this Mat mat, RECT relativeRect)
        {
            var wid = mat.Width;
            var hei = mat.Height;
            var xRange = new Range(Math.Max(relativeRect.x1, 0), Math.Min(relativeRect.x2, wid));
            var yRange = new Range(Math.Max(relativeRect.y1, 0), Math.Min(relativeRect.y2, hei));
            var child = mat[yRange, xRange];
            return child;
        }

        public static Mat GetChildMatByRectRate(this Mat mat, Vec4f rectRate)
        {
            var rect = GetRect(mat);
            var relativeRect = rect.GetChildRectByRate(rectRate);
            var childMat = GetChildMatByRect(mat, relativeRect);
            return childMat;
        }
        

        public static Mat ReadMatFromFile(string imgPath)
        {
            if (!File.Exists(imgPath))
                throw new BreakException($"Missing MatImg: {imgPath}");
            return Cv2.ImRead(imgPath, ImreadModes.Unchanged);
        }

        public static Mat ToGray(this Mat source)
        {
            var gray = new Mat();
            var channels = source.Channels();
            var code = channels == 4 ? ColorConversionCodes.BGRA2GRAY : ColorConversionCodes.BGR2GRAY;
            Cv2.CvtColor(source, gray, code);
            return gray;
        }

        public static Mat ToBinary(this Mat gray, int threshold)
        {
            var bin = new Mat();
            Cv2.Threshold(gray, bin, threshold, 255, ThresholdTypes.Binary);
            return bin;
        }

        public static Mat ToReverse(this Mat mat)
        {
            return (~mat).ToMat();
        }
    }

    public static class ListExtension
    {
        public static T GetByIndex<T>(this List<T> list, int index)
        {
            if (list == null)
                return default(T);
            if (index >= list.Count)
                return default(T);
            return list[index];
        }
    }
}
