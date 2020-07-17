using CustomModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GymMembershipSystem
{
    public partial class ExtendMemberShipForm : Form
    {
        PartialMember b;
        string basePrice;
        string path = @"C:\Users\Mehmed\Desktop\GymMembershipSystem\GymMembershipSystem";
        string ext = ".jpg";

        public ExtendMemberShipForm(PartialMember a)
        {
            InitializeComponent();
            b = new PartialMember();
            b = a;
        }

        private void ExtendMemberShipForm_Load(object sender, EventArgs e)
        {
            namelabel.Text = b.Name;
            surnamelabel.Text = b.Surname;
            noofentranceslabel.Text = b.NumOfEntrances.ToString();
            expirationdatelabel.Text = b.ExpirationDate.ToString("dd.MM.yyyy");
            int d = Convert.ToInt32((b.ExpirationDate.Date - DateTime.Now.Date).TotalDays);
            string typeId = b.TypeId.Trim();
            if (d > 0)
                daysleftlabel.Text = d.ToString();
            else
            {
                daysleftlabel.Text = "0";
                d = 0;
            }
            if (typeId == "VIP")
                pricelabel.Text = "0";
            if (typeId == "Student")
                pricelabel.Text = "40";
            if (typeId == "Work")
                pricelabel.Text = "50";
            if (typeId == "Other")
                pricelabel.Text = "50";
            basePrice = pricelabel.Text;
            if (d > 0)
                membershipuntil.Text = DateTime.Now.AddDays(30 + d).ToString();
            else
                membershipuntil.Text = DateTime.Now.AddDays(30).ToString();

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


                path = @"" + path + "\\Images\\";
                string imagePath = path.Trim() + b.CardId.ToString() + ext;
                if (!File.Exists(imagePath))
                {
                    imagePath = path.Trim() + "av2" + ".png";
                }
                pictureBox1.Image = LoadBitmap(imagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }


        }

        private void _noofmonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            membershipuntil.Text = "";
            int x = Convert.ToInt32(noofmonthslabel.Text);
            int p = Convert.ToInt32(basePrice);
            int s = Convert.ToInt32(daysleftlabel.Text);
            int tmp = discountlabel.SelectedIndex;
            discountlabel.SelectedIndex = 0;
            discountlabel.SelectedIndex = tmp;
            string typeId = b.TypeId.Trim();
            if (discountlabel.SelectedIndex == 0)
            {
                if (typeId == "VIP")
                    pricelabel.Text = (0 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
                if (typeId == "Student")
                    pricelabel.Text = (40 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
                if (typeId == "Work")
                    pricelabel.Text = (50 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
                if (typeId == "Other")
                    pricelabel.Text = (50 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
            }
            if (s > 0)
                membershipuntil.Text = DateTime.Now.AddDays(30 * x + s).ToString();
            else
                membershipuntil.Text = DateTime.Now.AddDays(30).ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string typeId = b.TypeId.Trim();

            if (typeId == "VIP")
                pricelabel.Text = (0 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
            if (typeId == "Student")
                pricelabel.Text = (40 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
            if (typeId == "Work")
                pricelabel.Text = (50 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
            if (typeId == "Other")
                pricelabel.Text = (50 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
            int discnumber = Convert.ToInt32(pricelabel.Text);

            if (discountlabel.SelectedIndex == 0)
            {
                if (typeId == "Student")
                    pricelabel.Text = (40 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
                if (typeId == "Work")
                    pricelabel.Text = (50 * Convert.ToInt32(noofmonthslabel.Text)).ToString();
            }

            if (discountlabel.SelectedIndex == 1)
            {
                int x = (discnumber * 5) / 100;
                x = (discnumber - x);
                pricelabel.Text = x.ToString();
            }

            if (discountlabel.SelectedIndex == 2)
            {
                int x = (discnumber * 10) / 100;
                x = (discnumber - x);
                pricelabel.Text = x.ToString();
            }
            if (discountlabel.SelectedIndex == 3)
            {
                int x = (discnumber * 20) / 100;
                x = (discnumber - x);
                pricelabel.Text = x.ToString();
            }
            if (discountlabel.SelectedIndex == 4)
            {
                int x = (discnumber * 30) / 100;
                x = (discnumber - x);
                pricelabel.Text = x.ToString();
            }
            if (discountlabel.SelectedIndex == 5)
            {
                int x = (discnumber * 40) / 100;
                x = (discnumber - x);
                pricelabel.Text = x.ToString();
            }
            if (discountlabel.SelectedIndex == 6)
            {
                int x = (discnumber * 50) / 100;
                x = (discnumber - x);
                pricelabel.Text = x.ToString();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(
           global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {

                    using (SqlCommand command2 = new SqlCommand("INSERT INTO Account(PaymentDate,ExpirationDate,Price,Name,Surname,MemberId)" +
                           "VALUES(@PaymentDate,@ExpirationDate,@Price,@Name,@Surname,@MemberId)", connection))
                    {
                        int x = Convert.ToInt32(noofmonthslabel.Text);

                        command2.Parameters.AddWithValue("PaymentDate", DateTime.Today);
                        command2.Parameters.AddWithValue("ExpirationDate", Convert.ToDateTime(membershipuntil.Text));
                        command2.Parameters.AddWithValue("Price", pricelabel.Text);
                        command2.Parameters.AddWithValue("Name", namelabel.Text);
                        command2.Parameters.AddWithValue("Surname", surnamelabel.Text);
                        command2.Parameters.AddWithValue("MemberId", b.id);
                        command2.Connection.Open();

                        if (command2.ExecuteNonQuery().ToString() == "1")
                        {
                            List<Label> Labels = new List<Label>();
                            Labels.Add(MyLabel.SetOKLabel("Membership extension."));
                            Labels.Add(MyLabel.SetOKLabel("Successfully Passed"));

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
                            Labels.Add(MyLabel.SetOKLabel("Membership extension."));
                            Labels.Add(MyLabel.SetOKLabel("Failed."));

                            List<Button> Buttons = new List<Button>();
                            Buttons.Add(MyButton.SetOKThemeButton());
                            MyMessageBox.Show(
                                Labels,
                                "",
                                Buttons,
                                MyImage.SetFailed());
                        }
                        command2.Connection.Close();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("Membership extension."));
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

        public static Bitmap LoadBitmap(string path)
        {
            using (
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                return new Bitmap(memoryStream);
            }
        }
    }
}
