namespace PrivilegeAdmin
{
    partial class ApplicationForm
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
            toolStrip1 = new ToolStrip();
            button_add = new ToolStripButton();
            button_edit = new ToolStripButton();
            button_del = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            button_refresh = new ToolStripButton();
            dGV = new DataGridView();
            id = new DataGridViewTextBoxColumn();
            fio = new DataGridViewTextBoxColumn();
            status = new DataGridViewTextBoxColumn();
            createdAt = new DataGridViewTextBoxColumn();
            editAt = new DataGridViewTextBoxColumn();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dGV).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.LightCyan;
            toolStrip1.ImageScalingSize = new Size(45, 45);
            toolStrip1.Items.AddRange(new ToolStripItem[] { button_add, button_edit, button_del, toolStripSeparator1, button_refresh });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(784, 52);
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
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 52);
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
            // dGV
            // 
            dGV.AllowUserToAddRows = false;
            dGV.AllowUserToDeleteRows = false;
            dGV.BackgroundColor = SystemColors.GrayText;
            dGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dGV.Columns.AddRange(new DataGridViewColumn[] { id, fio, status, createdAt, editAt });
            dGV.Dock = DockStyle.Fill;
            dGV.Location = new Point(0, 52);
            dGV.Margin = new Padding(4, 3, 4, 3);
            dGV.MultiSelect = false;
            dGV.Name = "dGV";
            dGV.ReadOnly = true;
            dGV.RowHeadersVisible = false;
            dGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV.Size = new Size(784, 409);
            dGV.TabIndex = 2;
            // 
            // id
            // 
            id.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            id.HeaderText = "ID заказа";
            id.Name = "id";
            id.ReadOnly = true;
            id.Width = 80;
            // 
            // fio
            // 
            fio.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            fio.HeaderText = "ФИО заказчика";
            fio.Name = "fio";
            fio.ReadOnly = true;
            // 
            // status
            // 
            status.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            status.HeaderText = "Статус";
            status.Name = "status";
            status.ReadOnly = true;
            status.Width = 68;
            // 
            // createdAt
            // 
            createdAt.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            createdAt.HeaderText = "Дата создания";
            createdAt.Name = "createdAt";
            createdAt.ReadOnly = true;
            createdAt.Width = 110;
            // 
            // editAt
            // 
            editAt.HeaderText = "Дата редактирования";
            editAt.Name = "editAt";
            editAt.ReadOnly = true;
            // 
            // ApplicationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(dGV);
            Controls.Add(toolStrip1);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(800, 400);
            Name = "ApplicationForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Управление услугами";
            Load += OrderForm_Load;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton button_refresh;
        private System.Windows.Forms.DataGridView dGV;
        private DataGridViewTextBoxColumn id;
        private DataGridViewTextBoxColumn fio;
        private DataGridViewTextBoxColumn status;
        private DataGridViewTextBoxColumn createdAt;
        private DataGridViewTextBoxColumn editAt;
    }
}