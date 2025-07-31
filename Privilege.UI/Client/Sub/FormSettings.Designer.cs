namespace Privilege.UI.Window.Client.Sub
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelBack = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btn_ftp = new System.Windows.Forms.Button();
            this.btn_db = new System.Windows.Forms.Button();
            this.btn_user = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tabControl = new Privilege.UI.MyControls.TabControlWithoutHeader();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.num_tableRefresh = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cB_sert = new System.Windows.Forms.ComboBox();
            this.lbl_sert = new System.Windows.Forms.Label();
            this.tB_tel = new System.Windows.Forms.TextBox();
            this.tB_fio = new System.Windows.Forms.TextBox();
            this.lbl_tel = new System.Windows.Forms.Label();
            this.lbl_fio = new System.Windows.Forms.Label();
            this.tabPageDb = new System.Windows.Forms.TabPage();
            this.tB_port = new System.Windows.Forms.TextBox();
            this.lbl_port = new System.Windows.Forms.Label();
            this.tB_name = new System.Windows.Forms.TextBox();
            this.tB_pass = new System.Windows.Forms.TextBox();
            this.lbl_name = new System.Windows.Forms.Label();
            this.lbl_pass = new System.Windows.Forms.Label();
            this.tB_user = new System.Windows.Forms.TextBox();
            this.tB_ip = new System.Windows.Forms.TextBox();
            this.lbl_user = new System.Windows.Forms.Label();
            this.lbl_ip = new System.Windows.Forms.Label();
            this.tabPageFtp = new System.Windows.Forms.TabPage();
            this.tB_portFtp = new System.Windows.Forms.TextBox();
            this.lbl_portFtp = new System.Windows.Forms.Label();
            this.tB_passFtp = new System.Windows.Forms.TextBox();
            this.lbl_passFtp = new System.Windows.Forms.Label();
            this.tB_userFtp = new System.Windows.Forms.TextBox();
            this.tB_ipFtp = new System.Windows.Forms.TextBox();
            this.lbl_userFtp = new System.Windows.Forms.Label();
            this.lbl_ipFtp = new System.Windows.Forms.Label();
            this.panelBack.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_tableRefresh)).BeginInit();
            this.tabPageDb.SuspendLayout();
            this.tabPageFtp.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(12, 71);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(937, 2);
            this.panel1.TabIndex = 9;
            // 
            // panelBack
            // 
            this.panelBack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBack.Controls.Add(this.panel2);
            this.panelBack.Controls.Add(this.tabControl);
            this.panelBack.Controls.Add(this.panelMenu);
            this.panelBack.Location = new System.Drawing.Point(0, 80);
            this.panelBack.Name = "panelBack";
            this.panelBack.Size = new System.Drawing.Size(961, 474);
            this.panelBack.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(220, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2, 474);
            this.panel2.TabIndex = 26;
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(75)))), ((int)(((byte)(119)))));
            this.panelMenu.Controls.Add(this.btn_ftp);
            this.panelMenu.Controls.Add(this.btn_db);
            this.panelMenu.Controls.Add(this.btn_user);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(220, 474);
            this.panelMenu.TabIndex = 9;
            // 
            // btn_ftp
            // 
            this.btn_ftp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ftp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_ftp.FlatAppearance.BorderSize = 0;
            this.btn_ftp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ftp.Font = new System.Drawing.Font("Arial Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_ftp.ForeColor = System.Drawing.Color.Gainsboro;
            this.btn_ftp.Image = global::Privilege.UI.Properties.Resources.ftp_white_96;
            this.btn_ftp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_ftp.Location = new System.Drawing.Point(0, 120);
            this.btn_ftp.Name = "btn_ftp";
            this.btn_ftp.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btn_ftp.Size = new System.Drawing.Size(220, 60);
            this.btn_ftp.TabIndex = 3;
            this.btn_ftp.Tag = "Принять в работу";
            this.btn_ftp.Text = "    FTP";
            this.btn_ftp.UseVisualStyleBackColor = true;
            this.btn_ftp.Click += new System.EventHandler(this.btn_ftp_Click);
            // 
            // btn_db
            // 
            this.btn_db.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_db.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_db.FlatAppearance.BorderSize = 0;
            this.btn_db.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_db.Font = new System.Drawing.Font("Arial Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_db.ForeColor = System.Drawing.Color.Gainsboro;
            this.btn_db.Image = global::Privilege.UI.Properties.Resources.database_administrator_white_40;
            this.btn_db.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_db.Location = new System.Drawing.Point(0, 60);
            this.btn_db.Name = "btn_db";
            this.btn_db.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btn_db.Size = new System.Drawing.Size(220, 60);
            this.btn_db.TabIndex = 2;
            this.btn_db.Tag = "Информация";
            this.btn_db.Text = "    База данных";
            this.btn_db.UseVisualStyleBackColor = true;
            this.btn_db.Click += new System.EventHandler(this.btn_db_Click);
            // 
            // btn_user
            // 
            this.btn_user.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_user.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_user.FlatAppearance.BorderSize = 0;
            this.btn_user.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_user.Font = new System.Drawing.Font("Arial Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_user.ForeColor = System.Drawing.Color.Gainsboro;
            this.btn_user.Image = global::Privilege.UI.Properties.Resources.user_white_40;
            this.btn_user.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_user.Location = new System.Drawing.Point(0, 0);
            this.btn_user.Name = "btn_user";
            this.btn_user.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btn_user.Size = new System.Drawing.Size(220, 60);
            this.btn_user.TabIndex = 1;
            this.btn_user.Tag = "Обновить";
            this.btn_user.Text = "    Пользователь";
            this.btn_user.UseVisualStyleBackColor = true;
            this.btn_user.Click += new System.EventHandler(this.btn_user_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::Privilege.UI.Properties.Resources.go_back_white_96;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(12, 13);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 50);
            this.btnClose.TabIndex = 7;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImage = global::Privilege.UI.Properties.Resources.ok_white_96;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(68, 13);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(50, 50);
            this.btnOk.TabIndex = 8;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageUser);
            this.tabControl.Controls.Add(this.tabPageDb);
            this.tabControl.Controls.Add(this.tabPageFtp);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(220, 0);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(741, 474);
            this.tabControl.TabIndex = 6;
            // 
            // tabPageUser
            // 
            this.tabPageUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(75)))), ((int)(((byte)(119)))));
            this.tabPageUser.Controls.Add(this.label2);
            this.tabPageUser.Controls.Add(this.num_tableRefresh);
            this.tabPageUser.Controls.Add(this.label1);
            this.tabPageUser.Controls.Add(this.cB_sert);
            this.tabPageUser.Controls.Add(this.lbl_sert);
            this.tabPageUser.Controls.Add(this.tB_tel);
            this.tabPageUser.Controls.Add(this.tB_fio);
            this.tabPageUser.Controls.Add(this.lbl_tel);
            this.tabPageUser.Controls.Add(this.lbl_fio);
            this.tabPageUser.Location = new System.Drawing.Point(4, 25);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUser.Size = new System.Drawing.Size(733, 445);
            this.tabPageUser.TabIndex = 0;
            this.tabPageUser.Text = "tabPageUser";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(368, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 18);
            this.label2.TabIndex = 14;
            this.label2.Text = "мин";
            // 
            // num_tableRefresh
            // 
            this.num_tableRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.num_tableRefresh.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.num_tableRefresh.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.num_tableRefresh.Location = new System.Drawing.Point(265, 159);
            this.num_tableRefresh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_tableRefresh.Name = "num_tableRefresh";
            this.num_tableRefresh.Size = new System.Drawing.Size(97, 26);
            this.num_tableRefresh.TabIndex = 13;
            this.num_tableRefresh.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(41, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 18);
            this.label1.TabIndex = 12;
            this.label1.Text = "Период обновления таблицы";
            // 
            // cB_sert
            // 
            this.cB_sert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.cB_sert.DropDownWidth = 500;
            this.cB_sert.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cB_sert.ForeColor = System.Drawing.Color.Gainsboro;
            this.cB_sert.FormattingEnabled = true;
            this.cB_sert.Location = new System.Drawing.Point(144, 113);
            this.cB_sert.Name = "cB_sert";
            this.cB_sert.Size = new System.Drawing.Size(360, 26);
            this.cB_sert.TabIndex = 11;
            // 
            // lbl_sert
            // 
            this.lbl_sert.AutoSize = true;
            this.lbl_sert.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_sert.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_sert.Location = new System.Drawing.Point(41, 116);
            this.lbl_sert.Name = "lbl_sert";
            this.lbl_sert.Size = new System.Drawing.Size(97, 18);
            this.lbl_sert.TabIndex = 10;
            this.lbl_sert.Text = "Сертификат";
            // 
            // tB_tel
            // 
            this.tB_tel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_tel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_tel.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_tel.Location = new System.Drawing.Point(144, 66);
            this.tB_tel.Name = "tB_tel";
            this.tB_tel.Size = new System.Drawing.Size(360, 26);
            this.tB_tel.TabIndex = 9;
            // 
            // tB_fio
            // 
            this.tB_fio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_fio.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_fio.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_fio.Location = new System.Drawing.Point(144, 19);
            this.tB_fio.Name = "tB_fio";
            this.tB_fio.Size = new System.Drawing.Size(360, 26);
            this.tB_fio.TabIndex = 8;
            // 
            // lbl_tel
            // 
            this.lbl_tel.AutoSize = true;
            this.lbl_tel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_tel.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_tel.Location = new System.Drawing.Point(67, 69);
            this.lbl_tel.Name = "lbl_tel";
            this.lbl_tel.Size = new System.Drawing.Size(71, 18);
            this.lbl_tel.TabIndex = 7;
            this.lbl_tel.Text = "Телефон";
            // 
            // lbl_fio
            // 
            this.lbl_fio.AutoSize = true;
            this.lbl_fio.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_fio.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_fio.Location = new System.Drawing.Point(95, 22);
            this.lbl_fio.Name = "lbl_fio";
            this.lbl_fio.Size = new System.Drawing.Size(43, 18);
            this.lbl_fio.TabIndex = 6;
            this.lbl_fio.Text = "ФИО";
            // 
            // tabPageDb
            // 
            this.tabPageDb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(75)))), ((int)(((byte)(119)))));
            this.tabPageDb.Controls.Add(this.tB_port);
            this.tabPageDb.Controls.Add(this.lbl_port);
            this.tabPageDb.Controls.Add(this.tB_name);
            this.tabPageDb.Controls.Add(this.tB_pass);
            this.tabPageDb.Controls.Add(this.lbl_name);
            this.tabPageDb.Controls.Add(this.lbl_pass);
            this.tabPageDb.Controls.Add(this.tB_user);
            this.tabPageDb.Controls.Add(this.tB_ip);
            this.tabPageDb.Controls.Add(this.lbl_user);
            this.tabPageDb.Controls.Add(this.lbl_ip);
            this.tabPageDb.Location = new System.Drawing.Point(4, 22);
            this.tabPageDb.Name = "tabPageDb";
            this.tabPageDb.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDb.Size = new System.Drawing.Size(733, 448);
            this.tabPageDb.TabIndex = 1;
            this.tabPageDb.Text = "tabPageDb";
            // 
            // tB_port
            // 
            this.tB_port.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_port.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_port.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_port.Location = new System.Drawing.Point(144, 205);
            this.tB_port.Name = "tB_port";
            this.tB_port.Size = new System.Drawing.Size(360, 26);
            this.tB_port.TabIndex = 20;
            // 
            // lbl_port
            // 
            this.lbl_port.AutoSize = true;
            this.lbl_port.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_port.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_port.Location = new System.Drawing.Point(94, 208);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(44, 18);
            this.lbl_port.TabIndex = 18;
            this.lbl_port.Text = "Порт";
            // 
            // tB_name
            // 
            this.tB_name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_name.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_name.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_name.Location = new System.Drawing.Point(144, 160);
            this.tB_name.Name = "tB_name";
            this.tB_name.Size = new System.Drawing.Size(360, 26);
            this.tB_name.TabIndex = 17;
            // 
            // tB_pass
            // 
            this.tB_pass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_pass.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_pass.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_pass.Location = new System.Drawing.Point(144, 113);
            this.tB_pass.Name = "tB_pass";
            this.tB_pass.PasswordChar = '●';
            this.tB_pass.Size = new System.Drawing.Size(360, 26);
            this.tB_pass.TabIndex = 16;
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_name.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_name.Location = new System.Drawing.Point(22, 163);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(116, 18);
            this.lbl_name.TabIndex = 15;
            this.lbl_name.Text = "Наименование";
            // 
            // lbl_pass
            // 
            this.lbl_pass.AutoSize = true;
            this.lbl_pass.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_pass.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_pass.Location = new System.Drawing.Point(75, 116);
            this.lbl_pass.Name = "lbl_pass";
            this.lbl_pass.Size = new System.Drawing.Size(63, 18);
            this.lbl_pass.TabIndex = 14;
            this.lbl_pass.Text = "Пароль";
            // 
            // tB_user
            // 
            this.tB_user.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_user.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_user.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_user.Location = new System.Drawing.Point(144, 66);
            this.tB_user.Name = "tB_user";
            this.tB_user.Size = new System.Drawing.Size(360, 26);
            this.tB_user.TabIndex = 13;
            // 
            // tB_ip
            // 
            this.tB_ip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_ip.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_ip.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_ip.Location = new System.Drawing.Point(144, 19);
            this.tB_ip.Name = "tB_ip";
            this.tB_ip.Size = new System.Drawing.Size(360, 26);
            this.tB_ip.TabIndex = 12;
            // 
            // lbl_user
            // 
            this.lbl_user.AutoSize = true;
            this.lbl_user.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_user.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_user.Location = new System.Drawing.Point(27, 69);
            this.lbl_user.Name = "lbl_user";
            this.lbl_user.Size = new System.Drawing.Size(111, 18);
            this.lbl_user.TabIndex = 11;
            this.lbl_user.Text = "Пользователь";
            // 
            // lbl_ip
            // 
            this.lbl_ip.AutoSize = true;
            this.lbl_ip.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_ip.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_ip.Location = new System.Drawing.Point(116, 22);
            this.lbl_ip.Name = "lbl_ip";
            this.lbl_ip.Size = new System.Drawing.Size(22, 18);
            this.lbl_ip.TabIndex = 10;
            this.lbl_ip.Text = "IP";
            // 
            // tabPageFtp
            // 
            this.tabPageFtp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(75)))), ((int)(((byte)(119)))));
            this.tabPageFtp.Controls.Add(this.tB_portFtp);
            this.tabPageFtp.Controls.Add(this.lbl_portFtp);
            this.tabPageFtp.Controls.Add(this.tB_passFtp);
            this.tabPageFtp.Controls.Add(this.lbl_passFtp);
            this.tabPageFtp.Controls.Add(this.tB_userFtp);
            this.tabPageFtp.Controls.Add(this.tB_ipFtp);
            this.tabPageFtp.Controls.Add(this.lbl_userFtp);
            this.tabPageFtp.Controls.Add(this.lbl_ipFtp);
            this.tabPageFtp.Location = new System.Drawing.Point(4, 22);
            this.tabPageFtp.Name = "tabPageFtp";
            this.tabPageFtp.Size = new System.Drawing.Size(733, 448);
            this.tabPageFtp.TabIndex = 2;
            this.tabPageFtp.Text = "tabPageFtp";
            // 
            // tB_portFtp
            // 
            this.tB_portFtp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_portFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_portFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_portFtp.Location = new System.Drawing.Point(144, 160);
            this.tB_portFtp.Name = "tB_portFtp";
            this.tB_portFtp.Size = new System.Drawing.Size(360, 26);
            this.tB_portFtp.TabIndex = 30;
            // 
            // lbl_portFtp
            // 
            this.lbl_portFtp.AutoSize = true;
            this.lbl_portFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_portFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_portFtp.Location = new System.Drawing.Point(94, 163);
            this.lbl_portFtp.Name = "lbl_portFtp";
            this.lbl_portFtp.Size = new System.Drawing.Size(44, 18);
            this.lbl_portFtp.TabIndex = 29;
            this.lbl_portFtp.Text = "Порт";
            // 
            // tB_passFtp
            // 
            this.tB_passFtp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_passFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_passFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_passFtp.Location = new System.Drawing.Point(144, 113);
            this.tB_passFtp.Name = "tB_passFtp";
            this.tB_passFtp.PasswordChar = '●';
            this.tB_passFtp.Size = new System.Drawing.Size(360, 26);
            this.tB_passFtp.TabIndex = 27;
            // 
            // lbl_passFtp
            // 
            this.lbl_passFtp.AutoSize = true;
            this.lbl_passFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_passFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_passFtp.Location = new System.Drawing.Point(75, 116);
            this.lbl_passFtp.Name = "lbl_passFtp";
            this.lbl_passFtp.Size = new System.Drawing.Size(63, 18);
            this.lbl_passFtp.TabIndex = 25;
            this.lbl_passFtp.Text = "Пароль";
            // 
            // tB_userFtp
            // 
            this.tB_userFtp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_userFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_userFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_userFtp.Location = new System.Drawing.Point(144, 66);
            this.tB_userFtp.Name = "tB_userFtp";
            this.tB_userFtp.Size = new System.Drawing.Size(360, 26);
            this.tB_userFtp.TabIndex = 24;
            // 
            // tB_ipFtp
            // 
            this.tB_ipFtp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_ipFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_ipFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_ipFtp.Location = new System.Drawing.Point(144, 19);
            this.tB_ipFtp.Name = "tB_ipFtp";
            this.tB_ipFtp.Size = new System.Drawing.Size(360, 26);
            this.tB_ipFtp.TabIndex = 23;
            // 
            // lbl_userFtp
            // 
            this.lbl_userFtp.AutoSize = true;
            this.lbl_userFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_userFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_userFtp.Location = new System.Drawing.Point(27, 69);
            this.lbl_userFtp.Name = "lbl_userFtp";
            this.lbl_userFtp.Size = new System.Drawing.Size(111, 18);
            this.lbl_userFtp.TabIndex = 22;
            this.lbl_userFtp.Text = "Пользователь";
            // 
            // lbl_ipFtp
            // 
            this.lbl_ipFtp.AutoSize = true;
            this.lbl_ipFtp.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_ipFtp.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_ipFtp.Location = new System.Drawing.Point(116, 22);
            this.lbl_ipFtp.Name = "lbl_ipFtp";
            this.lbl_ipFtp.Size = new System.Drawing.Size(22, 18);
            this.lbl_ipFtp.TabIndex = 21;
            this.lbl_ipFtp.Text = "IP";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(75)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(961, 554);
            this.Controls.Add(this.panelBack);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(824, 458);
            this.Name = "FormSettings";
            this.Text = "FormSettings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.panelBack.ResumeLayout(false);
            this.panelMenu.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageUser.ResumeLayout(false);
            this.tabPageUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_tableRefresh)).EndInit();
            this.tabPageDb.ResumeLayout(false);
            this.tabPageDb.PerformLayout();
            this.tabPageFtp.ResumeLayout(false);
            this.tabPageFtp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panelBack;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btn_ftp;
        private System.Windows.Forms.Button btn_db;
        private System.Windows.Forms.Button btn_user;
        private MyControls.TabControlWithoutHeader tabControl;
        private System.Windows.Forms.TabPage tabPageUser;
        private System.Windows.Forms.ComboBox cB_sert;
        private System.Windows.Forms.Label lbl_sert;
        private System.Windows.Forms.TextBox tB_tel;
        private System.Windows.Forms.TextBox tB_fio;
        private System.Windows.Forms.Label lbl_tel;
        private System.Windows.Forms.Label lbl_fio;
        private System.Windows.Forms.TabPage tabPageDb;
        private System.Windows.Forms.TextBox tB_port;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.TextBox tB_name;
        private System.Windows.Forms.TextBox tB_pass;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Label lbl_pass;
        private System.Windows.Forms.TextBox tB_user;
        private System.Windows.Forms.TextBox tB_ip;
        private System.Windows.Forms.Label lbl_user;
        private System.Windows.Forms.Label lbl_ip;
        private System.Windows.Forms.TabPage tabPageFtp;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tB_portFtp;
        private System.Windows.Forms.Label lbl_portFtp;
        private System.Windows.Forms.TextBox tB_passFtp;
        private System.Windows.Forms.Label lbl_passFtp;
        private System.Windows.Forms.TextBox tB_userFtp;
        private System.Windows.Forms.TextBox tB_ipFtp;
        private System.Windows.Forms.Label lbl_userFtp;
        private System.Windows.Forms.Label lbl_ipFtp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown num_tableRefresh;
        private System.Windows.Forms.Label label1;
    }
}