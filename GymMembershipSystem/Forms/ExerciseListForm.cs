using CustomModels;
using Method;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace GymMembershipSystem.Forms
{
    public partial class ExerciseListForm : Form
    {
        string path = @"C:\Users\Mehmed\Desktop\GymMembershipSystem\";
        string ext = ".jpg";
        string datareceived = "";
        public SerialPort sport = new SerialPort();
        List<FullExercise> exercises = new List<FullExercise>();
        string tmp = "";
        string imgLocation = "";
        string globalId;
        long theNewId;
        Methods method = new Methods();
        int currExerciseId;
        public ExerciseListForm()
        {
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(
global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
            {
                using (SqlCommand cmd4 = new SqlCommand("SELECT PATH FROM Settings", connection))
                {
                    cmd4.CommandType = CommandType.Text;
                    cmd4.Connection.Open();
                    SqlDataReader dr = cmd4.ExecuteReader();
                    while (dr.Read())
                    {
                        path = dr["PATH"].ToString();

                    }
                    cmd4.Connection.Close();

                    path = @"" + path + "\\";
                }
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

        private void button4_Click(object sender, EventArgs e)
        {
            sport.Close();
            this.Close();
        }

        private void ExerciseListForm_Load(object sender, EventArgs e)
        {
            sport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            sport.Open();
            listView1.View = View.LargeIcon;
            listView1.Columns.Add("", 150);

            getExercises();
        }

        delegate void SetTextCallback(ShopMember shopmember);

        private void SetText(ShopMember shopmember)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (label12.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { shopmember });
            }
            else
            {
                label12.Text = shopmember.id;
                label13.Text = shopmember.Name;
                label15.Text = shopmember.Gender;
                label17.Text = shopmember.LastEntrance;
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                datareceived = sport.ReadLine();
                theNewId = Int64.Parse(datareceived.Trim(), NumberStyles.HexNumber);
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT id, (Name + ' ' + Surname) AS Name, Gender, LastEntrance FROM Member WHERE CardId=@CardId", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardId", theNewId);
                        connection.Open();

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows == true)
                        {
                            ShopMember shopmember = new ShopMember();
                            while (dr.Read())
                            {
                                string id = dr["id"].ToString();
                                string name = dr["Name"].ToString();
                                string gender = dr["Gender"].ToString();
                                string lastentrance = dr["LastEntrance"].ToString();
                                shopmember.id = dr["id"].ToString();
                                shopmember.Name = dr["Name"].ToString();
                                shopmember.Gender = dr["Gender"].ToString();
                                shopmember.LastEntrance = dr["LastEntrance"].ToString();
                                globalId = id;
                                SetText(shopmember);
                                sport.Close();
                            }

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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void getExercises()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("SELECT id, Name, Description, Link, Day, Time, Enrolled, Price" +
                        " FROM Exercise", connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = Convert.ToInt32(dr["id"]);
                            string name = dr["Name"].ToString();
                            string desc = dr["Description"].ToString();
                            string link = dr["Link"].ToString();
                            string day = dr["Day"].ToString();
                            string time = dr["Time"].ToString();
                            int enrolled = Convert.ToInt32(dr["Enrolled"]);
                            int price = Convert.ToInt32(dr["Price"]);


                            string exerciseurl = path.Trim() + "Images\\trainings\\" + id.ToString() + ext;

                            FullExercise exercise = new FullExercise();

                            exercise.id = id;
                            exercise.Name = name;
                            exercise.Description = desc;
                            exercise.Link = link;
                            exercise.Day = day;
                            exercise.Time = time;
                            exercise.Enrolled = enrolled;
                            exercise.Price = price;
                            exercise.ExerciseUrl = exerciseurl;
                            exercises.Add(exercise);
                        }
                        connection.Close();
                    }
                }

                populate();
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

        public void populate()
        {
            listView1.Items.Clear();
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(147, 147);

            try
            {

                listView1.LargeImageList = imgs;

                for (int i = 0; i < exercises.Count; i++)
                {
                    System.Drawing.Image img = new Bitmap(System.Drawing.Image.FromFile(exercises[i].ExerciseUrl));
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        g.DrawRectangle(Pens.Black, 0, 0, img.Width - 2, img.Height - 2);
                    }
                    imgs.Images.Add(img);
                    listView1.Items.Add(exercises[i].id.ToString(), "", i);
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

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            String selected = listView1.SelectedItems[0].SubItems[0].Name;

            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT id, Name, Description, Link, Day, Time, Enrolled, Price" +
                        " FROM Exercise WHERE id = " + selected, connection))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();

                        while (dr.Read())
                        {
                            int id = Convert.ToInt32(dr["id"]);
                            string name = dr["Name"].ToString();
                            string desc = dr["Description"].ToString();
                            string link = dr["Link"].ToString();
                            string day = dr["Day"].ToString();
                            string time = dr["Time"].ToString();
                            int enrolled = Convert.ToInt32(dr["Enrolled"]);
                            int price = Convert.ToInt32(dr["Price"]);
                            getYoutubeVideo(link);

                            currExerciseId = id;

                            label9.Text = name;
                            label5.Text = desc;
                            label3.Text = "Every " + day + " at " + time + ".";
                            label1.Text = enrolled.ToString();
                            label10.Text = price.ToString();
                        }
                        cmd2.Connection.Close();

                        string doesIdExists = method.checkTrainingEnrollment(currExerciseId.ToString());

                        if(doesIdExists == globalId)
                        {
                            button1.Enabled = false;
                            button2.Enabled = true;
                        } else
                        {
                            button1.Enabled = true;
                            button2.Enabled = false;
                        }
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

        private void getYoutubeVideo(string ytlink)
        {
            string link = ytlink;
            string html = "<html><head>";
            html += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html += "<iframe id='video' src= 'https://www.youtube.com/embed/{0}' width='499' height='246' frameborder='0' allowfullscreen></iframe>";
            html += "</body></html>";
            this.webBrowser1.DocumentText = string.Format(html, link.Split('=')[1]);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Body.Style = "overflow:hidden";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string[] name;
            if (label12.Text != "----------" && label9.Text != "----------")
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                    {
                        using (SqlCommand command2 = new SqlCommand("INSERT INTO TrainingEnrollment(MemberId, EnrollmentDate, Name, Surname, ExerciseId, " +
                            "ExerciseName, ExercisePrice) VALUES(@MemberId, @EnrollmentDate, @Name, @Surname, @ExerciseId, @ExerciseName, @ExercisePrice)", connection))
                        {
                            name = label13.Text.Split(null);
                            command2.Parameters.AddWithValue("@MemberId", label12.Text);
                            command2.Parameters.AddWithValue("@EnrollmentDate", DateTime.Now);
                            command2.Parameters.AddWithValue("@Name", name[0]);
                            command2.Parameters.AddWithValue("@Surname", name[1]);
                            command2.Parameters.AddWithValue("@ExerciseId", currExerciseId);
                            command2.Parameters.AddWithValue("@ExerciseName", label9.Text);
                            command2.Parameters.AddWithValue("@ExercisePrice", label10.Text);

                            command2.Connection.Open();
                            command2.ExecuteNonQuery();
                            command2.Connection.Close();
                        }
                        method.UpdateEnrollment(currExerciseId);
                        List<Label> Labels = new List<Label>();
                        Labels.Add(MyLabel.SetOKLabel("Succesfull Enrolled"));
                        Labels.Add(MyLabel.SetOKLabel("Successfully enrolled into " + label9.Text + " workout programme"));
                        sport.Write("#SUCCS: Welcome to"+ label9.Text +"!\n");
                        List<Button> Buttons = new List<Button>();
                        Buttons.Add(MyButton.SetOKThemeButton());
                        MyMessageBox.Show(
                            Labels,
                            "",
                            Buttons,
                            MyImage.SetSuccess());

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
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel("Missing user and/or training programme."));
                sport.Write("#ERROR: Missing user/training programme!\n");
                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string[] name;
            if (label12.Text != "----------" && label9.Text != "----------")
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                    {
                        using (SqlCommand command2 = new SqlCommand("DELETE FROM TrainingEnrollment Where " +
                            "MemberId='" + globalId +"' AND ExerciseId='" + currExerciseId + "'", connection))
                        {
                            
                            command2.Connection.Open();
                            command2.ExecuteNonQuery();
                            command2.Connection.Close();
                        }
                        method.ReduceEnrollment(currExerciseId);
                        List<Label> Labels = new List<Label>();
                        Labels.Add(MyLabel.SetOKLabel("Succesfull Left"));
                        Labels.Add(MyLabel.SetOKLabel("Successfully left the " + label9.Text + " workout programme"));



                        List<Button> Buttons = new List<Button>();
                        Buttons.Add(MyButton.SetOKThemeButton());
                        MyMessageBox.Show(
                            Labels,
                            "",
                            Buttons,
                            MyImage.SetSuccess());

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
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel("Missing user and/or training programme."));
                sport.Write("#ERROR: Missing user/training programme!\n");
                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
