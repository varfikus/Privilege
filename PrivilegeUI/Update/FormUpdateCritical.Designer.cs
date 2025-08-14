namespace PrivilegeUI.Update
{
    partial class FormUpdateCritical
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_verNew = new System.Windows.Forms.Label();
            this.lbl_ver = new System.Windows.Forms.Label();
            this.lbl_verCurrent = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(91, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 19);
            this.label1.TabIndex = 12;
            this.label1.Text = "Найдено критическое обновление";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(31, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(387, 19);
            this.label2.TabIndex = 15;
            this.label2.Text = "Это приложение будет автоматически закрыто";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Gainsboro;
            this.label3.Location = new System.Drawing.Point(95, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(257, 19);
            this.label3.TabIndex = 14;
            this.label3.Text = "Будет загружена новая версия";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOk.BackColor = System.Drawing.Color.LimeGreen;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOk.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOk.Location = new System.Drawing.Point(127, 84);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(209, 37);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "ОК";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.Controls.Add(this.lbl_verNew, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_ver, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_verCurrent, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 41);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(429, 34);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // lbl_verNew
            // 
            this.lbl_verNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_verNew.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_verNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbl_verNew.Location = new System.Drawing.Point(238, 0);
            this.lbl_verNew.Name = "lbl_verNew";
            this.lbl_verNew.Size = new System.Drawing.Size(188, 34);
            this.lbl_verNew.TabIndex = 14;
            this.lbl_verNew.Text = "v2";
            this.lbl_verNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_ver
            // 
            this.lbl_ver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_ver.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_ver.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_ver.Location = new System.Drawing.Point(196, 0);
            this.lbl_ver.Name = "lbl_ver";
            this.lbl_ver.Size = new System.Drawing.Size(36, 34);
            this.lbl_ver.TabIndex = 13;
            this.lbl_ver.Text = "🡺";
            this.lbl_ver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_verCurrent
            // 
            this.lbl_verCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_verCurrent.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_verCurrent.ForeColor = System.Drawing.Color.Red;
            this.lbl_verCurrent.Location = new System.Drawing.Point(3, 0);
            this.lbl_verCurrent.Name = "lbl_verCurrent";
            this.lbl_verCurrent.Size = new System.Drawing.Size(187, 34);
            this.lbl_verCurrent.TabIndex = 12;
            this.lbl_verCurrent.Text = "v1";
            this.lbl_verCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormUpdateCritical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(51)))), ((int)(((byte)(84)))));
            this.ClientSize = new System.Drawing.Size(453, 205);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(469, 244);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(469, 244);
            this.Name = "FormUpdateCritical";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Критическое обновление";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUpdateCritical_FormClosing);
            this.Shown += new System.EventHandler(this.FormUpdateCritical_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbl_verNew;
        private System.Windows.Forms.Label lbl_ver;
        private System.Windows.Forms.Label lbl_verCurrent;
    }
}