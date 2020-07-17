using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using Method;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace GymMembershipSystem
{
    public partial class AddMemberForm : Form
    {

        public SerialPort sport = new SerialPort();
        public bool image = false;
        Methods methods = new Methods();
        long theNewId;
        string path = @"C:\Users\Mehmed\Desktop\GymMembershipSystem\GymMembershipSystem";
        string ext = ".jpg";

        public AddMemberForm()
        {
            InitializeComponent();
            try
            {
                string tmp = "0";

                tmp = methods.getComm();
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

        private void AddMemberForm_Load(object sender, EventArgs e)
        {
            try
            {
                sport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                sport.Open();
                monthsno.SelectedIndex = 0;
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
                    }
                }
                path = @"" + path + "\\";

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
                theNewId = Int64.Parse(indata.Trim(), NumberStyles.HexNumber);
                this.Invoke((MethodInvoker)delegate
                {
                    cardIdLabel.Text = theNewId.ToString();

                });
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cardIdLabel.Text == "______________________")
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("Card doesn't exist"));
                    Labels.Add(MyLabel.SetOKLabel("Please put your card to the scanner"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetFailed());
                    sport.Write("#ERROR: Card doesn't exist!\n");
                }
                else if (pictureBox1.BackgroundImage == null)
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("Image is empty"));
                    Labels.Add(MyLabel.SetOKLabel("Please upload your image"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetFailed());
                    sport.Write("#ERROR: Image is empty!\n");
                }
                else if (radioButton2.Checked == false && radioButton3.Checked == false)
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("Card type empty"));
                    Labels.Add(MyLabel.SetOKLabel("Please select a card type"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetFailed());
                    sport.Write("#ERROR: Card type is empty!\n");
                }
                else if (radioButton5.Checked == false && radioButton6.Checked == false)
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("Gender is empty"));
                    Labels.Add(MyLabel.SetOKLabel("Please select a gender!"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetFailed());
                    sport.Write("#ERROR: Gender is empty!\n");
                }
                else if (methods.CheckId(cardIdLabel.Text) == false)
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("Card already exists!"));
                    Labels.Add(MyLabel.SetOKLabel("There is a card in the database!"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetFailed());
                    sport.Write("#ERROR: Already a member!\n");
                }

                else
                    using (SqlConnection connection = new SqlConnection(
                 global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                    {
                        using (SqlCommand command = new SqlCommand("INSERT INTO Member(Name,Surname,Address,PhoneNumber,CardId,TypeId,NumOfEntrances,Gender,LastEntrance)" +
                                "VALUES(@Name,@Surname,@Address,@PhoneNumber,@CardId,@TypeId,@NumOfEntrances, @Gender, @LastEntrance)", connection))
                        {
                            string cardType = " ";
                            string gender = " ";
                            if (radioButton2.Checked == true)
                                cardType = "Student";
                            if (radioButton3.Checked == true)
                                cardType = "Work";
                            if (radioButton5.Checked == true)
                                gender = "Male";
                            if (radioButton6.Checked == true)
                                gender = "Female";

                            command.Parameters.AddWithValue("Name", name.Text);
                            command.Parameters.AddWithValue("Surname", surname.Text);
                            command.Parameters.AddWithValue("Address", address.Text);
                            command.Parameters.AddWithValue("PhoneNumber", phoneno.Text);
                            command.Parameters.AddWithValue("Cardid", Convert.ToInt64(cardIdLabel.Text));
                            command.Parameters.AddWithValue("TypeId", cardType);
                            command.Parameters.AddWithValue("NumOfEntrances", 1);
                            command.Parameters.AddWithValue("Gender", gender);
                            command.Parameters.AddWithValue("LastEntrance", DateTime.Now);


                            command.Connection.Open();

                            if (command.ExecuteNonQuery().ToString() == "1")
                            {
                                List<Label> Labels = new List<Label>();
                                Labels.Add(MyLabel.SetOKLabel("Member insertion."));
                                Labels.Add(MyLabel.SetOKLabel("Member insertion passed."));

                                List<Button> Buttons = new List<Button>();
                                Buttons.Add(MyButton.SetOKThemeButton());
                                MyMessageBox.Show(
                                    Labels,
                                    "",
                                    Buttons,
                                    MyImage.SetSuccess());
                                if (cardType == "Student")
                                {
                                    sport.Write("#SUCCS: Your membership is 40 KM!\n");
                                } else
                                {
                                    sport.Write("#SUCCS: Your membership is 50 KM!\n");
                                }
                            }
                            else
                            {
                                List<Label> Labels = new List<Label>();
                                Labels.Add(MyLabel.SetOKLabel("Member insertion."));
                                Labels.Add(MyLabel.SetOKLabel("Member insertion failed."));

                                List<Button> Buttons = new List<Button>();
                                Buttons.Add(MyButton.SetOKThemeButton());
                                MyMessageBox.Show(
                                    Labels,
                                    "",
                                    Buttons,
                                    MyImage.SetFailed());
                            }
                            command.Connection.Close();

                        }
                        int p = methods.GetMemberId(cardIdLabel.Text);

                        using (SqlCommand command2 = new SqlCommand("INSERT INTO Account(PaymentDate,ExpirationDate,Price,Name,Surname,MemberId)" +
                               "VALUES(@PaymentDate,@ExpirationDate,@Price,@Name,@Surname,@MemberId)", connection))
                        {
                            command2.Parameters.AddWithValue("PaymentDate", DateTime.Today);
                            command2.Parameters.AddWithValue("ExpirationDate", DateTime.Today.AddDays(30 * (monthsno.SelectedIndex + 1)));
                            command2.Parameters.AddWithValue("Price", price.Text);
                            command2.Parameters.AddWithValue("Name", name.Text);
                            command2.Parameters.AddWithValue("Surname", surname.Text);
                            command2.Parameters.AddWithValue("MemberId", p);
                            command2.Connection.Open();

                            command2.ExecuteNonQuery();

                            command2.Connection.Close();
                        }
                        using (SqlCommand command3 = new SqlCommand("INSERT INTO Report(MemberId,EntranceDate,Name,Surname) VALUES(@MemberId,@EntranceDate,@Name,@Surname)", connection))
                        {

                            command3.Parameters.AddWithValue("@MemberId", p);
                            command3.Parameters.AddWithValue("@EntranceDate", DateTime.Now);
                            command3.Parameters.AddWithValue("@Name", name.Text);
                            command3.Parameters.AddWithValue("@Surname", surname.Text);

                            command3.Connection.Open();
                            command3.ExecuteNonQuery();

                            command3.Connection.Close();

                        }

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
                        }

                        saveImage();
                        this.Close();
                    }

            }
            catch (Exception ex)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("General error"));
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

        public static Bitmap LoadBitmap(string path)
        {
            //Open file in read only mode
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            //Get a binary reader for the file stream
            using (BinaryReader reader = new BinaryReader(stream))
            {
                //copy the content of the file into a memory stream
                var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                //make a new Bitmap object the owner of the MemoryStream
                return new Bitmap(memoryStream);
            }
        }

        private void saveImage()
        {
            Bitmap bmp1 = new Bitmap(pictureBox1.Image);

            if (System.IO.File.Exists(path.Trim() + cardIdLabel.Text.Trim() + ext))
                System.IO.File.Delete(path.Trim() + cardIdLabel.Text.Trim() + ext);

            bmp1.Save(path.Trim() + "\\Images\\" + cardIdLabel.Text.Trim() + ext);
            bmp1.Dispose();
        }

        private void _cardid_TextChanged(object sender, EventArgs e)
        {

            if (methods.CheckId(cardIdLabel.Text) == false)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("Card already exists."));
                Labels.Add(MyLabel.SetOKLabel("There is a card with this Card Id."));

                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
            }
        }

        private void _monthsno_SelectedIndexChanged(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(monthsno.Text);
            if (radioButton3.Checked == true)
                price.Text = (x * 50).ToString();
            else if (radioButton2.Checked == true)
                price.Text = (x * 40).ToString();
            else
                price.Text = (x * 0).ToString();
        }

        private void radioButton3_Click_1(object sender, EventArgs e)
        {

            int x = Convert.ToInt32(monthsno.Text);
            price.Text = (x * 50).ToString();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(monthsno.Text);
            price.Text = (x * 40).ToString();
        }

        private void AddMemberForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            sport.Close();

        }

        private void button2_Click_1(object sender, EventArgs e)

        {
            sport.Close();
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog d = new OpenFileDialog();
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    pictureBox1.Image = LoadBitmap(d.FileName);

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
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
