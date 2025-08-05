namespace PrivilegeUI
{
    partial class AuthorizationForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel_login = new Panel();
            tB_login = new TextBox();
            panel_pass = new Panel();
            tB_pass = new TextBox();
            cB_save = new CheckBox();
            btn_ok = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            rB_director = new RadioButton();
            rB_client = new RadioButton();
            btn_settings = new Button();
            labelHeader = new Label();
            pB_pass = new PictureBox();
            pB_logo = new PictureBox();
            pB_user = new PictureBox();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pB_pass).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pB_logo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pB_user).BeginInit();
            SuspendLayout();
            // 
            // panel_login
            // 
            panel_login.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel_login.BackColor = Color.Gainsboro;
            panel_login.Location = new Point(14, 252);
            panel_login.Margin = new Padding(4, 3, 4, 3);
            panel_login.Name = "panel_login";
            panel_login.Size = new Size(379, 1);
            panel_login.TabIndex = 5;
            // 
            // tB_login
            // 
            tB_login.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_login.BackColor = Color.FromArgb(0, 36, 63);
            tB_login.BorderStyle = BorderStyle.None;
            tB_login.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_login.ForeColor = Color.Gainsboro;
            tB_login.Location = new Point(49, 222);
            tB_login.Margin = new Padding(4, 3, 4, 3);
            tB_login.Name = "tB_login";
            tB_login.Size = new Size(344, 18);
            tB_login.TabIndex = 4;
            // 
            // panel_pass
            // 
            panel_pass.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel_pass.BackColor = Color.Gainsboro;
            panel_pass.Location = new Point(14, 314);
            panel_pass.Margin = new Padding(4, 3, 4, 3);
            panel_pass.Name = "panel_pass";
            panel_pass.Size = new Size(379, 1);
            panel_pass.TabIndex = 7;
            // 
            // tB_pass
            // 
            tB_pass.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_pass.BackColor = Color.FromArgb(0, 36, 63);
            tB_pass.BorderStyle = BorderStyle.None;
            tB_pass.Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_pass.ForeColor = Color.Gainsboro;
            tB_pass.Location = new Point(49, 284);
            tB_pass.Margin = new Padding(4, 3, 4, 3);
            tB_pass.Name = "tB_pass";
            tB_pass.PasswordChar = '●';
            tB_pass.Size = new Size(344, 18);
            tB_pass.TabIndex = 6;
            tB_pass.KeyDown += tB_pass_KeyDown;
            // 
            // cB_save
            // 
            cB_save.AutoSize = true;
            cB_save.Font = new Font("Arial", 11.25F);
            cB_save.ForeColor = Color.Gainsboro;
            cB_save.Location = new Point(149, 321);
            cB_save.Margin = new Padding(4, 3, 4, 3);
            cB_save.Name = "cB_save";
            cB_save.Size = new Size(100, 21);
            cB_save.TabIndex = 8;
            cB_save.Text = "Сохранить";
            cB_save.UseVisualStyleBackColor = true;
            // 
            // btn_ok
            // 
            btn_ok.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btn_ok.BackColor = Color.FromArgb(38, 75, 119);
            btn_ok.FlatStyle = FlatStyle.Flat;
            btn_ok.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btn_ok.ForeColor = Color.Gainsboro;
            btn_ok.Location = new Point(14, 376);
            btn_ok.Margin = new Padding(4, 3, 4, 3);
            btn_ok.Name = "btn_ok";
            btn_ok.Size = new Size(379, 48);
            btn_ok.TabIndex = 10;
            btn_ok.Text = "Авторизация";
            btn_ok.UseVisualStyleBackColor = false;
            btn_ok.Click += btn_ok_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(rB_director, 1, 0);
            tableLayoutPanel2.Controls.Add(rB_client, 0, 0);
            tableLayoutPanel2.Location = new Point(14, 144);
            tableLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(379, 48);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // rB_director
            // 
            rB_director.Appearance = Appearance.Button;
            rB_director.AutoSize = true;
            rB_director.Dock = DockStyle.Fill;
            rB_director.FlatAppearance.CheckedBackColor = Color.FromArgb(38, 75, 119);
            rB_director.FlatStyle = FlatStyle.Flat;
            rB_director.Font = new Font("Arial", 12F);
            rB_director.ForeColor = Color.Gainsboro;
            rB_director.Location = new Point(193, 3);
            rB_director.Margin = new Padding(4, 3, 4, 3);
            rB_director.Name = "rB_director";
            rB_director.Size = new Size(182, 42);
            rB_director.TabIndex = 3;
            rB_director.TabStop = true;
            rB_director.Text = "Руководитель";
            rB_director.TextAlign = ContentAlignment.MiddleCenter;
            rB_director.UseVisualStyleBackColor = true;
            // 
            // rB_client
            // 
            rB_client.Appearance = Appearance.Button;
            rB_client.AutoSize = true;
            rB_client.Dock = DockStyle.Fill;
            rB_client.FlatAppearance.CheckedBackColor = Color.FromArgb(38, 75, 119);
            rB_client.FlatStyle = FlatStyle.Flat;
            rB_client.Font = new Font("Arial", 12F);
            rB_client.ForeColor = Color.Gainsboro;
            rB_client.Location = new Point(4, 3);
            rB_client.Margin = new Padding(4, 3, 4, 3);
            rB_client.Name = "rB_client";
            rB_client.Size = new Size(181, 42);
            rB_client.TabIndex = 2;
            rB_client.TabStop = true;
            rB_client.Text = "Клиент";
            rB_client.TextAlign = ContentAlignment.MiddleCenter;
            rB_client.UseVisualStyleBackColor = true;
            // 
            // btn_settings
            // 
            btn_settings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btn_settings.BackColor = Color.FromArgb(0, 36, 63);
            btn_settings.FlatStyle = FlatStyle.Flat;
            btn_settings.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            btn_settings.ForeColor = Color.Gainsboro;
            btn_settings.Location = new Point(14, 448);
            btn_settings.Margin = new Padding(4, 3, 4, 3);
            btn_settings.Name = "btn_settings";
            btn_settings.Size = new Size(379, 48);
            btn_settings.TabIndex = 11;
            btn_settings.Text = "Настройки";
            btn_settings.UseVisualStyleBackColor = false;
            // 
            // labelHeader
            // 
            labelHeader.Anchor = AnchorStyles.Top;
            labelHeader.AutoSize = true;
            labelHeader.Cursor = Cursors.Hand;
            labelHeader.Font = new Font("Arial Black", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelHeader.ForeColor = Color.WhiteSmoke;
            labelHeader.Location = new Point(89, 50);
            labelHeader.Margin = new Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(244, 30);
            labelHeader.TabIndex = 21;
            labelHeader.Text = "ГосАдминистрация";
            labelHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pB_pass
            // 
            pB_pass.Image = Properties.Resources.security_password_2_white_icon;
            pB_pass.Location = new Point(14, 279);
            pB_pass.Margin = new Padding(4, 3, 4, 3);
            pB_pass.Name = "pB_pass";
            pB_pass.Size = new Size(28, 28);
            pB_pass.SizeMode = PictureBoxSizeMode.Zoom;
            pB_pass.TabIndex = 20;
            pB_pass.TabStop = false;
            // 
            // pB_logo
            // 
            pB_logo.Anchor = AnchorStyles.Top;
            pB_logo.BorderStyle = BorderStyle.FixedSingle;
            pB_logo.Enabled = false;
            pB_logo.Location = new Point(149, 14);
            pB_logo.Margin = new Padding(4, 3, 4, 3);
            pB_logo.Name = "pB_logo";
            pB_logo.Size = new Size(108, 107);
            pB_logo.TabIndex = 17;
            pB_logo.TabStop = false;
            // 
            // pB_user
            // 
            pB_user.Image = Properties.Resources.user_white_96;
            pB_user.Location = new Point(14, 217);
            pB_user.Margin = new Padding(4, 3, 4, 3);
            pB_user.Name = "pB_user";
            pB_user.Size = new Size(28, 28);
            pB_user.SizeMode = PictureBoxSizeMode.Zoom;
            pB_user.TabIndex = 16;
            pB_user.TabStop = false;
            // 
            // AuthorizationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 36, 63);
            ClientSize = new Size(407, 510);
            Controls.Add(labelHeader);
            Controls.Add(btn_settings);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(btn_ok);
            Controls.Add(cB_save);
            Controls.Add(panel_pass);
            Controls.Add(tB_pass);
            Controls.Add(pB_pass);
            Controls.Add(pB_logo);
            Controls.Add(panel_login);
            Controls.Add(tB_login);
            Controls.Add(pB_user);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new Size(423, 549);
            MinimizeBox = false;
            MinimumSize = new Size(423, 549);
            Name = "AuthorizationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Авторизация";
            Load += AuthorizationForm_Load;
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pB_pass).EndInit();
            ((System.ComponentModel.ISupportInitialize)pB_logo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pB_user).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_login;
        private System.Windows.Forms.TextBox tB_login;
        private System.Windows.Forms.PictureBox pB_user;
        private System.Windows.Forms.PictureBox pB_logo;
        private System.Windows.Forms.Panel panel_pass;
        private System.Windows.Forms.TextBox tB_pass;
        private System.Windows.Forms.PictureBox pB_pass;
        private System.Windows.Forms.CheckBox cB_save;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rB_director;
        private System.Windows.Forms.RadioButton rB_client;
        private System.Windows.Forms.Button btn_settings;
        private System.Windows.Forms.Label labelHeader;
    }
}
