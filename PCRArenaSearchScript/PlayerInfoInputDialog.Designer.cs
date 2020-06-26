namespace PCRArenaSearchScript
{
    partial class PlayerInfoInputDialog
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
            this.txtRank = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRank = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.rdoAnd = new System.Windows.Forms.RadioButton();
            this.rdoOr = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // txtRank
            // 
            this.txtRank.Location = new System.Drawing.Point(108, 88);
            this.txtRank.Margin = new System.Windows.Forms.Padding(4);
            this.txtRank.Name = "txtRank";
            this.txtRank.Size = new System.Drawing.Size(321, 29);
            this.txtRank.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnOk.Location = new System.Drawing.Point(35, 138);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(173, 39);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(256, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(173, 39);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblRank
            // 
            this.lblRank.AutoSize = true;
            this.lblRank.Location = new System.Drawing.Point(31, 91);
            this.lblRank.Name = "lblRank";
            this.lblRank.Size = new System.Drawing.Size(73, 22);
            this.lblRank.TabIndex = 3;
            this.lblRank.Text = "排名：";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(31, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(73, 22);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "名字：";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(108, 17);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(321, 29);
            this.txtName.TabIndex = 4;
            // 
            // rdoAnd
            // 
            this.rdoAnd.AutoSize = true;
            this.rdoAnd.Location = new System.Drawing.Point(35, 53);
            this.rdoAnd.Name = "rdoAnd";
            this.rdoAnd.Size = new System.Drawing.Size(52, 26);
            this.rdoAnd.TabIndex = 6;
            this.rdoAnd.TabStop = true;
            this.rdoAnd.Text = "且";
            this.rdoAnd.UseVisualStyleBackColor = true;
            this.rdoAnd.CheckedChanged += new System.EventHandler(this.rdoAnd_CheckedChanged);
            // 
            // rdoOr
            // 
            this.rdoOr.AutoSize = true;
            this.rdoOr.Location = new System.Drawing.Point(108, 53);
            this.rdoOr.Name = "rdoOr";
            this.rdoOr.Size = new System.Drawing.Size(52, 26);
            this.rdoOr.TabIndex = 7;
            this.rdoOr.TabStop = true;
            this.rdoOr.Text = "或";
            this.rdoOr.UseVisualStyleBackColor = true;
            this.rdoOr.CheckedChanged += new System.EventHandler(this.rdoOr_CheckedChanged);
            // 
            // PlayerInfoInputDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 196);
            this.Controls.Add(this.rdoOr);
            this.Controls.Add(this.rdoAnd);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblRank);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtRank);
            this.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlayerInfoInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "输入目标玩家信息";
            this.Load += new System.EventHandler(this.PlayerInfoInputDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRank;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRank;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.RadioButton rdoAnd;
        private System.Windows.Forms.RadioButton rdoOr;
    }
}