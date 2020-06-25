using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCRTemplateImgTool
{
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {

        }

        public string RawInput
        {
            get { return txtInput.Text; }
        }

        public bool GetBool()
        {
            var input = RawInput;
            bool r;
            if (bool.TryParse(input, out r))
                return r;
            else
                return false;
        }

        public int GetInt()
        {
            var input = RawInput;
            int r;
            if (int.TryParse(input, out r))
                return r;
            else
                return 0;
        }

        public Vec4f GetVec4f()
        {
            var input = RawInput;
            var fReg = "(\\d\\.\\d*)";
            var sReg = "\\s*,\\s*";
            var match = Regex.Match(input, $"\\[{fReg}{sReg}{fReg}{sReg}{fReg}{sReg}{fReg}\\]");
            if (match.Success)
            {
                var getFloat = new Func<int, float>((index) =>
                {
                    var v = match.Groups[index].Value;
                    float f;
                    if (float.TryParse(v, out f))
                        return f;
                    else
                        return 0;
                });
                return new Vec4f(getFloat(1), getFloat(2), getFloat(3), getFloat(4));
            }
            fReg = "(\\d\\.\\d*)f";
            match = Regex.Match(input, $"\\({fReg}{sReg}{fReg}{sReg}{fReg}{sReg}{fReg}\\)");
            if (match.Success)
            {
                var getFloat = new Func<int, float>((index) =>
                {
                    var v = match.Groups[index].Value;
                    float f;
                    if (float.TryParse(v, out f))
                        return f;
                    else
                        return 0;
                });
                return new Vec4f(getFloat(1), getFloat(2), getFloat(3), getFloat(4));
            }
            return new Vec4f(0, 0, 1, 1);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
