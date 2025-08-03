namespace Privilege.UI.Window.Client.Sub.Issue
{
    partial class FormAnsPaper
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
            this.components = new System.ComponentModel.Container();
            this.lbl_header = new System.Windows.Forms.Label();
            this.dTP_dateOut = new System.Windows.Forms.DateTimePicker();
            this.lbl_date = new System.Windows.Forms.Label();
            this.tB_fio = new System.Windows.Forms.TextBox();
            this.lbl_fio = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tB_service = new System.Windows.Forms.TextBox();
            this.lbl_service = new System.Windows.Forms.Label();
            this.tB_operatorTel = new System.Windows.Forms.TextBox();
            this.lbl_operatorTel = new System.Windows.Forms.Label();
            this.tB_operator = new System.Windows.Forms.TextBox();
            this.lbl_operator = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_preview = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tB_mesto = new System.Windows.Forms.TextBox();
            this.lbl_mesto = new System.Windows.Forms.Label();
            this.dTP_consid = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dTP_considTime = new System.Windows.Forms.DateTimePicker();
            this.lbl_considTime = new System.Windows.Forms.Label();
            this.dTP_timeFrom = new System.Windows.Forms.DateTimePicker();
            this.lbl_timeFrom = new System.Windows.Forms.Label();
            this.dTP_timeTo = new System.Windows.Forms.DateTimePicker();
            this.lbl_timeTo = new System.Windows.Forms.Label();
            this.tB_cab = new System.Windows.Forms.TextBox();
            this.lbl_cab = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lbl_header
            // 
            this.lbl_header.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_header.Font = new System.Drawing.Font("Arial Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_header.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_header.Location = new System.Drawing.Point(180, 20);
            this.lbl_header.Name = "lbl_header";
            this.lbl_header.Size = new System.Drawing.Size(616, 31);
            this.lbl_header.TabIndex = 3;
            this.lbl_header.Text = "Выдача документа в бумаге";
            this.lbl_header.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dTP_dateOut
            // 
            this.dTP_dateOut.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_dateOut.Location = new System.Drawing.Point(142, 248);
            this.dTP_dateOut.Name = "dTP_dateOut";
            this.dTP_dateOut.Size = new System.Drawing.Size(200, 26);
            this.dTP_dateOut.TabIndex = 15;
            // 
            // lbl_date
            // 
            this.lbl_date.AutoSize = true;
            this.lbl_date.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_date.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_date.Location = new System.Drawing.Point(12, 251);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Size = new System.Drawing.Size(124, 18);
            this.lbl_date.TabIndex = 14;
            this.lbl_date.Text = "Дата документа";
            // 
            // tB_fio
            // 
            this.tB_fio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_fio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_fio.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_fio.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_fio.Location = new System.Drawing.Point(142, 208);
            this.tB_fio.Name = "tB_fio";
            this.tB_fio.Size = new System.Drawing.Size(654, 26);
            this.tB_fio.TabIndex = 13;
            // 
            // lbl_fio
            // 
            this.lbl_fio.AutoSize = true;
            this.lbl_fio.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_fio.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_fio.Location = new System.Drawing.Point(12, 211);
            this.lbl_fio.Name = "lbl_fio";
            this.lbl_fio.Size = new System.Drawing.Size(121, 18);
            this.lbl_fio.TabIndex = 12;
            this.lbl_fio.Text = "ФИО заявителя";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Location = new System.Drawing.Point(12, 192);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 2);
            this.panel2.TabIndex = 11;
            // 
            // tB_service
            // 
            this.tB_service.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_service.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_service.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tB_service.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_service.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_service.Location = new System.Drawing.Point(182, 157);
            this.tB_service.Name = "tB_service";
            this.tB_service.ReadOnly = true;
            this.tB_service.Size = new System.Drawing.Size(614, 19);
            this.tB_service.TabIndex = 10;
            // 
            // lbl_service
            // 
            this.lbl_service.AutoSize = true;
            this.lbl_service.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_service.ForeColor = System.Drawing.Color.LightGray;
            this.lbl_service.Location = new System.Drawing.Point(12, 157);
            this.lbl_service.Name = "lbl_service";
            this.lbl_service.Size = new System.Drawing.Size(164, 18);
            this.lbl_service.TabIndex = 9;
            this.lbl_service.Text = "Наименование услуги";
            // 
            // tB_operatorTel
            // 
            this.tB_operatorTel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_operatorTel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_operatorTel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tB_operatorTel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_operatorTel.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_operatorTel.Location = new System.Drawing.Point(182, 122);
            this.tB_operatorTel.Name = "tB_operatorTel";
            this.tB_operatorTel.ReadOnly = true;
            this.tB_operatorTel.Size = new System.Drawing.Size(614, 19);
            this.tB_operatorTel.TabIndex = 8;
            // 
            // lbl_operatorTel
            // 
            this.lbl_operatorTel.AutoSize = true;
            this.lbl_operatorTel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_operatorTel.ForeColor = System.Drawing.Color.LightGray;
            this.lbl_operatorTel.Location = new System.Drawing.Point(12, 122);
            this.lbl_operatorTel.Name = "lbl_operatorTel";
            this.lbl_operatorTel.Size = new System.Drawing.Size(164, 18);
            this.lbl_operatorTel.TabIndex = 7;
            this.lbl_operatorTel.Text = "Телефон исполнителя";
            // 
            // tB_operator
            // 
            this.tB_operator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_operator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_operator.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tB_operator.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_operator.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_operator.Location = new System.Drawing.Point(119, 88);
            this.tB_operator.Name = "tB_operator";
            this.tB_operator.ReadOnly = true;
            this.tB_operator.Size = new System.Drawing.Size(677, 19);
            this.tB_operator.TabIndex = 6;
            // 
            // lbl_operator
            // 
            this.lbl_operator.AutoSize = true;
            this.lbl_operator.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_operator.ForeColor = System.Drawing.Color.LightGray;
            this.lbl_operator.Location = new System.Drawing.Point(12, 88);
            this.lbl_operator.Name = "lbl_operator";
            this.lbl_operator.Size = new System.Drawing.Size(101, 18);
            this.lbl_operator.TabIndex = 5;
            this.lbl_operator.Text = "Исполнитель";
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
            this.panel1.Size = new System.Drawing.Size(784, 2);
            this.panel1.TabIndex = 4;
            // 
            // btn_preview
            // 
            this.btn_preview.BackgroundImage = global::Privilege.UI.Properties.Resources.view_white_96;
            this.btn_preview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_preview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_preview.FlatAppearance.BorderSize = 0;
            this.btn_preview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_preview.Location = new System.Drawing.Point(124, 13);
            this.btn_preview.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_preview.Name = "btn_preview";
            this.btn_preview.Size = new System.Drawing.Size(50, 50);
            this.btn_preview.TabIndex = 2;
            this.btn_preview.UseVisualStyleBackColor = true;
            this.btn_preview.Click += new System.EventHandler(this.btn_preview_Click);
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
            this.btnClose.TabIndex = 0;
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
            this.btnOk.TabIndex = 1;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tB_mesto
            // 
            this.tB_mesto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_mesto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_mesto.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_mesto.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_mesto.Location = new System.Drawing.Point(230, 287);
            this.tB_mesto.Name = "tB_mesto";
            this.tB_mesto.Size = new System.Drawing.Size(566, 26);
            this.tB_mesto.TabIndex = 17;
            // 
            // lbl_mesto
            // 
            this.lbl_mesto.AutoSize = true;
            this.lbl_mesto.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_mesto.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_mesto.Location = new System.Drawing.Point(12, 290);
            this.lbl_mesto.Name = "lbl_mesto";
            this.lbl_mesto.Size = new System.Drawing.Size(212, 18);
            this.lbl_mesto.TabIndex = 16;
            this.lbl_mesto.Text = "Место получения документа";
            // 
            // dTP_consid
            // 
            this.dTP_consid.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_consid.Location = new System.Drawing.Point(92, 326);
            this.dTP_consid.Name = "dTP_consid";
            this.dTP_consid.Size = new System.Drawing.Size(200, 26);
            this.dTP_consid.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(12, 329);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 18);
            this.label3.TabIndex = 18;
            this.label3.Text = "Явиться";
            // 
            // dTP_considTime
            // 
            this.dTP_considTime.CustomFormat = "HH:mm";
            this.dTP_considTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_considTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_considTime.Location = new System.Drawing.Point(338, 326);
            this.dTP_considTime.Name = "dTP_considTime";
            this.dTP_considTime.ShowUpDown = true;
            this.dTP_considTime.Size = new System.Drawing.Size(93, 26);
            this.dTP_considTime.TabIndex = 21;
            // 
            // lbl_considTime
            // 
            this.lbl_considTime.AutoSize = true;
            this.lbl_considTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_considTime.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_considTime.Location = new System.Drawing.Point(307, 329);
            this.lbl_considTime.Name = "lbl_considTime";
            this.lbl_considTime.Size = new System.Drawing.Size(17, 18);
            this.lbl_considTime.TabIndex = 20;
            this.lbl_considTime.Text = "в";
            // 
            // dTP_timeFrom
            // 
            this.dTP_timeFrom.CustomFormat = "HH:mm";
            this.dTP_timeFrom.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_timeFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_timeFrom.Location = new System.Drawing.Point(92, 365);
            this.dTP_timeFrom.Name = "dTP_timeFrom";
            this.dTP_timeFrom.ShowUpDown = true;
            this.dTP_timeFrom.Size = new System.Drawing.Size(93, 26);
            this.dTP_timeFrom.TabIndex = 25;
            // 
            // lbl_timeFrom
            // 
            this.lbl_timeFrom.AutoSize = true;
            this.lbl_timeFrom.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_timeFrom.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_timeFrom.Location = new System.Drawing.Point(66, 368);
            this.lbl_timeFrom.Name = "lbl_timeFrom";
            this.lbl_timeFrom.Size = new System.Drawing.Size(16, 18);
            this.lbl_timeFrom.TabIndex = 24;
            this.lbl_timeFrom.Text = "с";
            // 
            // dTP_timeTo
            // 
            this.dTP_timeTo.CustomFormat = "HH:mm";
            this.dTP_timeTo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_timeTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTP_timeTo.Location = new System.Drawing.Point(249, 365);
            this.dTP_timeTo.Name = "dTP_timeTo";
            this.dTP_timeTo.ShowUpDown = true;
            this.dTP_timeTo.Size = new System.Drawing.Size(93, 26);
            this.dTP_timeTo.TabIndex = 27;
            // 
            // lbl_timeTo
            // 
            this.lbl_timeTo.AutoSize = true;
            this.lbl_timeTo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_timeTo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_timeTo.Location = new System.Drawing.Point(206, 368);
            this.lbl_timeTo.Name = "lbl_timeTo";
            this.lbl_timeTo.Size = new System.Drawing.Size(25, 18);
            this.lbl_timeTo.TabIndex = 26;
            this.lbl_timeTo.Text = "по";
            // 
            // tB_cab
            // 
            this.tB_cab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_cab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_cab.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_cab.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_cab.Location = new System.Drawing.Point(573, 326);
            this.tB_cab.Name = "tB_cab";
            this.tB_cab.Size = new System.Drawing.Size(223, 26);
            this.tB_cab.TabIndex = 23;
            // 
            // lbl_cab
            // 
            this.lbl_cab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_cab.AutoSize = true;
            this.lbl_cab.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_cab.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_cab.Location = new System.Drawing.Point(480, 329);
            this.lbl_cab.Name = "lbl_cab";
            this.lbl_cab.Size = new System.Drawing.Size(87, 18);
            this.lbl_cab.TabIndex = 22;
            this.lbl_cab.Text = "Кабинет №";
            // 
            // FormAnsPaper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(75)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(808, 506);
            this.Controls.Add(this.tB_cab);
            this.Controls.Add(this.lbl_cab);
            this.Controls.Add(this.dTP_timeTo);
            this.Controls.Add(this.lbl_timeTo);
            this.Controls.Add(this.dTP_timeFrom);
            this.Controls.Add(this.lbl_timeFrom);
            this.Controls.Add(this.dTP_considTime);
            this.Controls.Add(this.lbl_considTime);
            this.Controls.Add(this.dTP_consid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tB_mesto);
            this.Controls.Add(this.lbl_mesto);
            this.Controls.Add(this.btn_preview);
            this.Controls.Add(this.lbl_header);
            this.Controls.Add(this.dTP_dateOut);
            this.Controls.Add(this.lbl_date);
            this.Controls.Add(this.tB_fio);
            this.Controls.Add(this.lbl_fio);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tB_service);
            this.Controls.Add(this.lbl_service);
            this.Controls.Add(this.tB_operatorTel);
            this.Controls.Add(this.lbl_operatorTel);
            this.Controls.Add(this.tB_operator);
            this.Controls.Add(this.lbl_operator);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Name = "FormAnsPaper";
            this.Text = "FormAnsPaper";
            this.Load += new System.EventHandler(this.FormAnsPaper_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_preview;
        private System.Windows.Forms.Label lbl_header;
        private System.Windows.Forms.DateTimePicker dTP_dateOut;
        private System.Windows.Forms.Label lbl_date;
        private System.Windows.Forms.TextBox tB_fio;
        private System.Windows.Forms.Label lbl_fio;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tB_service;
        private System.Windows.Forms.Label lbl_service;
        private System.Windows.Forms.TextBox tB_operatorTel;
        private System.Windows.Forms.Label lbl_operatorTel;
        private System.Windows.Forms.TextBox tB_operator;
        private System.Windows.Forms.Label lbl_operator;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox tB_mesto;
        private System.Windows.Forms.Label lbl_mesto;
        private System.Windows.Forms.DateTimePicker dTP_consid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dTP_considTime;
        private System.Windows.Forms.Label lbl_considTime;
        private System.Windows.Forms.DateTimePicker dTP_timeFrom;
        private System.Windows.Forms.Label lbl_timeFrom;
        private System.Windows.Forms.DateTimePicker dTP_timeTo;
        private System.Windows.Forms.Label lbl_timeTo;
        private System.Windows.Forms.TextBox tB_cab;
        private System.Windows.Forms.Label lbl_cab;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}