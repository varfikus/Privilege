namespace PrivilegeAdmin
{
    partial class UserForm
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
            lblUsername = new Label();
            lblFullName = new Label();
            lblPassword = new Label();
            lblRole = new Label();
            txtUsername = new TextBox();
            txtFullName = new TextBox();
            txtPassword = new TextBox();
            cmbRoles = new ComboBox();
            btnSave = new Button();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(27, 25);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(44, 15);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Логин:";
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(27, 65);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(37, 15);
            lblFullName.TabIndex = 2;
            lblFullName.Text = "ФИО:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(27, 105);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(52, 15);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Пароль:";
            // 
            // lblRole
            // 
            lblRole.Location = new Point(27, 145);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(51, 15);
            lblRole.TabIndex = 0;
            lblRole.Text = "Роль:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(120, 22);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(200, 23);
            txtUsername.TabIndex = 1;
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(120, 62);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(200, 23);
            txtFullName.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(120, 102);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(200, 23);
            txtPassword.TabIndex = 3;
            // 
            // cmbRoles
            // 
            cmbRoles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoles.Location = new Point(120, 142);
            cmbRoles.Name = "cmbRoles";
            cmbRoles.Size = new Size(200, 23);
            cmbRoles.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(120, 190);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 6;
            btnSave.Text = "Сохранить";
            btnSave.Click += btnSave_Click;
            // 
            // UserForm
            // 
            ClientSize = new Size(360, 250);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblFullName);
            Controls.Add(txtFullName);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblRole);
            Controls.Add(cmbRoles);
            Controls.Add(btnSave);
            Name = "UserForm";
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox txtUsername;
        private TextBox txtFullName;
        private TextBox txtPassword;
        private ComboBox cmbRoles;
        private Button btnSave;
        #endregion

        private Label lblUsername;
        private Label lblFullName;
        private Label lblPassword;
        private Label lblRole;
    }
}