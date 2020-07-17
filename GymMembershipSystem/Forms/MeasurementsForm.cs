using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymMembershipSystem.Forms
{
    public partial class MeasurementsForm : Form
    {
        public MeasurementsForm()
        {
            InitializeComponent();
        }

        private void MeasurementsForm_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            try
            {
                DataSet userDataSet = new DataSet();
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT id, (Surname + ', ' + Name) AS Name FROM Member ORDER BY Surname ASC", connection))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd2;
                        da.Fill(userDataSet);
                        comboBox1.DisplayMember = "Name";
                        comboBox1.ValueMember = "id";
                        comboBox1.DataSource = userDataSet.Tables[0];
                        comboBox1.SelectedIndex = -1;
                        cmd2.Connection.Close();
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

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT TOP 1 * FROM Measurement WHERE MemberId='"+ comboBox1.SelectedValue +"' ORDER BY Timestamp DESC", connection))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();

                        if(dr.HasRows == false)
                        {
                            button2.Enabled = false;
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            textBox6.Clear();
                            textBox5.Clear();
                            textBox7.Clear();
                            textBox9.Clear();
                            textBox8.Clear();
                            textBox11.Clear();
                            textBox10.Clear();
                            textBox13.Clear();
                            textBox12.Clear();
                            textBox15.Clear();
                            textBox14.Clear();
                            textBox16.Clear();
                        }

                        while (dr.Read())
                        {
                            button2.Enabled = true;
                            string Height = dr["Height"].ToString();
                            string Weight = dr["Weight"].ToString();
                            string BodyFat = dr["BodyFat"].ToString();
                            string Neck = dr["Neck"].ToString();
                            string Shoulders = dr["Shoulders"].ToString();
                            string LeftBicep = dr["LeftBicep"].ToString();
                            string LeftForearm = dr["LeftForearm"].ToString();
                            string RightBicep = dr["RightBicep"].ToString();
                            string RightForearm = dr["RightForearm"].ToString();
                            string Chest = dr["Chest"].ToString();
                            string Waist = dr["Waist"].ToString();
                            string Hips = dr["Hips"].ToString();
                            string LeftThighs = dr["LeftThighs"].ToString();
                            string RightThighs = dr["RightThighs"].ToString();
                            string LeftCalves = dr["LeftCalves"].ToString();
                            string RightCalves = dr["RightCalves"].ToString();

                            textBox1.Text = Height;
                            textBox2.Text = Weight;
                            textBox3.Text = Neck;
                            textBox4.Text = BodyFat;
                            textBox6.Text = Shoulders;
                            textBox5.Text = LeftBicep;
                            textBox7.Text = LeftForearm;
                            textBox9.Text = RightBicep;
                            textBox8.Text = RightForearm;
                            textBox11.Text = Chest;
                            textBox10.Text = Waist;
                            textBox13.Text = Hips;
                            textBox12.Text = LeftThighs;
                            textBox15.Text = RightThighs;
                            textBox14.Text = LeftCalves;
                            textBox16.Text = RightCalves;
                        }
                        cmd2.Connection.Close();
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
        private void button1_Click(object sender, EventArgs e)
        {
            string Height = textBox1.Text;
            string Weight = textBox2.Text;
            string Neck = textBox3.Text;
            string BodyFat = textBox4.Text;
            string Shoulders = textBox6.Text;
            string LeftBicep = textBox5.Text;
            string LeftForearm = textBox7.Text;
            string RightBicep = textBox9.Text;
            string RightForearm  = textBox8.Text;
            string Chest = textBox11.Text;
            string Waist = textBox10.Text;
            string Hips = textBox13.Text;
            string LeftThighs = textBox12.Text;
            string RightThighs = textBox15.Text;
            string LeftCalves = textBox14.Text;
            string RightCalves = textBox16.Text;

            try
            {
                using (SqlConnection connection2 = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Measurement(Height, Weight, Neck, BodyFat, Shoulders," +
                        "LeftBicep, LeftForearm, RightBicep, RightForearm, Chest, Waist, Hips," +
                        "LeftThighs, RightThighs, LeftCalves, RightCalves, MemberId, Timestamp)" +
                        "VALUES(@Height, @Weight, @Neck, @BodyFat, @Shoulders, @LeftBicep, @LeftForearm, @RightBicep, @RightForearm, @Chest, @Waist, @Hips," +
                        "@LeftThighs, @RightThighs, @LeftCalves, @RightCalves, @UserId, @Timestamp)", connection2))
                    {
                        cmd.Parameters.AddWithValue("@Height", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Weight", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Neck", textBox3.Text);
                        cmd.Parameters.AddWithValue("@BodyFat", textBox4.Text);
                        cmd.Parameters.AddWithValue("@Shoulders", textBox6.Text);
                        cmd.Parameters.AddWithValue("@LeftBicep", textBox5.Text);
                        cmd.Parameters.AddWithValue("@LeftForearm", textBox7.Text);
                        cmd.Parameters.AddWithValue("@RightBicep", textBox9.Text);
                        cmd.Parameters.AddWithValue("@RightForearm", textBox8.Text);
                        cmd.Parameters.AddWithValue("@Chest", textBox11.Text);
                        cmd.Parameters.AddWithValue("@Waist", textBox10.Text);
                        cmd.Parameters.AddWithValue("@Hips", textBox13.Text);
                        cmd.Parameters.AddWithValue("@LeftThighs", textBox12.Text);
                        cmd.Parameters.AddWithValue("@RightThighs", textBox15.Text);
                        cmd.Parameters.AddWithValue("@LeftCalves", textBox14.Text);
                        cmd.Parameters.AddWithValue("@RightCalves", textBox16.Text);
                        cmd.Parameters.AddWithValue("@UserId", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                        cmd.Connection.Open();

                        if (cmd.ExecuteNonQuery().ToString() == "1")
                        {
                            List<Label> Labels = new List<Label>();
                            Labels.Add(MyLabel.SetOKLabel("Measurement Update"));
                            Labels.Add(MyLabel.SetOKLabel("Measurement update passed"));

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
                            Labels.Add(MyLabel.SetOKLabel("Measurement Update"));
                            Labels.Add(MyLabel.SetOKLabel("Measurement update failed"));

                            List<Button> Buttons = new List<Button>();
                            Buttons.Add(MyButton.SetOKThemeButton());
                            MyMessageBox.Show(
                                Labels,
                                "",
                                Buttons,
                                MyImage.SetFailed());
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void measurementHistoryClicked(object sender, EventArgs e)
        {

            try
            {
                MeasurementHistoryForm f = new MeasurementHistoryForm(comboBox1.SelectedValue.ToString());
                f.ShowDialog();
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
