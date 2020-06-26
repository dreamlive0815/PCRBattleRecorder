using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PCRBattleRecorder
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        void OpenUrl(string url)
        {
            Process.Start(url);
        }

        void Copy(string s)
        {
            Clipboard.SetText(s);
            MessageBox.Show(Trans.T("已复制: {0}", s));
        }

        private void lnkAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = "https://zh.moegirl.org/%E5%8F%A4%E6%98%8E%E5%9C%B0%E6%81%8B";
            OpenUrl(url);
        }

        private void lblQQ_Click(object sender, EventArgs e)
        {
            Copy("995928339@qq.com");
        }

        private void lnkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = "https://github.com/dreamlive0815/PCRBattleRecorder";
            OpenUrl(url);
        }

        private void lnkGZLJ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = "https://redive.estertion.win/";
            OpenUrl(url);
        }
    }
}
