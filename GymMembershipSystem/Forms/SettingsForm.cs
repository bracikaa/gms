using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GymMembershipSystem
{
    public partial class SettingsForm : Form
    {
        string currUsername = "";
        private string password;
        string authLevel;
        int visibilitylevel = 0;
        public SettingsForm(string username)
        {
            InitializeComponent();
            currUsername = username;
            if (visibilitylevel == 0)
            {
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label12.Visible = false;
                label9.Visible = false;
                label2.Visible = false;
                formbox1.Visible = false;
                formbox2.Visible = false;
                formbox3.Visible = false;
            }
        }

        private void imageButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            label8.Text = fbd.SelectedPath;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT COMM, PATH FROM Settings", connection))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            string com = dr["COMM"].ToString();
                            string path = dr["PATH"].ToString();
                            label8.Text = path;
                            int x = Convert.ToInt32(com);
                            comboBox1.SelectedIndex = x - 1;
                        }
                        cmd2.Connection.Close();
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT username, password, authLevel FROM Admin WHERE username='" + currUsername + "'", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection.Open();

                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            string user = dr["username"].ToString();
                            password = dr["password"].ToString();
                            authLevel = dr["authLevel"].ToString();
                            userLabel.Text = user;
                            passwordLabel.Text = new string('*', password.Length);
                        }
                        cmd.Connection.Close();
                    }
                    connection.Close();
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (label8.Text == "")
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel("Please enter location: "));

                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
            }
            else
                try
                {
                    using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE Settings SET COMM=@c,PATH=@p WHERE Id=1 ", connection))
                        {
                            int x = comboBox1.SelectedIndex + 1;
                            command.Parameters.AddWithValue("@c", x);
                            command.Parameters.AddWithValue("@p", label8.Text);
                            command.Connection.Open();

                            if (command.ExecuteNonQuery().ToString() == "1")
                            {
                                List<Label> Labels = new List<Label>();
                                Labels.Add(MyLabel.SetOKLabel("Settings Changed Successfully"));
                                Labels.Add(MyLabel.SetOKLabel("Settings changed"));

                                List<Button> Buttons = new List<Button>();
                                Buttons.Add(MyButton.SetOKThemeButton());
                                MyMessageBox.Show(
                                    Labels,
                                    "",
                                    Buttons,
                                    MyImage.SetSuccess());
                            }
                            else
                            {
                                List<Label> Labels = new List<Label>();
                                Labels.Add(MyLabel.SetOKLabel("Settings change Failed"));
                                Labels.Add(MyLabel.SetOKLabel("Settings not changed"));

                                List<Button> Buttons = new List<Button>();
                                Buttons.Add(MyButton.SetOKThemeButton());
                                MyMessageBox.Show(
                                    Labels,
                                    "",
                                    Buttons,
                                    MyImage.SetSuccess());

                                command.Connection.Close();
                                this.Close();
                            }
                        }
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (visibilitylevel == 1)
            {
                if (formbox1.Text != password)
                {
                    formbox1.Text = "";
                    formbox2.Text = "";
                    formbox3.Text = "";

                    label1.Text = "Password is incorrect!";
                    errorProvider1.SetError(formbox1, "Password is incorrect");
                }
                else if (formbox2.Text.Length <= 5)
                {
                    formbox2.Text = "";
                    formbox3.Text = "";
                    errorProvider1.SetError(formbox1, "");
                    label1.Text = "Password has to be longer than 5 characters!";
                    errorProvider1.SetError(formbox2, "Password has to be longer than 5 characters!");

                }
                else if (formbox2.Text != formbox3.Text)
                {
                    formbox2.Text = "";
                    formbox3.Text = "";
                    label1.Text = "Passwords aren't equal!";
                    errorProvider1.SetError(formbox2, "Passwords aren't equal!");
                    errorProvider1.SetError(formbox3, "Passwords aren't equal!");
                }
                else
                    try
                    {
                        label1.Text = "";
                        errorProvider1.SetError(formbox2, "");
                        errorProvider1.SetError(formbox3, "");
                        using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                        {

                            using (SqlCommand command = new SqlCommand("UPDATE Admin SET password=@s WHERE username='" + currUsername + "'", connection))
                            {
                                command.Parameters.AddWithValue("@s", formbox2.Text);
                                command.Connection.Open();

                                if (command.ExecuteNonQuery().ToString() == "1")
                                {
                                    List<Label> Labels = new List<Label>();
                                    Labels.Add(MyLabel.SetOKLabel("Password Change"));
                                    Labels.Add(MyLabel.SetOKLabel("Password changed successfully"));

                                    List<Button> Buttons = new List<Button>();
                                    Buttons.Add(MyButton.SetOKThemeButton());
                                    MyMessageBox.Show(
                                        Labels,
                                        "",
                                        Buttons,
                                        MyImage.SetSuccess());
                                }
                                else
                                {
                                    List<Label> Labels = new List<Label>();
                                    Labels.Add(MyLabel.SetOKLabel("Password Change"));
                                    Labels.Add(MyLabel.SetOKLabel("Password Change failed"));

                                    List<Button> Buttons = new List<Button>();
                                    Buttons.Add(MyButton.SetOKThemeButton());
                                    MyMessageBox.Show(
                                        Labels,
                                        "",
                                        Buttons,
                                        MyImage.SetFailed());
                                }

                                formbox1.Text = "";
                                formbox2.Text = "";
                                formbox3.Text = "";

                                command.Connection.Close();

                            }

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
            } else if(visibilitylevel == 2)
            {
                if (authLevel == "2")
                {
                    try
                    {
                        using (SqlConnection connection2 = new SqlConnection(
                          global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                        {

                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Admin(username, password, authLevel)" +
                                "VALUES(@username, @password, @authLevel)", connection2))
                            {
                                cmd.Parameters.AddWithValue("@username", formbox1.Text);
                                cmd.Parameters.AddWithValue("@password", formbox2.Text);
                                cmd.Parameters.AddWithValue("@authLevel", formbox3.Text);

                                cmd.Connection.Open();

                                if (cmd.ExecuteNonQuery().ToString() == "1")
                                {
                                    List<Label> Labels = new List<Label>();
                                    Labels.Add(MyLabel.SetOKLabel("Created new superuser"));
                                    Labels.Add(MyLabel.SetOKLabel("New superuser created successfully"));

                                    List<Button> Buttons = new List<Button>();
                                    Buttons.Add(MyButton.SetOKThemeButton());
                                    MyMessageBox.Show(
                                        Labels,
                                        "",
                                        Buttons,
                                        MyImage.SetSuccess());

                                    
                                formbox1.Text = "";
                                formbox2.Text = "";
                                formbox3.Text = "";
                                }
                            }

                            connection2.Close();
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
                else
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("Unavailable"));
                    Labels.Add(MyLabel.SetOKLabel("You don't have the Auth Level of 1"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetFailed());
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            visibilitylevel = 1;
            label3.Visible = true;
            label12.Visible = false;
            label4.Visible = true;
            label9.Visible = false;
            label5.Visible = true;
            label2.Visible = false;
            formbox1.Visible = true;
            formbox1.UseSystemPasswordChar = true;
            formbox2.Visible = true;
            formbox2.UseSystemPasswordChar = true;
            formbox3.Visible = true;
            formbox3.UseSystemPasswordChar = true;
            label13.Text = "Updating Password";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            visibilitylevel = 2;
            label3.Visible = false;
            label12.Visible = true;
            label4.Visible = false;
            label9.Visible = true;
            label5.Visible = false;
            label2.Visible = true;
            formbox1.Visible = true;
            formbox1.UseSystemPasswordChar = false;
            formbox2.Visible = true;
            formbox2.UseSystemPasswordChar = true;
            formbox3.Visible = true;
            formbox3.UseSystemPasswordChar = false;
            label13.Text = "Adding New Superuser";
        }
    }
}
