namespace GymMembershipSystem
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.userLabel = new System.Windows.Forms.Label();
            this.passLabel = new System.Windows.Forms.Label();
            this.passBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.icon1 = new System.Windows.Forms.PictureBox();
            this.userBox = new System.Windows.Forms.TextBox();
            this.name1 = new System.Windows.Forms.Label();
            this.name2 = new System.Windows.Forms.Label();
            this.logopanel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
            this.logopanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.userLabel.ForeColor = System.Drawing.Color.White;
            this.userLabel.Location = new System.Drawing.Point(186, 42);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(41, 18);
            this.userLabel.TabIndex = 0;
            this.userLabel.Text = "User:";
            // 
            // passLabel
            // 
            this.passLabel.AutoSize = true;
            this.passLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.passLabel.ForeColor = System.Drawing.Color.White;
            this.passLabel.Location = new System.Drawing.Point(156, 81);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(71, 18);
            this.passLabel.TabIndex = 1;
            this.passLabel.Text = "Password:";
            // 
            // passBox
            // 
            this.passBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.passBox.Location = new System.Drawing.Point(233, 78);
            this.passBox.Name = "passBox";
            this.passBox.Size = new System.Drawing.Size(242, 26);
            this.passBox.TabIndex = 3;
            this.passBox.UseSystemPasswordChar = true;
            this.passBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passBox_KeyDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(65)))), ((int)(((byte)(93)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(374, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 38);
            this.button1.TabIndex = 6;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // icon1
            // 
            this.icon1.Image = global::GymMembershipSystem.Properties.Resources.dumbbell__1_;
            this.icon1.Location = new System.Drawing.Point(20, 5);
            this.icon1.Name = "icon1";
            this.icon1.Size = new System.Drawing.Size(111, 56);
            this.icon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icon1.TabIndex = 7;
            this.icon1.TabStop = false;
            // 
            // userBox
            // 
            this.userBox.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.userBox.Location = new System.Drawing.Point(233, 39);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(242, 26);
            this.userBox.TabIndex = 2;
            // 
            // name1
            // 
            this.name1.AutoSize = true;
            this.name1.Font = new System.Drawing.Font("Ink Free", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name1.ForeColor = System.Drawing.Color.White;
            this.name1.Location = new System.Drawing.Point(32, 79);
            this.name1.Name = "name1";
            this.name1.Size = new System.Drawing.Size(99, 30);
            this.name1.TabIndex = 12;
            this.name1.Text = "Arnold\'s";
            // 
            // name2
            // 
            this.name2.AutoSize = true;
            this.name2.Font = new System.Drawing.Font("Ink Free", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name2.ForeColor = System.Drawing.Color.White;
            this.name2.Location = new System.Drawing.Point(39, 103);
            this.name2.Name = "name2";
            this.name2.Size = new System.Drawing.Size(88, 43);
            this.name2.TabIndex = 13;
            this.name2.Text = "Gym";
            // 
            // logopanel1
            // 
            this.logopanel1.Controls.Add(this.icon1);
            this.logopanel1.Controls.Add(this.name2);
            this.logopanel1.Controls.Add(this.name1);
            this.logopanel1.Location = new System.Drawing.Point(2, 7);
            this.logopanel1.Name = "logopanel1";
            this.logopanel1.Size = new System.Drawing.Size(157, 159);
            this.logopanel1.TabIndex = 14;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(65)))), ((int)(((byte)(93)))));
            this.ClientSize = new System.Drawing.Size(490, 169);
            this.Controls.Add(this.logopanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.passBox);
            this.Controls.Add(this.userBox);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.userLabel);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            ((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
            this.logopanel1.ResumeLayout(false);
            this.logopanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.Label passLabel;
        private System.Windows.Forms.TextBox passBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox icon1;
        private System.Windows.Forms.TextBox userBox;
        private System.Windows.Forms.Label name1;
        private System.Windows.Forms.Label name2;
        private System.Windows.Forms.Panel logopanel1;
    }
}

