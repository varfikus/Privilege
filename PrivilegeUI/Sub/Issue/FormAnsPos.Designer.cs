namespace PrivilegeUI.Sub.Issue
{
    partial class FormAnsPos
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
            components = new System.ComponentModel.Container();
            lbl_header = new Label();
            tB_fio = new TextBox();
            lbl_fio = new Label();
            panel2 = new Panel();
            tB_service = new TextBox();
            lbl_service = new Label();
            tB_operatorTel = new TextBox();
            lbl_operatorTel = new Label();
            tB_operator = new TextBox();
            lbl_operator = new Label();
            panel1 = new Panel();
            btn_preview = new Button();
            btnClose = new Button();
            btnOk = new Button();
            toolTip1 = new ToolTip(components);
            SuspendLayout();
            // 
            // lbl_header
            // 
            lbl_header.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lbl_header.Font = new Font("Arial Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lbl_header.ForeColor = Color.Gainsboro;
            lbl_header.Location = new Point(210, 23);
            lbl_header.Margin = new Padding(4, 0, 4, 0);
            lbl_header.Name = "lbl_header";
            lbl_header.Size = new Size(719, 36);
            lbl_header.TabIndex = 3;
            lbl_header.Text = "Выдача документа";
            lbl_header.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tB_fio
            // 
            tB_fio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_fio.BackColor = Color.FromArgb(0, 36, 63);
            tB_fio.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_fio.ForeColor = Color.Gainsboro;
            tB_fio.Location = new Point(166, 240);
            tB_fio.Margin = new Padding(4, 3, 4, 3);
            tB_fio.Name = "tB_fio";
            tB_fio.Size = new Size(762, 26);
            tB_fio.TabIndex = 13;
            // 
            // lbl_fio
            // 
            lbl_fio.AutoSize = true;
            lbl_fio.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_fio.ForeColor = Color.WhiteSmoke;
            lbl_fio.Location = new Point(14, 243);
            lbl_fio.Margin = new Padding(4, 0, 4, 0);
            lbl_fio.Name = "lbl_fio";
            lbl_fio.Size = new Size(121, 18);
            lbl_fio.TabIndex = 12;
            lbl_fio.Text = "ФИО заявителя";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.Gainsboro;
            panel2.ForeColor = SystemColors.ControlText;
            panel2.Location = new Point(14, 222);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(915, 2);
            panel2.TabIndex = 11;
            // 
            // tB_service
            // 
            tB_service.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_service.BackColor = Color.FromArgb(0, 36, 63);
            tB_service.BorderStyle = BorderStyle.None;
            tB_service.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_service.ForeColor = Color.Gainsboro;
            tB_service.Location = new Point(212, 181);
            tB_service.Margin = new Padding(4, 3, 4, 3);
            tB_service.Name = "tB_service";
            tB_service.ReadOnly = true;
            tB_service.Size = new Size(716, 19);
            tB_service.TabIndex = 10;
            // 
            // lbl_service
            // 
            lbl_service.AutoSize = true;
            lbl_service.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_service.ForeColor = Color.LightGray;
            lbl_service.Location = new Point(14, 181);
            lbl_service.Margin = new Padding(4, 0, 4, 0);
            lbl_service.Name = "lbl_service";
            lbl_service.Size = new Size(164, 18);
            lbl_service.TabIndex = 9;
            lbl_service.Text = "Наименование услуги";
            // 
            // tB_operatorTel
            // 
            tB_operatorTel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_operatorTel.BackColor = Color.FromArgb(0, 36, 63);
            tB_operatorTel.BorderStyle = BorderStyle.None;
            tB_operatorTel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_operatorTel.ForeColor = Color.Gainsboro;
            tB_operatorTel.Location = new Point(212, 141);
            tB_operatorTel.Margin = new Padding(4, 3, 4, 3);
            tB_operatorTel.Name = "tB_operatorTel";
            tB_operatorTel.ReadOnly = true;
            tB_operatorTel.Size = new Size(716, 19);
            tB_operatorTel.TabIndex = 8;
            // 
            // lbl_operatorTel
            // 
            lbl_operatorTel.AutoSize = true;
            lbl_operatorTel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_operatorTel.ForeColor = Color.LightGray;
            lbl_operatorTel.Location = new Point(14, 141);
            lbl_operatorTel.Margin = new Padding(4, 0, 4, 0);
            lbl_operatorTel.Name = "lbl_operatorTel";
            lbl_operatorTel.Size = new Size(164, 18);
            lbl_operatorTel.TabIndex = 7;
            lbl_operatorTel.Text = "Телефон исполнителя";
            // 
            // tB_operator
            // 
            tB_operator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tB_operator.BackColor = Color.FromArgb(0, 36, 63);
            tB_operator.BorderStyle = BorderStyle.None;
            tB_operator.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tB_operator.ForeColor = Color.Gainsboro;
            tB_operator.Location = new Point(139, 102);
            tB_operator.Margin = new Padding(4, 3, 4, 3);
            tB_operator.Name = "tB_operator";
            tB_operator.ReadOnly = true;
            tB_operator.Size = new Size(790, 19);
            tB_operator.TabIndex = 6;
            // 
            // lbl_operator
            // 
            lbl_operator.AutoSize = true;
            lbl_operator.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lbl_operator.ForeColor = Color.LightGray;
            lbl_operator.Location = new Point(14, 102);
            lbl_operator.Margin = new Padding(4, 0, 4, 0);
            lbl_operator.Name = "lbl_operator";
            lbl_operator.Size = new Size(101, 18);
            lbl_operator.TabIndex = 5;
            lbl_operator.Text = "Исполнитель";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Gainsboro;
            panel1.ForeColor = SystemColors.ControlText;
            panel1.Location = new Point(14, 82);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(915, 2);
            panel1.TabIndex = 4;
            // 
            // btn_preview
            // 
            btn_preview.BackgroundImage = Properties.Resources.view_white_96;
            btn_preview.BackgroundImageLayout = ImageLayout.Zoom;
            btn_preview.Cursor = Cursors.Hand;
            btn_preview.FlatAppearance.BorderSize = 0;
            btn_preview.FlatStyle = FlatStyle.Flat;
            btn_preview.Location = new Point(145, 15);
            btn_preview.Margin = new Padding(4, 5, 4, 5);
            btn_preview.Name = "btn_preview";
            btn_preview.Size = new Size(58, 58);
            btn_preview.TabIndex = 2;
            btn_preview.UseVisualStyleBackColor = true;
            btn_preview.Click += btn_preview_Click;
            // 
            // btnClose
            // 
            btnClose.BackgroundImage = Properties.Resources.go_back_white_96;
            btnClose.BackgroundImageLayout = ImageLayout.Zoom;
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Location = new Point(14, 15);
            btnClose.Margin = new Padding(4, 5, 4, 5);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(58, 58);
            btnClose.TabIndex = 0;
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
            btnOk.Location = new Point(79, 15);
            btnOk.Margin = new Padding(4, 5, 4, 5);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(58, 58);
            btnOk.TabIndex = 1;
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // FormAnsPos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(38, 75, 119);
            ClientSize = new Size(943, 747);
            Controls.Add(btn_preview);
            Controls.Add(lbl_header);
            Controls.Add(tB_fio);
            Controls.Add(lbl_fio);
            Controls.Add(panel2);
            Controls.Add(tB_service);
            Controls.Add(lbl_service);
            Controls.Add(tB_operatorTel);
            Controls.Add(lbl_operatorTel);
            Controls.Add(tB_operator);
            Controls.Add(lbl_operator);
            Controls.Add(panel1);
            Controls.Add(btnClose);
            Controls.Add(btnOk);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(959, 786);
            Name = "FormAnsPos";
            Text = "FormAnsPos";
            Load += FormAnsPos_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_preview;
        private System.Windows.Forms.Label lbl_header;
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
        private System.Windows.Forms.ToolTip toolTip1;
    }
}