namespace ServiceMinsoc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tB_port = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tB_name = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tB_password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tB_userName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tB_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.num_interval = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tB_url = new System.Windows.Forms.TextBox();
            this.tB_url_port = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cB_post = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cB_crypto = new System.Windows.Forms.ComboBox();
            this.num_apply = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cB_autorun = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tB_ftpPort = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tB_ftpPass = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tB_ftpUser = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tB_ftpIp = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_apply)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tB_port);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tB_name);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tB_password);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tB_userName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tB_ip);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 159);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подключение к базе данных";
            // 
            // tB_port
            // 
            this.tB_port.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_port.Location = new System.Drawing.Point(104, 126);
            this.tB_port.Name = "tB_port";
            this.tB_port.Size = new System.Drawing.Size(191, 20);
            this.tB_port.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Порт";
            // 
            // tB_name
            // 
            this.tB_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_name.Location = new System.Drawing.Point(104, 100);
            this.tB_name.Name = "tB_name";
            this.tB_name.Size = new System.Drawing.Size(191, 20);
            this.tB_name.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Имя базы";
            // 
            // tB_password
            // 
            this.tB_password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_password.Location = new System.Drawing.Point(104, 74);
            this.tB_password.Name = "tB_password";
            this.tB_password.PasswordChar = '*';
            this.tB_password.Size = new System.Drawing.Size(191, 20);
            this.tB_password.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Пароль";
            // 
            // tB_userName
            // 
            this.tB_userName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_userName.Location = new System.Drawing.Point(104, 48);
            this.tB_userName.Name = "tB_userName";
            this.tB_userName.Size = new System.Drawing.Size(191, 20);
            this.tB_userName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Пользователь";
            // 
            // tB_ip
            // 
            this.tB_ip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_ip.Location = new System.Drawing.Point(104, 22);
            this.tB_ip.Name = "tB_ip";
            this.tB_ip.Size = new System.Drawing.Size(191, 20);
            this.tB_ip.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Адрес";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 474);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Интервал проверки информации в БД";
            // 
            // num_interval
            // 
            this.num_interval.Location = new System.Drawing.Point(223, 472);
            this.num_interval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_interval.Name = "num_interval";
            this.num_interval.Size = new System.Drawing.Size(64, 20);
            this.num_interval.TabIndex = 2;
            this.num_interval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(293, 474);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "мин.";
            // 
            // button_ok
            // 
            this.button_ok.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_ok.BackColor = System.Drawing.Color.Chartreuse;
            this.button_ok.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_ok.Location = new System.Drawing.Point(73, 567);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "ОК";
            this.button_ok.UseVisualStyleBackColor = false;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_cancel.BackColor = System.Drawing.Color.Salmon;
            this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_cancel.Location = new System.Drawing.Point(190, 567);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = false;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Url";
            // 
            // tB_url
            // 
            this.tB_url.Location = new System.Drawing.Point(57, 12);
            this.tB_url.Name = "tB_url";
            this.tB_url.Size = new System.Drawing.Size(174, 20);
            this.tB_url.TabIndex = 7;
            // 
            // tB_url_port
            // 
            this.tB_url_port.Location = new System.Drawing.Point(57, 38);
            this.tB_url_port.Name = "tB_url_port";
            this.tB_url_port.Size = new System.Drawing.Size(174, 20);
            this.tB_url_port.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Port";
            // 
            // cB_post
            // 
            this.cB_post.AutoSize = true;
            this.cB_post.Enabled = false;
            this.cB_post.Location = new System.Drawing.Point(18, 529);
            this.cB_post.Name = "cB_post";
            this.cB_post.Size = new System.Drawing.Size(216, 17);
            this.cB_post.TabIndex = 10;
            this.cB_post.Text = "Заглушка POST запроса (для тестов)";
            this.cB_post.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 384);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Выбор сертификата";
            // 
            // cB_crypto
            // 
            this.cB_crypto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_crypto.FormattingEnabled = true;
            this.cB_crypto.Location = new System.Drawing.Point(18, 400);
            this.cB_crypto.Name = "cB_crypto";
            this.cB_crypto.Size = new System.Drawing.Size(289, 21);
            this.cB_crypto.TabIndex = 12;
            // 
            // num_apply
            // 
            this.num_apply.Location = new System.Drawing.Point(221, 436);
            this.num_apply.Name = "num_apply";
            this.num_apply.Size = new System.Drawing.Size(86, 20);
            this.num_apply.TabIndex = 16;
            this.num_apply.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 438);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(200, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Срок на принятие заявления в работу";
            // 
            // cB_autorun
            // 
            this.cB_autorun.AutoSize = true;
            this.cB_autorun.Location = new System.Drawing.Point(18, 500);
            this.cB_autorun.Name = "cB_autorun";
            this.cB_autorun.Size = new System.Drawing.Size(168, 17);
            this.cB_autorun.TabIndex = 17;
            this.cB_autorun.Text = "Запуск при старте системы";
            this.cB_autorun.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tB_ftpPort);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.tB_ftpPass);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.tB_ftpUser);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.tB_ftpIp);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Location = new System.Drawing.Point(12, 239);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 130);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Подключение к FTP";
            // 
            // tB_ftpPort
            // 
            this.tB_ftpPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_ftpPort.Location = new System.Drawing.Point(104, 97);
            this.tB_ftpPort.Name = "tB_ftpPort";
            this.tB_ftpPort.Size = new System.Drawing.Size(191, 20);
            this.tB_ftpPort.TabIndex = 9;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Порт";
            // 
            // tB_ftpPass
            // 
            this.tB_ftpPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_ftpPass.Location = new System.Drawing.Point(104, 71);
            this.tB_ftpPass.Name = "tB_ftpPass";
            this.tB_ftpPass.PasswordChar = '*';
            this.tB_ftpPass.Size = new System.Drawing.Size(191, 20);
            this.tB_ftpPass.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "Пароль";
            // 
            // tB_ftpUser
            // 
            this.tB_ftpUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_ftpUser.Location = new System.Drawing.Point(104, 45);
            this.tB_ftpUser.Name = "tB_ftpUser";
            this.tB_ftpUser.Size = new System.Drawing.Size(191, 20);
            this.tB_ftpUser.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Пользователь";
            // 
            // tB_ftpIp
            // 
            this.tB_ftpIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_ftpIp.Location = new System.Drawing.Point(104, 19);
            this.tB_ftpIp.Name = "tB_ftpIp";
            this.tB_ftpIp.Size = new System.Drawing.Size(191, 20);
            this.tB_ftpIp.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Адрес";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(337, 602);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cB_autorun);
            this.Controls.Add(this.num_apply);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cB_crypto);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cB_post);
            this.Controls.Add(this.tB_url_port);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tB_url);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.num_interval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(353, 641);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(353, 641);
            this.Name = "FormSettings";
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_apply)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tB_port;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tB_name;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tB_password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tB_userName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tB_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown num_interval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tB_url;
        private System.Windows.Forms.TextBox tB_url_port;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cB_post;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cB_crypto;
        private System.Windows.Forms.NumericUpDown num_apply;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cB_autorun;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tB_ftpPort;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tB_ftpPass;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tB_ftpUser;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tB_ftpIp;
        private System.Windows.Forms.Label label12;
    }
}