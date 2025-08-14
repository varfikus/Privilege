namespace PrivilegeAdmin
{
    partial class MainForm
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
            toolStrip1 = new ToolStrip();
            button_add = new ToolStripButton();
            button_edit = new ToolStripButton();
            button_del = new ToolStripButton();
            button_settings = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripSeparator2 = new ToolStripSeparator();
            button_refresh = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            button_order = new ToolStripButton();
            button_files = new ToolStripButton();
            button_update = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            dGV = new DataGridView();
            id = new DataGridViewTextBoxColumn();
            Login = new DataGridViewTextBoxColumn();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dGV).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.LightCyan;
            toolStrip1.ImageScalingSize = new Size(45, 45);
            toolStrip1.Items.AddRange(new ToolStripItem[] { button_add, button_edit, button_del, button_settings, toolStripSeparator1, toolStripSeparator2, button_refresh, toolStripSeparator4, button_order, button_files, button_update });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(975, 52);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // button_add
            // 
            button_add.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_add.Image = Properties.Resource.add_icon;
            button_add.ImageTransparentColor = Color.Magenta;
            button_add.Name = "button_add";
            button_add.Size = new Size(49, 49);
            button_add.Text = "Добавить пользователя";
            button_add.Click += button_add_Click;
            // 
            // button_edit
            // 
            button_edit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_edit.Image = Properties.Resource.edit_icon;
            button_edit.ImageTransparentColor = Color.Magenta;
            button_edit.Name = "button_edit";
            button_edit.Size = new Size(49, 49);
            button_edit.Text = "Изменить запись";
            button_edit.Click += button_edit_Click;
            // 
            // button_del
            // 
            button_del.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_del.Image = Properties.Resource.delete_icon;
            button_del.ImageTransparentColor = Color.Magenta;
            button_del.Name = "button_del";
            button_del.Size = new Size(49, 49);
            button_del.Text = "Удалить запись";
            button_del.Click += button_del_Click;
            // 
            // button_settings
            // 
            button_settings.Alignment = ToolStripItemAlignment.Right;
            button_settings.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_settings.Image = Properties.Resource.settings_blue_icon;
            button_settings.ImageTransparentColor = Color.Magenta;
            button_settings.Name = "button_settings";
            button_settings.Size = new Size(49, 49);
            button_settings.Text = "Настройки подключения";
            button_settings.Click += button_settings_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 52);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Alignment = ToolStripItemAlignment.Right;
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 52);
            // 
            // button_refresh
            // 
            button_refresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_refresh.Image = Properties.Resource.actions_view_refresh_icon;
            button_refresh.ImageTransparentColor = Color.Magenta;
            button_refresh.Name = "button_refresh";
            button_refresh.Size = new Size(49, 49);
            button_refresh.Text = "Обновить таблицу";
            button_refresh.Click += button_refresh_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 52);
            // 
            // button_order
            // 
            button_order.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_order.Image = Properties.Resource.order;
            button_order.ImageTransparentColor = Color.Magenta;
            button_order.Name = "button_order";
            button_order.Size = new Size(49, 49);
            button_order.Text = "Изменение заказов";
            button_order.Click += button_order_Click;
            // 
            // button_files
            // 
            button_files.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_files.Image = Properties.Resource.files;
            button_files.ImageTransparentColor = Color.Magenta;
            button_files.Name = "button_files";
            button_files.Size = new Size(49, 49);
            button_files.Text = "Список файлов";
            button_files.Click += button_files_Click;
            // 
            // button_update
            // 
            button_update.Alignment = ToolStripItemAlignment.Right;
            button_update.BackColor = SystemColors.Highlight;
            button_update.DisplayStyle = ToolStripItemDisplayStyle.Image;
            button_update.Image = Properties.Resource.updates_mini;
            button_update.ImageTransparentColor = Color.Magenta;
            button_update.Name = "button_update";
            button_update.Size = new Size(49, 49);
            button_update.Text = "Обновление";
            button_update.Click += button_update_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = Color.LightCyan;
            statusStrip1.Location = new Point(0, 535);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(975, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // dGV
            // 
            dGV.AllowUserToAddRows = false;
            dGV.AllowUserToDeleteRows = false;
            dGV.BackgroundColor = SystemColors.GrayText;
            dGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGV.Columns.AddRange(new DataGridViewColumn[] { id, Login });
            dGV.Dock = DockStyle.Fill;
            dGV.Location = new Point(0, 52);
            dGV.Margin = new Padding(4, 3, 4, 3);
            dGV.MultiSelect = false;
            dGV.Name = "dGV";
            dGV.ReadOnly = true;
            dGV.RowHeadersVisible = false;
            dGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV.Size = new Size(975, 483);
            dGV.TabIndex = 2;
            // 
            // id
            // 
            id.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            id.HeaderText = "ID";
            id.Name = "id";
            id.ReadOnly = true;
            id.Width = 43;
            // 
            // Login
            // 
            Login.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Login.HeaderText = "Логин";
            Login.Name = "Login";
            Login.ReadOnly = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(975, 557);
            Controls.Add(dGV);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(835, 476);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GAdmin";
            Load += MainForm_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dGV).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton button_add;
        private System.Windows.Forms.ToolStripButton button_edit;
        private System.Windows.Forms.ToolStripButton button_del;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dGV;
        private System.Windows.Forms.ToolStripButton button_settings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton button_refresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton button_order;
        private System.Windows.Forms.ToolStripButton button_files;
        private System.Windows.Forms.ToolStripButton button_update;
        private DataGridViewTextBoxColumn id;
        private DataGridViewTextBoxColumn Login;
    }
}
