using GymMembershipSystem.Forms;
using Method;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace GymMembershipSystem
{
    public partial class HomeForm : Form
    {
        public SerialPort sport = new SerialPort();
        string tmp = "";
        Methods method = new Methods();
        string currUsername = "";

        public HomeForm(string authLevel, string username)
        {
            InitializeComponent();
            currUsername = username;
            string AuthLevel = authLevel;
            label6.Text = "Welcome back " + username + ". Your Authorization Level is " + authLevel + ".";
            if(AuthLevel == "1")
            {
                measurementsButton.Visible = false;
                shopButton.Visible = false;
                exercisesButton.Visible = false;
            }
            try
            {
                tmp = method.getComm();
                sport = new System.IO.Ports.SerialPort("COM" + tmp, 9600, Parity.None, 8, StopBits.One);
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

        private void HomeForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            try
            {
                using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    bool tmp = false;
                    using (SqlCommand cmd = new SqlCommand("SELECT PATH FROM Settings WHERE Id=1", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                       
                        connection.Open();

                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            if (o.ToString() == "")
                                tmp = true;
                            else
                            tmp = false;
                        }
                        else
                            tmp = true;

                        cmd.Connection.Close();
                    }
                    if(tmp)
                    using (SqlCommand command = new SqlCommand("UPDATE Settings SET PATH=@p WHERE Id=1 ", connection))
                    {


                        string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

                        command.Parameters.AddWithValue("@p", wanted_path );
                        command.Connection.Open();
                        command.ExecuteNonQuery();            
                    
                        command.Connection.Close();

                    }

                }

                sport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
               
                sport.Open();
                
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

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string indata = sport.ReadLine();
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT id FROM Member WHERE CardId=@CardId", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardId", indata);
                        connection.Open();
                           
                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            sport.Close();
                            InformationForm ev = new InformationForm(indata);
                            ev.ShowDialog();
                            ev.Focus();
                            sport.Open();
                        }
                        connection.Close();
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reportFormButtonClick(object sender, EventArgs e)
        {
            try
            {
                sport.Close();
                InformationForm ev = new InformationForm("0");
                ev.ShowDialog();
                sport.Open();
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

        private void addMemberFormButtonClick(object sender, EventArgs e)
        {
            try
            {
                sport.Close();
                AddMemberForm f = new AddMemberForm();
                f.ShowDialog();
                sport.Open();
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

        private void searchMembersButtonClick(object sender, EventArgs e)
        {
            try
            {
                sport.Close();
                SearchMembersForm p = new SearchMembersForm();
                p.ShowDialog();
                sport.Open();

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

        private void evidenceReportButtonClick(object sender, EventArgs e)
        {
            try
            {
                sport.Close();

                AccountForm i = new AccountForm();
                i.ShowDialog();
                sport.Open();

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

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                sport.Close();

                SettingsForm p = new SettingsForm(currUsername);
                p.ShowDialog();
                using (SqlConnection connection = new SqlConnection(
                   global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COMM FROM Settings WHERE Id=1", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            tmp = o.ToString();

                        }
                        connection.Close();
                    }
                }
                sport = new System.IO.Ports.SerialPort("COM" + tmp, 9600, Parity.None, 8, StopBits.One);
                sport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                sport.Open();
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

        private void button7_Click(object sender, EventArgs e)
        {
            string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

            System.Diagnostics.Process.Start(@wanted_path + "\\Mehmed_Duhovic_Thesis.pdf");
        }

        private void measurementsButtonClick(object sender, EventArgs e)
        {
            try
            {
                sport.Close();

                MeasurementsForm i = new MeasurementsForm();
                i.ShowDialog();
                sport.Open();

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.label3.Text = dateTime.ToString();
        }

        private void exercisesButton_Click(object sender, EventArgs e)
        {
            try
            {
                sport.Close();

                ExerciseListForm formDialog = new ExerciseListForm();
                formDialog.ShowDialog();
                sport.Open();

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

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                sport.Close();

                ShopForm formDialog = new ShopForm();
                formDialog.ShowDialog();
                sport.Open();

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
    }
}
