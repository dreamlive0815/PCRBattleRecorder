namespace PCRBattleRecorder
{
    partial class Frm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuRegions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegionMainland = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegionTaiwan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegionJapan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetAdbServerPath = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetTesseractPath = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetPCRTemplateDir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuScripts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowViewportSizeScript = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStopScript = new System.Windows.Forms.ToolStripMenuItem();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRegions,
            this.menuSettings,
            this.menuScripts,
            this.menuStopScript});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(677, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuRegions
            // 
            this.menuRegions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRegionMainland,
            this.menuRegionTaiwan,
            this.menuRegionJapan});
            this.menuRegions.Name = "menuRegions";
            this.menuRegions.Size = new System.Drawing.Size(60, 21);
            this.menuRegions.Text = "区域(&R)";
            // 
            // menuRegionMainland
            // 
            this.menuRegionMainland.Name = "menuRegionMainland";
            this.menuRegionMainland.Size = new System.Drawing.Size(100, 22);
            this.menuRegionMainland.Text = "国服";
            this.menuRegionMainland.Click += new System.EventHandler(this.menuRegionMainland_Click);
            // 
            // menuRegionTaiwan
            // 
            this.menuRegionTaiwan.Name = "menuRegionTaiwan";
            this.menuRegionTaiwan.Size = new System.Drawing.Size(100, 22);
            this.menuRegionTaiwan.Text = "台湾";
            this.menuRegionTaiwan.Click += new System.EventHandler(this.menuRegionTaiwan_Click);
            // 
            // menuRegionJapan
            // 
            this.menuRegionJapan.Name = "menuRegionJapan";
            this.menuRegionJapan.Size = new System.Drawing.Size(100, 22);
            this.menuRegionJapan.Text = "日本";
            this.menuRegionJapan.Click += new System.EventHandler(this.menuRegionJapan_Click);
            // 
            // menuSettings
            // 
            this.menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetAdbServerPath,
            this.menuSetTesseractPath,
            this.menuSetPCRTemplateDir});
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(59, 21);
            this.menuSettings.Text = "设置(&S)";
            // 
            // menuSetAdbServerPath
            // 
            this.menuSetAdbServerPath.Name = "menuSetAdbServerPath";
            this.menuSetAdbServerPath.Size = new System.Drawing.Size(222, 22);
            this.menuSetAdbServerPath.Text = "设置MumuAdbServer路径";
            this.menuSetAdbServerPath.Click += new System.EventHandler(this.menuSetAdbServerPath_Click);
            // 
            // menuSetTesseractPath
            // 
            this.menuSetTesseractPath.Name = "menuSetTesseractPath";
            this.menuSetTesseractPath.Size = new System.Drawing.Size(222, 22);
            this.menuSetTesseractPath.Text = "设置Tesseract主程序路径";
            this.menuSetTesseractPath.Click += new System.EventHandler(this.menuSetTesseractPath_Click);
            // 
            // menuSetPCRTemplateDir
            // 
            this.menuSetPCRTemplateDir.Name = "menuSetPCRTemplateDir";
            this.menuSetPCRTemplateDir.Size = new System.Drawing.Size(222, 22);
            this.menuSetPCRTemplateDir.Text = "设置PCR样图目录";
            this.menuSetPCRTemplateDir.Click += new System.EventHandler(this.menuSetPCRTemplateDir_Click);
            // 
            // menuScripts
            // 
            this.menuScripts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShowViewportSizeScript});
            this.menuScripts.Name = "menuScripts";
            this.menuScripts.Size = new System.Drawing.Size(59, 21);
            this.menuScripts.Text = "脚本(&T)";
            // 
            // menuShowViewportSizeScript
            // 
            this.menuShowViewportSizeScript.Name = "menuShowViewportSizeScript";
            this.menuShowViewportSizeScript.Size = new System.Drawing.Size(185, 22);
            this.menuShowViewportSizeScript.Text = "跟踪Mumu视口大小";
            this.menuShowViewportSizeScript.Click += new System.EventHandler(this.menuShowViewportSizeScript_Click);
            // 
            // menuStopScript
            // 
            this.menuStopScript.Name = "menuStopScript";
            this.menuStopScript.Size = new System.Drawing.Size(83, 21);
            this.menuStopScript.Text = "终止脚本(&E)";
            this.menuStopScript.Click += new System.EventHandler(this.menuStopScript_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(0, 25);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(677, 361);
            this.txtOutput.TabIndex = 2;
            this.txtOutput.Text = "";
            // 
            // Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 386);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Frm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PCRBattleRecorder";
            this.Load += new System.EventHandler(this.Frm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuRegions;
        private System.Windows.Forms.ToolStripMenuItem menuRegionMainland;
        private System.Windows.Forms.ToolStripMenuItem menuRegionTaiwan;
        private System.Windows.Forms.ToolStripMenuItem menuRegionJapan;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuSetAdbServerPath;
        private System.Windows.Forms.ToolStripMenuItem menuSetTesseractPath;
        private System.Windows.Forms.ToolStripMenuItem menuSetPCRTemplateDir;
        private System.Windows.Forms.ToolStripMenuItem menuScripts;
        private System.Windows.Forms.ToolStripMenuItem menuStopScript;
        private System.Windows.Forms.ToolStripMenuItem menuShowViewportSizeScript;
    }
}

