namespace PCRBattleRecorder
{
    partial class FrmAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lnkAuthor = new System.Windows.Forms.LinkLabel();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblQQ = new System.Windows.Forms.Label();
            this.lblGithub = new System.Windows.Forms.Label();
            this.lnkGithub = new System.Windows.Forms.LinkLabel();
            this.lblFriendLinks = new System.Windows.Forms.Label();
            this.lnkGZLJ = new System.Windows.Forms.LinkLabel();
            this.lblGZLJ = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(13, 9);
            this.lblAuthor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(73, 22);
            this.lblAuthor.TabIndex = 0;
            this.lblAuthor.Text = "作者：";
            // 
            // lnkAuthor
            // 
            this.lnkAuthor.AutoSize = true;
            this.lnkAuthor.Location = new System.Drawing.Point(83, 9);
            this.lnkAuthor.Name = "lnkAuthor";
            this.lnkAuthor.Size = new System.Drawing.Size(70, 22);
            this.lnkAuthor.TabIndex = 1;
            this.lnkAuthor.TabStop = true;
            this.lnkAuthor.Text = "Koishi";
            this.lnkAuthor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAuthor_LinkClicked);
            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.Location = new System.Drawing.Point(13, 42);
            this.lblContact.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(73, 22);
            this.lblContact.TabIndex = 2;
            this.lblContact.Text = "联系：";
            // 
            // lblQQ
            // 
            this.lblQQ.AutoSize = true;
            this.lblQQ.Location = new System.Drawing.Point(83, 42);
            this.lblQQ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQQ.Name = "lblQQ";
            this.lblQQ.Size = new System.Drawing.Size(170, 22);
            this.lblQQ.TabIndex = 3;
            this.lblQQ.Text = "995928339@qq.com";
            this.lblQQ.Click += new System.EventHandler(this.lblQQ_Click);
            // 
            // lblGithub
            // 
            this.lblGithub.AutoSize = true;
            this.lblGithub.Location = new System.Drawing.Point(13, 76);
            this.lblGithub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGithub.Name = "lblGithub";
            this.lblGithub.Size = new System.Drawing.Size(91, 22);
            this.lblGithub.TabIndex = 4;
            this.lblGithub.Text = "Github：";
            // 
            // lnkGithub
            // 
            this.lnkGithub.AutoSize = true;
            this.lnkGithub.Location = new System.Drawing.Point(95, 76);
            this.lnkGithub.Name = "lnkGithub";
            this.lnkGithub.Size = new System.Drawing.Size(510, 22);
            this.lnkGithub.TabIndex = 5;
            this.lnkGithub.TabStop = true;
            this.lnkGithub.Text = "https://github.com/dreamlive0815/PCRBattleRecorder";
            this.lnkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGithub_LinkClicked);
            // 
            // lblFriendLinks
            // 
            this.lblFriendLinks.AutoSize = true;
            this.lblFriendLinks.Location = new System.Drawing.Point(13, 127);
            this.lblFriendLinks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFriendLinks.Name = "lblFriendLinks";
            this.lblFriendLinks.Size = new System.Drawing.Size(94, 22);
            this.lblFriendLinks.TabIndex = 6;
            this.lblFriendLinks.Text = "友情链接";
            // 
            // lnkGZLJ
            // 
            this.lnkGZLJ.AutoSize = true;
            this.lnkGZLJ.Location = new System.Drawing.Point(187, 162);
            this.lnkGZLJ.Name = "lnkGZLJ";
            this.lnkGZLJ.Size = new System.Drawing.Size(300, 22);
            this.lnkGZLJ.TabIndex = 8;
            this.lnkGZLJ.TabStop = true;
            this.lnkGZLJ.Text = "https://redive.estertion.win/";
            this.lnkGZLJ.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGZLJ_LinkClicked);
            // 
            // lblGZLJ
            // 
            this.lblGZLJ.AutoSize = true;
            this.lblGZLJ.Location = new System.Drawing.Point(13, 162);
            this.lblGZLJ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGZLJ.Name = "lblGZLJ";
            this.lblGZLJ.Size = new System.Drawing.Size(167, 22);
            this.lblGZLJ.TabIndex = 7;
            this.lblGZLJ.Text = "干炸里脊资源站:";
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 212);
            this.Controls.Add(this.lnkGZLJ);
            this.Controls.Add(this.lblGZLJ);
            this.Controls.Add(this.lblFriendLinks);
            this.Controls.Add(this.lnkGithub);
            this.Controls.Add(this.lblGithub);
            this.Controls.Add(this.lblQQ);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.lnkAuthor);
            this.Controls.Add(this.lblAuthor);
            this.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "关于";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.LinkLabel lnkAuthor;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblQQ;
        private System.Windows.Forms.Label lblGithub;
        private System.Windows.Forms.LinkLabel lnkGithub;
        private System.Windows.Forms.Label lblFriendLinks;
        private System.Windows.Forms.LinkLabel lnkGZLJ;
        private System.Windows.Forms.Label lblGZLJ;
    }
}