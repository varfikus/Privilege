namespace ServiceMinsoc
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.button_start_stop = new System.Windows.Forms.ToolStripButton();
            this.button_settings = new System.Windows.Forms.ToolStripButton();
            this.button_update = new System.Windows.Forms.ToolStripButton();
            this.button_logs = new System.Windows.Forms.ToolStripButton();
            this.button_sendFtp = new System.Windows.Forms.ToolStripButton();
            this.dGV = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_start = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_close = new System.Windows.Forms.ToolStripMenuItem();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.info = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.button_start_stop,
            this.button_settings,
            this.button_update,
            this.button_logs,
            this.button_sendFtp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(688, 67);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // button_start_stop
            // 
            this.button_start_stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_start_stop.Image = global::ServiceMinsoc.Properties.Resources.play_icon;
            this.button_start_stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_start_stop.Name = "button_start_stop";
            this.button_start_stop.Size = new System.Drawing.Size(64, 64);
            this.button_start_stop.Text = "Запуск сервиса";
            this.button_start_stop.Click += new System.EventHandler(this.button_start_stop_Click);
            // 
            // button_settings
            // 
            this.button_settings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.button_settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_settings.Image = global::ServiceMinsoc.Properties.Resources.settings_2_icon;
            this.button_settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(64, 64);
            this.button_settings.Text = "Настройки";
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // button_update
            // 
            this.button_update.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.button_update.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_update.Image = global::ServiceMinsoc.Properties.Resources.download_icon;
            this.button_update.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(64, 64);
            this.button_update.Text = "Проверка на новую версию программы";
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // button_logs
            // 
            this.button_logs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.button_logs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_logs.Image = global::ServiceMinsoc.Properties.Resources.folder_icon;
            this.button_logs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_logs.Name = "button_logs";
            this.button_logs.Size = new System.Drawing.Size(64, 64);
            this.button_logs.Text = "Открыть папку логов";
            this.button_logs.Click += new System.EventHandler(this.button_logs_Click);
            // 
            // button_sendFtp
            // 
            this.button_sendFtp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.button_sendFtp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.button_sendFtp.Image = ((System.Drawing.Image)(resources.GetObject("button_sendFtp.Image")));
            this.button_sendFtp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.button_sendFtp.Name = "button_sendFtp";
            this.button_sendFtp.Size = new System.Drawing.Size(64, 64);
            this.button_sendFtp.Text = "Отправить файлы на FTP";
            this.button_sendFtp.Visible = false;
            this.button_sendFtp.Click += new System.EventHandler(this.button_sendFtp_Click);
            // 
            // dGV
            // 
            this.dGV.AllowUserToAddRows = false;
            this.dGV.AllowUserToDeleteRows = false;
            this.dGV.BackgroundColor = System.Drawing.SystemColors.WindowFrame;
            this.dGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.status,
            this.date,
            this.info});
            this.dGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV.Location = new System.Drawing.Point(0, 67);
            this.dGV.Name = "dGV";
            this.dGV.ReadOnly = true;
            this.dGV.Size = new System.Drawing.Size(688, 343);
            this.dGV.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Minsoc Service";
            this.notifyIcon1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_open,
            this.toolStripMenuItem_start,
            this.toolStripMenuItem_close});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(105, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem_open
            // 
            this.toolStripMenuItem_open.Name = "toolStripMenuItem_open";
            this.toolStripMenuItem_open.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem_open.Text = "Открыть";
            this.toolStripMenuItem_open.Click += new System.EventHandler(this.toolStripMenuItem_open_Click);
            // 
            // toolStripMenuItem_start
            // 
            this.toolStripMenuItem_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripMenuItem_start.Name = "toolStripMenuItem_start";
            this.toolStripMenuItem_start.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem_start.Text = "Запустить";
            this.toolStripMenuItem_start.Click += new System.EventHandler(this.button_start_stop_Click);
            // 
            // toolStripMenuItem_close
            // 
            this.toolStripMenuItem_close.Name = "toolStripMenuItem_close";
            this.toolStripMenuItem_close.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem_close.Text = "Закрыть";
            this.toolStripMenuItem_close.Click += new System.EventHandler(this.toolStripMenuItem_close_Click);
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.status.HeaderText = "Статус";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 66;
            // 
            // date
            // 
            this.date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Format = "G";
            dataGridViewCellStyle1.NullValue = null;
            this.date.DefaultCellStyle = dataGridViewCellStyle1;
            this.date.HeaderText = "Дата действия";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            this.date.Width = 99;
            // 
            // info
            // 
            this.info.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.info.HeaderText = "Информация";
            this.info.Name = "info";
            this.info.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 410);
            this.Controls.Add(this.dGV);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Minsoc Service";
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton button_start_stop;
        private System.Windows.Forms.ToolStripButton button_settings;
        private System.Windows.Forms.DataGridView dGV;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_open;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_start;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_close;
        private System.Windows.Forms.ToolStripButton button_logs;
        private System.Windows.Forms.ToolStripButton button_update;
        private System.Windows.Forms.ToolStripButton button_sendFtp;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn info;
    }
}

