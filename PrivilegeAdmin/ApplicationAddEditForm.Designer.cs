using System.Windows.Forms.VisualStyles;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace PrivilegeAdmin
{
    partial class ApplicationAddEditForm
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
            labelOrderId = new Label();
            textBoxOrderId = new TextBox();
            labelFio = new Label();
            textBoxFio = new TextBox();
            labelStatus = new Label();
            comboBoxStatus = new ComboBox();
            buttonSave = new Button();
            txtFilePath = new TextBox();
            btnBrowse = new Button();
            SuspendLayout();
            // 
            // labelOrderId
            // 
            labelOrderId.Location = new Point(30, 30);
            labelOrderId.Name = "labelOrderId";
            labelOrderId.Size = new Size(100, 23);
            labelOrderId.TabIndex = 0;
            labelOrderId.Text = "Номер заказа:";
            labelOrderId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxOrderId
            // 
            textBoxOrderId.Location = new Point(150, 30);
            textBoxOrderId.Name = "textBoxOrderId";
            textBoxOrderId.Size = new Size(200, 23);
            textBoxOrderId.TabIndex = 1;
            // 
            // labelFio
            // 
            labelFio.Location = new Point(30, 70);
            labelFio.Name = "labelFio";
            labelFio.Size = new Size(100, 23);
            labelFio.TabIndex = 2;
            labelFio.Text = "ФИО заказчика:";
            labelFio.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxFio
            // 
            textBoxFio.Location = new Point(150, 70);
            textBoxFio.Name = "textBoxFio";
            textBoxFio.Size = new Size(300, 23);
            textBoxFio.TabIndex = 3;
            // 
            // labelStatus
            // 
            labelStatus.Location = new Point(30, 110);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(100, 23);
            labelStatus.TabIndex = 6;
            labelStatus.Text = "Статус:";
            labelStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxStatus
            // 
            comboBoxStatus.Location = new Point(150, 110);
            comboBoxStatus.Name = "comboBoxStatus";
            comboBoxStatus.Size = new Size(200, 23);
            comboBoxStatus.TabIndex = 7;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(150, 148);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(100, 30);
            buttonSave.TabIndex = 8;
            buttonSave.Text = "Создать";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(12, 30);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(318, 23);
            txtFilePath.TabIndex = 9;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(336, 30);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(114, 23);
            btnBrowse.TabIndex = 10;
            btnBrowse.Text = "Выбрать файл";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // ApplicationAddEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(469, 190);
            Controls.Add(btnBrowse);
            Controls.Add(txtFilePath);
            Controls.Add(labelOrderId);
            Controls.Add(textBoxOrderId);
            Controls.Add(labelFio);
            Controls.Add(textBoxFio);
            Controls.Add(labelStatus);
            Controls.Add(comboBoxStatus);
            Controls.Add(buttonSave);
            Name = "ApplicationAddEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Создание заказа";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.TextBox textBoxOrderId;
        private System.Windows.Forms.TextBox textBoxFio;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelOrderId;
        private System.Windows.Forms.Label labelFio;

        #endregion

        private TextBox txtFilePath;
        private Button btnBrowse;
    }
}