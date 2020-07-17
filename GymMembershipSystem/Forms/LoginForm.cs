using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using GymMembershipSystem.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GymMembershipSystem
{
    public partial class LoginForm : Form
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
                int nLeftRect,     // x-coordinate of upper-left corner
                int nTopRect,      // y-coordinate of upper-left corner
                int nRightRect,    // x-coordinate of lower-right corner
                int nBottomRect,   // y-coordinate of lower-right corner
                int nWidthEllipse, // width of ellipse
                int nHeightEllipse // height of ellipse
            );

        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string tmp = "";
            string authLevel = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(
                global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT password, authLevel FROM Admin WHERE username=@user", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@user", userBox.Text);

                        connection.Open();

                        SqlDataReader o = cmd.ExecuteReader();
                        if (o != null)
                        {
                            while (o.Read())
                            {
                                tmp = o["password"].ToString();
                                authLevel = o["authLevel"].ToString();
                            }
                        }
                        connection.Close();
                    }
                }

                if (passBox.Text != tmp || tmp == "")
                {
                     List<Label> Labels = new List<Label>();
                     Labels.Add(MyLabel.SetOKLabel("Login Failed!"));
                     Labels.Add(MyLabel.SetOKLabel("Incorrect Username/Password"));

                     List<Button> Buttons = new List<Button>();
                     Buttons.Add(MyButton.SetOKThemeButton());
                     MyMessageBox.Show(
                         Labels,
                         "",
                         Buttons,
                         MyImage.SetFailed());
                     userBox.Text = "";
                     passBox.Text = "";
                }
                else
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("Successfull Login!"));
                    Labels.Add(MyLabel.SetOKLabel("Welcome Back " + userBox.Text + "!"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetSuccess());

                    HomeForm h = new HomeForm(authLevel, userBox.Text);
                    h.MaximizeBox = false;
                    h.MinimizeBox = false;
                    this.Hide();
                    h.ShowDialog();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel(ex.Message));

                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
            }
        }

        private void passBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }
    }
}
