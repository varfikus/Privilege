namespace PrivilegeUI.Sub
{
    partial class FormSettings
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
            panel1 = new Panel();
            panelBack = new Panel();
            panel2 = new Panel();
            panelMenu = new Panel();
            btn_ftp = new Button();
            btn_user = new Button();
            btnClose = new Button();
            btnOk = new Button();
            tabPageFtp = new TabPage();
            lbl_ipFtp = new Label();
            lbl_userFtp = new Label();
            tB_ipFtp = new TextBox();
            tB_userFtp = new TextBox();
            lbl_passFtp = new Label();
            tB_passFtp = new TextBox();
            lbl_portFtp = new Label();
            tB_portFtp = new TextBox();
            tabPageUser = new TabPage();
            lbl_fio = new Label();
            lbl_tel = new Label();
            tB_fio = new TextBox();
            tB_tel = new TextBox();
            lbl_sert = new Label();
            cB_sert = new ComboBox();
            label1 = new Label();
            num_tableRefresh = new NumericUpDown();
            label2 = new Label();
            tabControl = new PrivilegeUI.MyControls.TabControlWithoutHeader();
            panelBack.SuspendLayout();
            panelMenu.SuspendLayout();
            tabPageFtp.SuspendLayout();
            tabPageUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_tableRefresh).BeginInit();
            tabControl.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Gainsboro;
            panel1.ForeColor = SystemColors.ControlText;
            panel1.Location = new Point(12, 71);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(937, 2);
            panel1.TabIndex = 9;
            // 
            // panelBack
            // 
            panelBack.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBack.Controls.Add(panel2);
            panelBack.Controls.Add(tabControl);
            panelBack.Controls.Add(panelMenu);
            panelBack.Location = new Point(0, 80);
            panelBack.Name = "panelBack";
            panelBack.Size = new Size(961, 474);
            panelBack.TabIndex = 10;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Gainsboro;
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(220, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(2, 474);
            panel2.TabIndex = 26;
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(38, 75, 119);
            panelMenu.Controls.Add(btn_ftp);
            panelMenu.Controls.Add(btn_user);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(220, 474);
            panelMenu.TabIndex = 9;
            // 
            // btn_ftp
            // 
            btn_ftp.Cursor = Cursors.Hand;
            btn_ftp.Dock = DockStyle.Top;
            btn_ftp.FlatAppearance.BorderSize = 0;
            btn_ftp.FlatStyle = FlatStyle.Flat;
            btn_ftp.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_ftp.ForeColor = Color.Gainsboro;
            btn_ftp.Image = Properties.Resources.ftp_white_96;
            btn_ftp.ImageAlign = ContentAlignment.MiddleLeft;
            btn_ftp.Location = new Point(0, 60);
            btn_ftp.Name = "btn_ftp";
            btn_ftp.Padding = new Padding(10, 0, 0, 0);
            btn_ftp.Size = new Size(220, 60);
            btn_ftp.TabIndex = 3;
            btn_ftp.Tag = "Принять в работу";
            btn_ftp.Text = "    FTP";
            btn_ftp.UseVisualStyleBackColor = true;
            btn_ftp.Click += btn_ftp_Click;
            // 
            // btn_user
            // 
            btn_user.Cursor = Cursors.Hand;
            btn_user.Dock = DockStyle.Top;
            btn_user.FlatAppearance.BorderSize = 0;
            btn_user.FlatStyle = FlatStyle.Flat;
            btn_user.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_user.ForeColor = Color.Gainsboro;
            btn_user.Image = Properties.Resources.user_white_40;
            btn_user.ImageAlign = ContentAlignment.MiddleLeft;
            btn_user.Location = new Point(0, 0);
            btn_user.Name = "btn_user";
            btn_user.Padding = new Padding(10, 0, 0, 0);
            btn_user.Size = new Size(220, 60);
            btn_user.TabIndex = 1;
            btn_user.Tag = "Обновить";
            btn_user.Text = "    Пользователь";
            btn_user.UseVisualStyleBackColor = true;
            btn_user.Click += btn_user_Click;
            // 
            // btnClose
            // 
            btnClose.BackgroundImage = Properties.Resources.go_back_white_96;
            btnClose.BackgroundImageLayout = ImageLayout.Zoom;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Location = new Point(12, 13);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(50, 50);
            btnClose.TabIndex = 7;
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnOk
            // 
            btnOk.BackgroundImage = Properties.Resources.ok_white_96;
            btnOk.BackgroundImageLayout = ImageLayout.Zoom;
            btnOk.Cursor = Cursors.Hand;
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Location = new Point(68, 13);
            btnOk.Margin = new Padding(3, 4, 3, 4);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(50, 50);
            btnOk.TabIndex = 8;
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // tabPageFtp
            // 
            tabPageFtp.BackColor = Color.FromArgb(38, 75, 119);
            tabPageFtp.Controls.Add(tB_portFtp);
            tabPageFtp.Controls.Add(tB_passFtp);
            tabPageFtp.Controls.Add(tB_userFtp);
            tabPageFtp.Controls.Add(tB_ipFtp);
            tabPageFtp.Controls.Add(lbl_portFtp);
            tabPageFtp.Controls.Add(lbl_passFtp);
            tabPageFtp.Controls.Add(lbl_userFtp);
            tabPageFtp.Controls.Add(lbl_ipFtp);
            tabPageFtp.Location = new Point(4, 25);
            tabPageFtp.Name = "tabPageFtp";
            tabPageFtp.Size = new Size(733, 445);
            tabPageFtp.TabIndex = 2;
            tabPageFtp.Text = "tabPageFtp";
            // 
            // lbl_ipFtp
            // 
            lbl_ipFtp.AutoSize = true;
            lbl_ipFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_ipFtp.ForeColor = Color.Gainsboro;
            lbl_ipFtp.Location = new Point(116, 22);
            lbl_ipFtp.Name = "lbl_ipFtp";
            lbl_ipFtp.Size = new Size(22, 18);
            lbl_ipFtp.TabIndex = 21;
            lbl_ipFtp.Text = "IP";
            // 
            // lbl_userFtp
            // 
            lbl_userFtp.AutoSize = true;
            lbl_userFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_userFtp.ForeColor = Color.Gainsboro;
            lbl_userFtp.Location = new Point(27, 69);
            lbl_userFtp.Name = "lbl_userFtp";
            lbl_userFtp.Size = new Size(111, 18);
            lbl_userFtp.TabIndex = 22;
            lbl_userFtp.Text = "Пользователь";
            // 
            // tB_ipFtp
            // 
            tB_ipFtp.BackColor = Color.FromArgb(0, 36, 63);
            tB_ipFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_ipFtp.ForeColor = Color.Gainsboro;
            tB_ipFtp.Location = new Point(144, 19);
            tB_ipFtp.Name = "tB_ipFtp";
            tB_ipFtp.Size = new Size(360, 26);
            tB_ipFtp.TabIndex = 23;
            // 
            // tB_userFtp
            // 
            tB_userFtp.BackColor = Color.FromArgb(0, 36, 63);
            tB_userFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_userFtp.ForeColor = Color.Gainsboro;
            tB_userFtp.Location = new Point(144, 66);
            tB_userFtp.Name = "tB_userFtp";
            tB_userFtp.Size = new Size(360, 26);
            tB_userFtp.TabIndex = 24;
            // 
            // lbl_passFtp
            // 
            lbl_passFtp.AutoSize = true;
            lbl_passFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_passFtp.ForeColor = Color.Gainsboro;
            lbl_passFtp.Location = new Point(75, 116);
            lbl_passFtp.Name = "lbl_passFtp";
            lbl_passFtp.Size = new Size(63, 18);
            lbl_passFtp.TabIndex = 25;
            lbl_passFtp.Text = "Пароль";
            // 
            // tB_passFtp
            // 
            tB_passFtp.BackColor = Color.FromArgb(0, 36, 63);
            tB_passFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_passFtp.ForeColor = Color.Gainsboro;
            tB_passFtp.Location = new Point(144, 113);
            tB_passFtp.Name = "tB_passFtp";
            tB_passFtp.PasswordChar = '●';
            tB_passFtp.Size = new Size(360, 26);
            tB_passFtp.TabIndex = 27;
            // 
            // lbl_portFtp
            // 
            lbl_portFtp.AutoSize = true;
            lbl_portFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_portFtp.ForeColor = Color.Gainsboro;
            lbl_portFtp.Location = new Point(94, 163);
            lbl_portFtp.Name = "lbl_portFtp";
            lbl_portFtp.Size = new Size(44, 18);
            lbl_portFtp.TabIndex = 29;
            lbl_portFtp.Text = "Порт";
            // 
            // tB_portFtp
            // 
            tB_portFtp.BackColor = Color.FromArgb(0, 36, 63);
            tB_portFtp.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_portFtp.ForeColor = Color.Gainsboro;
            tB_portFtp.Location = new Point(144, 160);
            tB_portFtp.Name = "tB_portFtp";
            tB_portFtp.Size = new Size(360, 26);
            tB_portFtp.TabIndex = 30;
            // 
            // tabPageUser
            // 
            tabPageUser.BackColor = Color.FromArgb(38, 75, 119);
            tabPageUser.Controls.Add(label2);
            tabPageUser.Controls.Add(num_tableRefresh);
            tabPageUser.Controls.Add(label1);
            tabPageUser.Controls.Add(cB_sert);
            tabPageUser.Controls.Add(lbl_sert);
            tabPageUser.Controls.Add(tB_tel);
            tabPageUser.Controls.Add(tB_fio);
            tabPageUser.Controls.Add(lbl_tel);
            tabPageUser.Controls.Add(lbl_fio);
            tabPageUser.Location = new Point(4, 25);
            tabPageUser.Name = "tabPageUser";
            tabPageUser.Padding = new Padding(3);
            tabPageUser.Size = new Size(733, 445);
            tabPageUser.TabIndex = 0;
            tabPageUser.Text = "tabPageUser";
            // 
            // lbl_fio
            // 
            lbl_fio.AutoSize = true;
            lbl_fio.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_fio.ForeColor = Color.Gainsboro;
            lbl_fio.Location = new Point(95, 22);
            lbl_fio.Name = "lbl_fio";
            lbl_fio.Size = new Size(43, 18);
            lbl_fio.TabIndex = 6;
            lbl_fio.Text = "ФИО";
            // 
            // lbl_tel
            // 
            lbl_tel.AutoSize = true;
            lbl_tel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_tel.ForeColor = Color.Gainsboro;
            lbl_tel.Location = new Point(67, 69);
            lbl_tel.Name = "lbl_tel";
            lbl_tel.Size = new Size(71, 18);
            lbl_tel.TabIndex = 7;
            lbl_tel.Text = "Телефон";
            // 
            // tB_fio
            // 
            tB_fio.BackColor = Color.FromArgb(0, 36, 63);
            tB_fio.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_fio.ForeColor = Color.Gainsboro;
            tB_fio.Location = new Point(144, 19);
            tB_fio.Name = "tB_fio";
            tB_fio.Size = new Size(360, 26);
            tB_fio.TabIndex = 8;
            // 
            // tB_tel
            // 
            tB_tel.BackColor = Color.FromArgb(0, 36, 63);
            tB_tel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_tel.ForeColor = Color.Gainsboro;
            tB_tel.Location = new Point(144, 66);
            tB_tel.Name = "tB_tel";
            tB_tel.Size = new Size(360, 26);
            tB_tel.TabIndex = 9;
            // 
            // lbl_sert
            // 
            lbl_sert.AutoSize = true;
            lbl_sert.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_sert.ForeColor = Color.Gainsboro;
            lbl_sert.Location = new Point(41, 116);
            lbl_sert.Name = "lbl_sert";
            lbl_sert.Size = new Size(97, 18);
            lbl_sert.TabIndex = 10;
            lbl_sert.Text = "Сертификат";
            // 
            // cB_sert
            // 
            cB_sert.BackColor = Color.FromArgb(0, 36, 63);
            cB_sert.DropDownWidth = 500;
            cB_sert.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cB_sert.ForeColor = Color.Gainsboro;
            cB_sert.FormattingEnabled = true;
            cB_sert.Location = new Point(144, 113);
            cB_sert.Name = "cB_sert";
            cB_sert.Size = new Size(360, 26);
            cB_sert.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.ForeColor = Color.Gainsboro;
            label1.Location = new Point(41, 161);
            label1.Name = "label1";
            label1.Size = new Size(218, 18);
            label1.TabIndex = 12;
            label1.Text = "Период обновления таблицы";
            // 
            // num_tableRefresh
            // 
            num_tableRefresh.BackColor = Color.FromArgb(0, 36, 63);
            num_tableRefresh.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            num_tableRefresh.ForeColor = Color.WhiteSmoke;
            num_tableRefresh.Location = new Point(265, 159);
            num_tableRefresh.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            num_tableRefresh.Name = "num_tableRefresh";
            num_tableRefresh.Size = new Size(97, 26);
            num_tableRefresh.TabIndex = 13;
            num_tableRefresh.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.ForeColor = Color.Gainsboro;
            label2.Location = new Point(368, 161);
            label2.Name = "label2";
            label2.Size = new Size(35, 18);
            label2.TabIndex = 14;
            label2.Text = "мин";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPageUser);
            tabControl.Controls.Add(tabPageFtp);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(220, 0);
            tabControl.Multiline = true;
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(741, 474);
            tabControl.TabIndex = 6;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(38, 75, 119);
            ClientSize = new Size(961, 554);
            Controls.Add(panelBack);
            Controls.Add(panel1);
            Controls.Add(btnClose);
            Controls.Add(btnOk);
            Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(824, 458);
            Name = "FormSettings";
            Text = "FormSettings";
            Load += FormSettings_Load;
            panelBack.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            tabPageFtp.ResumeLayout(false);
            tabPageFtp.PerformLayout();
            tabPageUser.ResumeLayout(false);
            tabPageUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_tableRefresh).EndInit();
            tabControl.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panelBack;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btn_ftp;
        private System.Windows.Forms.Button btn_user;
        private System.Windows.Forms.Panel panel2;
        private MyControls.TabControlWithoutHeader tabControl;
        private TabPage tabPageUser;
        private Label label2;
        private NumericUpDown num_tableRefresh;
        private Label label1;
        private ComboBox cB_sert;
        private Label lbl_sert;
        private TextBox tB_tel;
        private TextBox tB_fio;
        private Label lbl_tel;
        private Label lbl_fio;
        private TabPage tabPageFtp;
        private TextBox tB_portFtp;
        private TextBox tB_passFtp;
        private TextBox tB_userFtp;
        private TextBox tB_ipFtp;
        private Label lbl_portFtp;
        private Label lbl_passFtp;
        private Label lbl_userFtp;
        private Label lbl_ipFtp;
    }
}