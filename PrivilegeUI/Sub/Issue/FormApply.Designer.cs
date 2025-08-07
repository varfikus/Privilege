namespace PrivilegeUI.Sub.Issue
{
    partial class FormApply
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
            this.tB_denial = new System.Windows.Forms.TextBox();
            this.lbl_denial = new System.Windows.Forms.Label();
            this.btn_preview = new System.Windows.Forms.Button();
            this.lbl_header = new System.Windows.Forms.Label();
            this.dTP_consid = new System.Windows.Forms.DateTimePicker();
            this.lbl_consid = new System.Windows.Forms.Label();
            this.dTP_dateOut = new System.Windows.Forms.DateTimePicker();
            this.lbl_dateOut = new System.Windows.Forms.Label();
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tB_denial
            // 
            this.tB_denial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB_denial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(36)))), ((int)(((byte)(63)))));
            this.tB_denial.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_denial.ForeColor = System.Drawing.Color.Gainsboro;
            this.tB_denial.Location = new System.Drawing.Point(142, 289);
            this.tB_denial.Name = "tB_denial";
            this.tB_denial.Size = new System.Drawing.Size(762, 26);
            this.tB_denial.TabIndex = 47;
            this.tB_denial.Visible = false;
            // 
            // lbl_denial
            // 
            this.lbl_denial.AutoSize = true;
            this.lbl_denial.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_denial.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_denial.Location = new System.Drawing.Point(12, 292);
            this.lbl_denial.Name = "lbl_denial";
            this.lbl_denial.Size = new System.Drawing.Size(121, 18);
            this.lbl_denial.TabIndex = 46;
            this.lbl_denial.Text = "Причина отказа";
            this.lbl_denial.Visible = false;
            // 
            // btn_preview
            // 
            this.btn_preview.BackgroundImage = global::PrivilegeUI.Properties.Resources.view_white_96;
            this.btn_preview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_preview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_preview.FlatAppearance.BorderSize = 0;
            this.btn_preview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_preview.Location = new System.Drawing.Point(124, 13);
            this.btn_preview.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_preview.Name = "btn_preview";
            this.btn_preview.Size = new System.Drawing.Size(50, 50);
            this.btn_preview.TabIndex = 30;
            this.btn_preview.UseVisualStyleBackColor = true;
            this.btn_preview.Click += new System.EventHandler(this.btn_preview_Click);
            // 
            // lbl_header
            // 
            this.lbl_header.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_header.Font = new System.Drawing.Font("Arial Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_header.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_header.Location = new System.Drawing.Point(180, 20);
            this.lbl_header.Name = "lbl_header";
            this.lbl_header.Size = new System.Drawing.Size(724, 31);
            this.lbl_header.TabIndex = 31;
            this.lbl_header.Text = "Принятие документа";
            this.lbl_header.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dTP_consid
            // 
            this.dTP_consid.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dTP_consid.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_consid.Location = new System.Drawing.Point(704, 248);
            this.dTP_consid.Name = "dTP_consid";
            this.dTP_consid.Size = new System.Drawing.Size(200, 26);
            this.dTP_consid.TabIndex = 45;
            // 
            // lbl_consid
            // 
            this.lbl_consid.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbl_consid.AutoSize = true;
            this.lbl_consid.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_consid.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_consid.Location = new System.Drawing.Point(355, 251);
            this.lbl_consid.Name = "lbl_consid";
            this.lbl_consid.Size = new System.Drawing.Size(343, 18);
            this.lbl_consid.TabIndex = 44;
            this.lbl_consid.Text = "Максимальный срок рассмотрения заявления";
            // 
            // dTP_dateOut
            // 
            this.dTP_dateOut.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTP_dateOut.Location = new System.Drawing.Point(142, 248);
            this.dTP_dateOut.Name = "dTP_dateOut";
            this.dTP_dateOut.Size = new System.Drawing.Size(200, 26);
            this.dTP_dateOut.TabIndex = 43;
            // 
            // lbl_dateOut
            // 
            this.lbl_dateOut.AutoSize = true;
            this.lbl_dateOut.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_dateOut.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_dateOut.Location = new System.Drawing.Point(12, 251);
            this.lbl_dateOut.Name = "lbl_dateOut";
            this.lbl_dateOut.Size = new System.Drawing.Size(124, 18);
            this.lbl_dateOut.TabIndex = 42;
            this.lbl_dateOut.Text = "Дата документа";
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
            this.tB_fio.Size = new System.Drawing.Size(762, 26);
            this.tB_fio.TabIndex = 41;
            // 
            // lbl_fio
            // 
            this.lbl_fio.AutoSize = true;
            this.lbl_fio.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_fio.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_fio.Location = new System.Drawing.Point(12, 211);
            this.lbl_fio.Name = "lbl_fio";
            this.lbl_fio.Size = new System.Drawing.Size(121, 18);
            this.lbl_fio.TabIndex = 40;
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
            this.panel2.Size = new System.Drawing.Size(892, 2);
            this.panel2.TabIndex = 39;
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
            this.tB_service.Size = new System.Drawing.Size(722, 19);
            this.tB_service.TabIndex = 38;
            // 
            // lbl_service
            // 
            this.lbl_service.AutoSize = true;
            this.lbl_service.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_service.ForeColor = System.Drawing.Color.LightGray;
            this.lbl_service.Location = new System.Drawing.Point(12, 157);
            this.lbl_service.Name = "lbl_service";
            this.lbl_service.Size = new System.Drawing.Size(164, 18);
            this.lbl_service.TabIndex = 37;
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
            this.tB_operatorTel.Size = new System.Drawing.Size(722, 19);
            this.tB_operatorTel.TabIndex = 36;
            // 
            // lbl_operatorTel
            // 
            this.lbl_operatorTel.AutoSize = true;
            this.lbl_operatorTel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_operatorTel.ForeColor = System.Drawing.Color.LightGray;
            this.lbl_operatorTel.Location = new System.Drawing.Point(12, 122);
            this.lbl_operatorTel.Name = "lbl_operatorTel";
            this.lbl_operatorTel.Size = new System.Drawing.Size(164, 18);
            this.lbl_operatorTel.TabIndex = 35;
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
            this.tB_operator.Size = new System.Drawing.Size(785, 19);
            this.tB_operator.TabIndex = 34;
            // 
            // lbl_operator
            // 
            this.lbl_operator.AutoSize = true;
            this.lbl_operator.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_operator.ForeColor = System.Drawing.Color.LightGray;
            this.lbl_operator.Location = new System.Drawing.Point(12, 88);
            this.lbl_operator.Name = "lbl_operator";
            this.lbl_operator.Size = new System.Drawing.Size(101, 18);
            this.lbl_operator.TabIndex = 33;
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
            this.panel1.Size = new System.Drawing.Size(892, 2);
            this.panel1.TabIndex = 32;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::PrivilegeUI.Properties.Resources.go_back_white_96;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(12, 13);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 50);
            this.btnClose.TabIndex = 28;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImage = global::PrivilegeUI.Properties.Resources.ok_white_96;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(68, 13);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(50, 50);
            this.btnOk.TabIndex = 29;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormApply
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(75)))), ((int)(((byte)(119)))));
            this.ClientSize = new System.Drawing.Size(916, 447);
            this.Controls.Add(this.tB_denial);
            this.Controls.Add(this.lbl_denial);
            this.Controls.Add(this.btn_preview);
            this.Controls.Add(this.lbl_header);
            this.Controls.Add(this.dTP_consid);
            this.Controls.Add(this.lbl_consid);
            this.Controls.Add(this.dTP_dateOut);
            this.Controls.Add(this.lbl_dateOut);
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
            this.Name = "FormApply";
            this.Text = "FormApply";
            this.Load += new System.EventHandler(this.FormApply_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tB_denial;
        private System.Windows.Forms.Label lbl_denial;
        private System.Windows.Forms.Button btn_preview;
        private System.Windows.Forms.Label lbl_header;
        private System.Windows.Forms.DateTimePicker dTP_consid;
        private System.Windows.Forms.Label lbl_consid;
        private System.Windows.Forms.DateTimePicker dTP_dateOut;
        private System.Windows.Forms.Label lbl_dateOut;
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
    }
}