using PrivilegeUI;

namespace PrivilegeUI
{
    partial class FormMain
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
            components = new System.ComponentModel.Container();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            btn_exit = new Button();
            panelLogo = new Panel();
            labelHeader = new Label();
            pB_header = new PictureBox();
            lblTitleClildForm = new Label();
            panelMenu = new Panel();
            btn_denyAnswer = new Button();
            btn_cancel = new Button();
            btn_archive = new Button();
            btn_settings = new Button();
            btn_finaly = new Button();
            btn_denial = new Button();
            btn_apply = new Button();
            btn_info = new Button();
            btn_refresh = new Button();
            panelShadow = new Panel();
            panelTitle = new Panel();
            btn_error = new Button();
            btn_update = new Button();
            pB_CurrentChildForm = new PictureBox();
            panelDown = new Panel();
            label4 = new Label();
            lbl_time = new Label();
            lbl_apply = new Label();
            label3 = new Label();
            lbl_add = new Label();
            label1 = new Label();
            panelDesktop = new Panel();
            timer_refresh = new System.Windows.Forms.Timer(components);
            panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pB_header).BeginInit();
            panelMenu.SuspendLayout();
            panelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pB_CurrentChildForm).BeginInit();
            panelDown.SuspendLayout();
            SuspendLayout();
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            // 
            // btn_exit
            // 
            btn_exit.Cursor = Cursors.Hand;
            btn_exit.Dock = DockStyle.Bottom;
            btn_exit.FlatAppearance.BorderSize = 0;
            btn_exit.FlatStyle = FlatStyle.Flat;
            btn_exit.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_exit.ForeColor = Color.Gainsboro;
            btn_exit.Image = Properties.Resources.go_back_white_40;
            btn_exit.ImageAlign = ContentAlignment.MiddleLeft;
            btn_exit.Location = new Point(0, 799);
            btn_exit.Margin = new Padding(4, 3, 4, 3);
            btn_exit.Name = "btn_exit";
            btn_exit.Padding = new Padding(12, 0, 0, 0);
            btn_exit.Size = new Size(257, 69);
            btn_exit.TabIndex = 7;
            btn_exit.Text = "Выход";
            btn_exit.UseVisualStyleBackColor = true;
            btn_exit.Visible = false;
            // 
            // panelLogo
            // 
            panelLogo.Controls.Add(labelHeader);
            panelLogo.Controls.Add(pB_header);
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Location = new Point(0, 0);
            panelLogo.Margin = new Padding(4, 3, 4, 3);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(257, 125);
            panelLogo.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.Anchor = AnchorStyles.Top;
            labelHeader.AutoSize = true;
            labelHeader.Cursor = Cursors.Hand;
            labelHeader.Font = new Font("Arial Black", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelHeader.ForeColor = Color.WhiteSmoke;
            labelHeader.Location = new Point(85, 15);
            labelHeader.Margin = new Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(95, 90);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Заявки\nНа\nЛьготу";
            labelHeader.TextAlign = ContentAlignment.MiddleCenter;
            labelHeader.Click += pB_header_Click;
            // 
            // pB_header
            // 
            pB_header.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pB_header.BorderStyle = BorderStyle.FixedSingle;
            pB_header.Cursor = Cursors.Hand;
            pB_header.Location = new Point(10, 5);
            pB_header.Margin = new Padding(4, 3, 4, 3);
            pB_header.Name = "pB_header";
            pB_header.Size = new Size(236, 113);
            pB_header.SizeMode = PictureBoxSizeMode.Zoom;
            pB_header.TabIndex = 1;
            pB_header.TabStop = false;
            pB_header.Click += pB_header_Click;
            // 
            // lblTitleClildForm
            // 
            lblTitleClildForm.Anchor = AnchorStyles.Left;
            lblTitleClildForm.AutoSize = true;
            lblTitleClildForm.Font = new Font("Arial Black", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblTitleClildForm.ForeColor = Color.Gainsboro;
            lblTitleClildForm.Location = new Point(78, 15);
            lblTitleClildForm.Margin = new Padding(4, 0, 4, 0);
            lblTitleClildForm.Name = "lblTitleClildForm";
            lblTitleClildForm.Size = new Size(149, 23);
            lblTitleClildForm.TabIndex = 1;
            lblTitleClildForm.Text = "Главный экран";
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(60, 91, 116);
            panelMenu.Controls.Add(btn_denyAnswer);
            panelMenu.Controls.Add(btn_cancel);
            panelMenu.Controls.Add(btn_archive);
            panelMenu.Controls.Add(btn_settings);
            panelMenu.Controls.Add(btn_exit);
            panelMenu.Controls.Add(btn_finaly);
            panelMenu.Controls.Add(btn_denial);
            panelMenu.Controls.Add(btn_apply);
            panelMenu.Controls.Add(btn_info);
            panelMenu.Controls.Add(btn_refresh);
            panelMenu.Controls.Add(panelLogo);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Margin = new Padding(4, 3, 4, 3);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(257, 868);
            panelMenu.TabIndex = 8;
            // 
            // btn_denyAnswer
            // 
            btn_denyAnswer.Cursor = Cursors.Hand;
            btn_denyAnswer.Dock = DockStyle.Top;
            btn_denyAnswer.FlatAppearance.BorderSize = 0;
            btn_denyAnswer.FlatStyle = FlatStyle.Flat;
            btn_denyAnswer.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_denyAnswer.ForeColor = Color.Gainsboro;
            btn_denyAnswer.Image = Properties.Resources.cancel_subscription_white_40;
            btn_denyAnswer.ImageAlign = ContentAlignment.MiddleLeft;
            btn_denyAnswer.Location = new Point(0, 539);
            btn_denyAnswer.Margin = new Padding(4, 3, 4, 3);
            btn_denyAnswer.Name = "btn_denyAnswer";
            btn_denyAnswer.Padding = new Padding(12, 0, 0, 0);
            btn_denyAnswer.Size = new Size(257, 69);
            btn_denyAnswer.TabIndex = 14;
            btn_denyAnswer.Tag = "Отказ в выдаче";
            btn_denyAnswer.Text = "Отказ\r\nв принятии";
            btn_denyAnswer.UseVisualStyleBackColor = true;
            btn_denyAnswer.Click += btn_denyAnswer_Click;
            // 
            // btn_cancel
            // 
            btn_cancel.Cursor = Cursors.Hand;
            btn_cancel.Dock = DockStyle.Top;
            btn_cancel.FlatAppearance.BorderSize = 0;
            btn_cancel.FlatStyle = FlatStyle.Flat;
            btn_cancel.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_cancel.ForeColor = Color.Gainsboro;
            btn_cancel.Image = Properties.Resources.cancel_subscription_white_40;
            btn_cancel.ImageAlign = ContentAlignment.MiddleLeft;
            btn_cancel.Location = new Point(0, 470);
            btn_cancel.Margin = new Padding(4, 3, 4, 3);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Padding = new Padding(12, 0, 0, 0);
            btn_cancel.Size = new Size(257, 69);
            btn_cancel.TabIndex = 11;
            btn_cancel.Tag = "Отказ в выдаче";
            btn_cancel.Text = "Отказ\r\nв выдаче";
            btn_cancel.UseVisualStyleBackColor = true;
            btn_cancel.Click += btn_cancel_Click;
            // 
            // btn_archive
            // 
            btn_archive.Cursor = Cursors.Hand;
            btn_archive.Dock = DockStyle.Bottom;
            btn_archive.FlatAppearance.BorderSize = 0;
            btn_archive.FlatStyle = FlatStyle.Flat;
            btn_archive.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_archive.ForeColor = Color.Gainsboro;
            btn_archive.Image = Properties.Resources.winrar_white_40;
            btn_archive.ImageAlign = ContentAlignment.MiddleLeft;
            btn_archive.Location = new Point(0, 661);
            btn_archive.Margin = new Padding(4, 3, 4, 3);
            btn_archive.Name = "btn_archive";
            btn_archive.Padding = new Padding(12, 0, 0, 0);
            btn_archive.Size = new Size(257, 69);
            btn_archive.TabIndex = 9;
            btn_archive.Tag = "Архив";
            btn_archive.Text = "Архив";
            btn_archive.UseVisualStyleBackColor = true;
            btn_archive.Click += btn_archive_Click;
            // 
            // btn_settings
            // 
            btn_settings.Cursor = Cursors.Hand;
            btn_settings.Dock = DockStyle.Bottom;
            btn_settings.FlatAppearance.BorderSize = 0;
            btn_settings.FlatStyle = FlatStyle.Flat;
            btn_settings.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_settings.ForeColor = Color.Gainsboro;
            btn_settings.Image = Properties.Resources.settings_white_40;
            btn_settings.ImageAlign = ContentAlignment.MiddleLeft;
            btn_settings.Location = new Point(0, 730);
            btn_settings.Margin = new Padding(4, 3, 4, 3);
            btn_settings.Name = "btn_settings";
            btn_settings.Padding = new Padding(12, 0, 0, 0);
            btn_settings.Size = new Size(257, 69);
            btn_settings.TabIndex = 6;
            btn_settings.Tag = "Настройки";
            btn_settings.Text = "Настройки";
            btn_settings.UseVisualStyleBackColor = true;
            btn_settings.Click += btn_settings_Click;
            // 
            // btn_finaly
            // 
            btn_finaly.Cursor = Cursors.Hand;
            btn_finaly.Dock = DockStyle.Top;
            btn_finaly.FlatAppearance.BorderSize = 0;
            btn_finaly.FlatStyle = FlatStyle.Flat;
            btn_finaly.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_finaly.ForeColor = Color.Gainsboro;
            btn_finaly.Image = Properties.Resources.submit_document_white_40;
            btn_finaly.ImageAlign = ContentAlignment.MiddleLeft;
            btn_finaly.Location = new Point(0, 401);
            btn_finaly.Margin = new Padding(4, 3, 4, 3);
            btn_finaly.Name = "btn_finaly";
            btn_finaly.Padding = new Padding(12, 0, 0, 0);
            btn_finaly.Size = new Size(257, 69);
            btn_finaly.TabIndex = 4;
            btn_finaly.Tag = "Выдать";
            btn_finaly.Text = "Выдать";
            btn_finaly.UseVisualStyleBackColor = true;
            btn_finaly.Click += btn_finaly_Click;
            // 
            // btn_denial
            // 
            btn_denial.Cursor = Cursors.Hand;
            btn_denial.Dock = DockStyle.Top;
            btn_denial.FlatAppearance.BorderSize = 0;
            btn_denial.FlatStyle = FlatStyle.Flat;
            btn_denial.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_denial.ForeColor = Color.Gainsboro;
            btn_denial.Image = Properties.Resources.decline;
            btn_denial.ImageAlign = ContentAlignment.MiddleLeft;
            btn_denial.Location = new Point(0, 332);
            btn_denial.Margin = new Padding(4, 3, 4, 3);
            btn_denial.Name = "btn_denial";
            btn_denial.Padding = new Padding(12, 0, 0, 0);
            btn_denial.Size = new Size(257, 69);
            btn_denial.TabIndex = 13;
            btn_denial.Tag = "Информация";
            btn_denial.Text = "Отклонить";
            btn_denial.UseVisualStyleBackColor = true;
            btn_denial.Click += btn_denial_Click;
            // 
            // btn_apply
            // 
            btn_apply.Cursor = Cursors.Hand;
            btn_apply.Dock = DockStyle.Top;
            btn_apply.FlatAppearance.BorderSize = 0;
            btn_apply.FlatStyle = FlatStyle.Flat;
            btn_apply.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_apply.ForeColor = Color.Gainsboro;
            btn_apply.Image = Properties.Resources.ok_white_40;
            btn_apply.ImageAlign = ContentAlignment.MiddleLeft;
            btn_apply.Location = new Point(0, 263);
            btn_apply.Margin = new Padding(4, 3, 4, 3);
            btn_apply.Name = "btn_apply";
            btn_apply.Padding = new Padding(12, 0, 0, 0);
            btn_apply.Size = new Size(257, 69);
            btn_apply.TabIndex = 12;
            btn_apply.Tag = "Информация";
            btn_apply.Text = "Принять\nв работу";
            btn_apply.UseVisualStyleBackColor = true;
            btn_apply.Click += btn_apply_Click;
            // 
            // btn_info
            // 
            btn_info.Cursor = Cursors.Hand;
            btn_info.Dock = DockStyle.Top;
            btn_info.FlatAppearance.BorderSize = 0;
            btn_info.FlatStyle = FlatStyle.Flat;
            btn_info.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_info.ForeColor = Color.Gainsboro;
            btn_info.Image = Properties.Resources.info_white_40;
            btn_info.ImageAlign = ContentAlignment.MiddleLeft;
            btn_info.Location = new Point(0, 194);
            btn_info.Margin = new Padding(4, 3, 4, 3);
            btn_info.Name = "btn_info";
            btn_info.Padding = new Padding(12, 0, 0, 0);
            btn_info.Size = new Size(257, 69);
            btn_info.TabIndex = 8;
            btn_info.Tag = "Информация";
            btn_info.Text = "Информация";
            btn_info.UseVisualStyleBackColor = true;
            btn_info.Click += btn_info_Click;
            // 
            // btn_refresh
            // 
            btn_refresh.Cursor = Cursors.Hand;
            btn_refresh.Dock = DockStyle.Top;
            btn_refresh.FlatAppearance.BorderSize = 0;
            btn_refresh.FlatStyle = FlatStyle.Flat;
            btn_refresh.Font = new Font("Arial Black", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            btn_refresh.ForeColor = Color.Gainsboro;
            btn_refresh.Image = Properties.Resources.update_left_rotation_white_40;
            btn_refresh.ImageAlign = ContentAlignment.MiddleLeft;
            btn_refresh.Location = new Point(0, 125);
            btn_refresh.Margin = new Padding(4, 3, 4, 3);
            btn_refresh.Name = "btn_refresh";
            btn_refresh.Padding = new Padding(12, 0, 0, 0);
            btn_refresh.Size = new Size(257, 69);
            btn_refresh.TabIndex = 1;
            btn_refresh.Tag = "Обновить";
            btn_refresh.Text = "Обновить";
            btn_refresh.UseVisualStyleBackColor = true;
            btn_refresh.Click += btn_refresh_Click;
            // 
            // panelShadow
            // 
            panelShadow.BackColor = Color.FromArgb(0, 36, 63);
            panelShadow.Dock = DockStyle.Top;
            panelShadow.Location = new Point(257, 55);
            panelShadow.Margin = new Padding(4, 3, 4, 3);
            panelShadow.Name = "panelShadow";
            panelShadow.Size = new Size(1124, 9);
            panelShadow.TabIndex = 10;
            // 
            // panelTitle
            // 
            panelTitle.BackColor = Color.FromArgb(38, 75, 119);
            panelTitle.Controls.Add(btn_error);
            panelTitle.Controls.Add(btn_update);
            panelTitle.Controls.Add(lblTitleClildForm);
            panelTitle.Controls.Add(pB_CurrentChildForm);
            panelTitle.Dock = DockStyle.Top;
            panelTitle.Location = new Point(257, 0);
            panelTitle.Margin = new Padding(4, 3, 4, 3);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new Size(1124, 55);
            panelTitle.TabIndex = 9;
            // 
            // btn_error
            // 
            btn_error.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_error.BackgroundImageLayout = ImageLayout.Zoom;
            btn_error.Cursor = Cursors.Hand;
            btn_error.FlatAppearance.BorderSize = 0;
            btn_error.FlatStyle = FlatStyle.Flat;
            btn_error.Image = Properties.Resources.warning1;
            btn_error.Location = new Point(1018, 6);
            btn_error.Margin = new Padding(4, 3, 4, 3);
            btn_error.Name = "btn_error";
            btn_error.Size = new Size(47, 46);
            btn_error.TabIndex = 4;
            btn_error.UseVisualStyleBackColor = true;
            btn_error.Click += btn_error_Click;
            // 
            // btn_update
            // 
            btn_update.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_update.BackgroundImageLayout = ImageLayout.Zoom;
            btn_update.Cursor = Cursors.Hand;
            btn_update.FlatAppearance.BorderSize = 0;
            btn_update.FlatStyle = FlatStyle.Flat;
            btn_update.Image = Properties.Resources.synchronize_white_40;
            btn_update.Location = new Point(1073, 5);
            btn_update.Margin = new Padding(4, 3, 4, 3);
            btn_update.Name = "btn_update";
            btn_update.Size = new Size(47, 46);
            btn_update.TabIndex = 3;
            btn_update.UseVisualStyleBackColor = true;
            btn_update.Click += btn_update_Click;
            // 
            // pB_CurrentChildForm
            // 
            pB_CurrentChildForm.Anchor = AnchorStyles.Left;
            pB_CurrentChildForm.BackgroundImageLayout = ImageLayout.Zoom;
            pB_CurrentChildForm.Image = Properties.Resources.home_white_96;
            pB_CurrentChildForm.Location = new Point(24, 5);
            pB_CurrentChildForm.Margin = new Padding(4, 3, 4, 3);
            pB_CurrentChildForm.Name = "pB_CurrentChildForm";
            pB_CurrentChildForm.Size = new Size(47, 46);
            pB_CurrentChildForm.SizeMode = PictureBoxSizeMode.Zoom;
            pB_CurrentChildForm.TabIndex = 0;
            pB_CurrentChildForm.TabStop = false;
            // 
            // panelDown
            // 
            panelDown.BackColor = Color.FromArgb(14, 51, 84);
            panelDown.Controls.Add(label4);
            panelDown.Controls.Add(lbl_time);
            panelDown.Controls.Add(lbl_apply);
            panelDown.Controls.Add(label3);
            panelDown.Controls.Add(lbl_add);
            panelDown.Controls.Add(label1);
            panelDown.Dock = DockStyle.Bottom;
            panelDown.Location = new Point(257, 833);
            panelDown.Margin = new Padding(4, 3, 4, 3);
            panelDown.Name = "panelDown";
            panelDown.Size = new Size(1124, 35);
            panelDown.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Right;
            label4.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.ForeColor = Color.Gainsboro;
            label4.Location = new Point(909, 0);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Padding = new Padding(6, 7, 0, 6);
            label4.Size = new Size(209, 29);
            label4.TabIndex = 5;
            label4.Text = "Последнее обновление таблицы:";
            // 
            // lbl_time
            // 
            lbl_time.AutoSize = true;
            lbl_time.Dock = DockStyle.Right;
            lbl_time.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_time.ForeColor = Color.Gainsboro;
            lbl_time.Location = new Point(1118, 0);
            lbl_time.Margin = new Padding(4, 0, 4, 0);
            lbl_time.Name = "lbl_time";
            lbl_time.Padding = new Padding(0, 7, 6, 6);
            lbl_time.Size = new Size(6, 29);
            lbl_time.TabIndex = 6;
            // 
            // lbl_apply
            // 
            lbl_apply.AutoSize = true;
            lbl_apply.Dock = DockStyle.Left;
            lbl_apply.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_apply.ForeColor = Color.Yellow;
            lbl_apply.Location = new Point(293, 0);
            lbl_apply.Margin = new Padding(4, 0, 4, 0);
            lbl_apply.Name = "lbl_apply";
            lbl_apply.Padding = new Padding(0, 7, 6, 6);
            lbl_apply.Size = new Size(20, 29);
            lbl_apply.TabIndex = 4;
            lbl_apply.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Left;
            label3.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.ForeColor = Color.Gainsboro;
            label3.Location = new Point(157, 0);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Padding = new Padding(6, 7, 0, 6);
            label3.Size = new Size(136, 29);
            label3.TabIndex = 3;
            label3.Text = "Ожидание решения:";
            // 
            // lbl_add
            // 
            lbl_add.AutoSize = true;
            lbl_add.Dock = DockStyle.Left;
            lbl_add.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_add.ForeColor = Color.Red;
            lbl_add.Location = new Point(137, 0);
            lbl_add.Margin = new Padding(4, 0, 4, 0);
            lbl_add.Name = "lbl_add";
            lbl_add.Padding = new Padding(0, 7, 6, 6);
            lbl_add.Size = new Size(20, 29);
            lbl_add.TabIndex = 2;
            lbl_add.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.ForeColor = Color.Gainsboro;
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(6, 7, 0, 6);
            label1.Size = new Size(137, 29);
            label1.TabIndex = 1;
            label1.Text = "Ожидание принятия:";
            // 
            // panelDesktop
            // 
            panelDesktop.AutoScroll = true;
            panelDesktop.BackColor = Color.FromArgb(148, 153, 165);
            panelDesktop.Dock = DockStyle.Fill;
            panelDesktop.Location = new Point(257, 64);
            panelDesktop.Margin = new Padding(4, 3, 4, 3);
            panelDesktop.Name = "panelDesktop";
            panelDesktop.Size = new Size(1124, 769);
            panelDesktop.TabIndex = 11;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1381, 868);
            Controls.Add(panelDesktop);
            Controls.Add(panelShadow);
            Controls.Add(panelTitle);
            Controls.Add(panelDown);
            Controls.Add(panelMenu);
            DoubleBuffered = true;
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(1397, 802);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Заявки АСОП";
            FormClosing += FormMain_FormClosing;
            FormClosed += FormMain_FormClosed;
            Shown += FormMain_Shown;
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pB_header).EndInit();
            panelMenu.ResumeLayout(false);
            panelTitle.ResumeLayout(false);
            panelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pB_CurrentChildForm).EndInit();
            panelDown.ResumeLayout(false);
            panelDown.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pB_header;
        private System.Windows.Forms.Button btn_settings;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_finaly;
        private System.Windows.Forms.Button btn_info;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Label lblTitleClildForm;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.PictureBox pB_CurrentChildForm;
        private System.Windows.Forms.Panel panelShadow;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Panel panelDown;
        private System.Windows.Forms.Panel panelDesktop;
        private System.Windows.Forms.Button btn_archive;
        private System.Windows.Forms.Button btn_cancel;
        private MyControls.DoubleBufferedDataGridView dGV;
        private System.Windows.Forms.Timer timer_refresh;
        private System.Windows.Forms.Label lbl_apply;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_time;
        private Button btn_apply;
        private Button btn_denial;
        private Button btn_error;
        private Button btn_denyAnswer;
    }
}
