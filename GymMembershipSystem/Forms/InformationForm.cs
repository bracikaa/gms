using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Gsort;
using Method;
using System.IO.Ports;
using CustomModels;

namespace GymMembershipSystem
{
    public partial class InformationForm : Form
    {
        PartialMember a = new PartialMember();
        SortableBindingList<ReducedAccount> catSortable;
        SortableBindingList<Report> catSortable2;
        SortableBindingList<ShopPayment> catSortable3;
        SortableBindingList<TrainingEnrollment> catSortable4;
        List<ReducedAccount> accounts = new List<ReducedAccount>();
        List<Report> reports = new List<Report>();
        List<ShopPayment> shopPayments = new List<ShopPayment>();
        List<TrainingEnrollment> trainingEnrollments = new List<TrainingEnrollment>();
        string id;
        Methods methods = new Methods();
        string path = @"C:\Users\Mehmed\Desktop\GymMembershipSystem\GymMembershipSystem";
        string ext = ".jpg";
        public SerialPort sport = new SerialPort();
        public InformationForm(string id)
        {
            InitializeComponent();
            try
            {
                this.id = id;
                string tmp = "0";
                tmp = methods.getComm();

                sport = new System.IO.Ports.SerialPort("COM" + tmp, 9600, Parity.None, 8, StopBits.One);
                if (id != "0")
                {
                    cardid.Text = cardid.Text = "---------------------------";
                    cardid.Text = id;
                }
                else
                {
                    cardid.Text = "---------------------------";

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

        private void InformationForm_Load(object sender, EventArgs e)
        {
            if (id != "0")
            {
                GetInfo();
                name.Text = a.Name;
                surname.Text = a.Surname;
                address.Text = a.Address;
                phone.Text = a.PhoneNumber;
                numofentrances.Text = a.NumOfEntrances.ToString();
                cardtype.Text = a.TypeId;
                expdate.Text = a.ExpirationDate.Date.ToString("dd/MM/yyyy");
                DateTime now = DateTime.Now;
                int d = Convert.ToInt32((a.ExpirationDate.Date.AddDays(1) - now).TotalDays);
                gender.Text = a.Gender;
                if (d > 0)
                    daysleft.Text = d.ToString();
                cardid.Text = a.CardId.ToString();
                GetAccount(a.id);
                BindingSource b = new BindingSource();
                catSortable = new SortableBindingList<ReducedAccount>(accounts);
                b.DataSource = catSortable;

                dataGridView1.DataSource = b;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[2].Width = 250;
                dataGridView1.Columns[3].Width = 250;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;

                GetReport(a.id);
                BindingSource c = new BindingSource();

                catSortable2 = new SortableBindingList<Report>(reports);
                c.DataSource = catSortable2;
                dataGridView2.DataSource = c;
                dataGridView2.RowHeadersVisible = false;
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Width = 250;
                dataGridView2.Columns[2].Width = 250;
                dataGridView2.Columns[3].Width = 250;
                dataGridView2.Columns[4].Visible = false;

                GetShop(a.id);
                BindingSource f = new BindingSource();

                catSortable3 = new SortableBindingList<ShopPayment>(shopPayments);
                f.DataSource = catSortable3;
                dataGridView3.DataSource = f;
                dataGridView3.RowHeadersVisible = false;
                dataGridView3.Columns[0].Visible = false;
                dataGridView3.Columns[1].Width = 125;
                dataGridView3.Columns[2].Width = 125;
                dataGridView3.Columns[3].Width = 125;
                dataGridView3.Columns[4].Width = 125;
                dataGridView3.Columns[5].Width = 125;
                dataGridView3.Columns[6].Width = 125;
                dataGridView3.Columns[7].Visible = false;
                dataGridView3.Columns[8].Visible = false;

                GetEnrollments(a.id);
                BindingSource g = new BindingSource();

                catSortable4 = new SortableBindingList<TrainingEnrollment>(trainingEnrollments);
                g.DataSource = catSortable4;
                dataGridView4.DataSource = g;
                dataGridView4.RowHeadersVisible = false;
                dataGridView4.Columns[0].Visible = false;
                dataGridView4.Columns[1].Width = 125;
                dataGridView4.Columns[2].Width = 125;
                dataGridView4.Columns[3].Width = 125;
                dataGridView4.Columns[4].Width = 125;
                dataGridView4.Columns[5].Width = 125;
                dataGridView4.Columns[6].Width = 125;
                dataGridView4.Columns[7].Visible = false;
                dataGridView4.Columns[8].Visible = false;

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

                path = @"" + path + "\\Images\\";
                string imagePath = path.Trim() + a.CardId.ToString() + ext;
                if (!File.Exists(imagePath))
                {
                    imagePath = path.Trim() + "av2" + ".png";
                }
                pictureBox1.Image = LoadBitmap(imagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
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

        private void GetAccount(long id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT PaymentDate, ExpirationDate, Price FROM Account WHERE MemberId=@id ", connection))
                    {
                        cmd2.Parameters.AddWithValue("@id", id);

                        cmd2.CommandType = CommandType.Text;

                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            DateTime payments = Convert.ToDateTime(dr["PaymentDate"]);
                            DateTime expirations = Convert.ToDateTime(dr["ExpirationDate"]);
                            double amounts = Convert.ToDouble(dr["Price"].ToString());

                            ReducedAccount c = new ReducedAccount();

                            c.Price = amounts;
                            c.PaymentDate = payments;
                            c.ExpirationDate = expirations;
                            accounts.Add(c);

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

        private void GetReport(long id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT Name, Surname, EntranceDate FROM Report WHERE MemberId=@id ", connection))
                    {
                        cmd2.Parameters.AddWithValue("@id", id);

                        cmd2.CommandType = CommandType.Text;

                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            string name = Convert.ToString(dr["Name"]);
                            string surname = Convert.ToString(dr["Surname"]);
                            DateTime entranceDates = Convert.ToDateTime(dr["EntranceDate"].ToString());

                            Report c = new Report();

                            c.Name = name;
                            c.Surname = surname;
                            c.EntranceDate = entranceDates;
                            reports.Add(c);

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

        private void GetShop(long id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT PaymentDate, Name, Surname, ItemId, ItemName, ItemPrice FROM ShopPayment WHERE MemberId=@id ", connection))
                    {
                        cmd2.Parameters.AddWithValue("@id", id);

                        cmd2.CommandType = CommandType.Text;

                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            string name = Convert.ToString(dr["Name"]);
                            string surname = Convert.ToString(dr["Surname"]);
                            DateTime paymentDate = Convert.ToDateTime(dr["PaymentDate"].ToString());
                            int itemid = Convert.ToInt32(dr["ItemId"]);
                            string itemname = Convert.ToString(dr["ItemName"]);
                            int itemPrice = Convert.ToInt32(dr["ItemPrice"]);

                            ShopPayment sp = new ShopPayment();

                            sp.Name = name;
                            sp.Surname = surname;
                            sp.PaymentDate = paymentDate;
                            sp.ItemId = itemid;
                            sp.ItemName = itemname;
                            sp.ItemPrice = itemPrice;
                            shopPayments.Add(sp);

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

        private void GetEnrollments(long id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT EnrollmentDate, Name, Surname, ExerciseId, ExerciseName, ExercisePrice FROM TrainingEnrollment WHERE MemberId=@id ", connection))
                    {
                        cmd2.Parameters.AddWithValue("@id", id);

                        cmd2.CommandType = CommandType.Text;

                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            string name = Convert.ToString(dr["Name"]);
                            string surname = Convert.ToString(dr["Surname"]);
                            DateTime enrollmentdate = Convert.ToDateTime(dr["EnrollmentDate"].ToString());
                            int exerciseid = Convert.ToInt32(dr["ExerciseId"]);
                            string exercisename = Convert.ToString(dr["ExerciseName"]);
                            decimal exercisePrice = Convert.ToDecimal(dr["ExercisePrice"]);

                            TrainingEnrollment te = new TrainingEnrollment();

                            te.Name = name;
                            te.Surname = surname;
                            te.EnrollmentDate = enrollmentdate;
                            te.ExerciseId = exerciseid;
                            te.ExerciseName= exercisename;
                            te.ExercisePrice= exercisePrice;
                            trainingEnrollments.Add(te);

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

        private void button4_Click(object sender, EventArgs e)
        {

            if (name.Text != "---------------------------")
            {
                ExtendMemberShipForm extendmembershipform = new ExtendMemberShipForm(a);
                extendmembershipform.ShowDialog();

                Int64 i = Convert.ToInt64(cardid.Text);

                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT id FROM Member WHERE CardId=@CardId", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardId", i);
                        connection.Open();

                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            string responseid = o.ToString();
                            int k = Convert.ToInt32(responseid);
                            using (SqlCommand cmd2 = new SqlCommand("SELECT Name, Surname, Address, PhoneNumber, TypeId," +
                                "NumOfEntrances, Gender, LastEntrance FROM Member WHERE id=@idd", connection))
                            {
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Parameters.AddWithValue("@idd", k);

                                SqlDataReader dr = cmd2.ExecuteReader();
                                while (dr.Read())
                                {
                                    string name1 = dr["Name"].ToString();
                                    string surname1 = dr["Surname"].ToString();
                                    string address1 = dr["Address"].ToString();
                                    string phoneno1 = dr["PhoneNumber"].ToString();
                                    string type1 = dr["TypeId"].ToString();
                                    int numofentr1 = Convert.ToInt32(dr["NumOfEntrances"]);
                                    DateTime lastent1 = Convert.ToDateTime(dr["LastEntrance"]);
                                    string gender1 = dr["Gender"].ToString();

                                    DateTime z = methods.GetExpirationDate(k);
                                    if (lastent1.Date != DateTime.Today.Date)
                                    {
                                        numofentr1++;
                                        methods.WriteEntrance(k, numofentr1, name1, surname1);
                                    }
                                    methods.WriteLastEntrance(k);
                                    lastent1 = DateTime.Now;

                                    a.Name = name1;
                                    a.Surname = surname1;
                                    a.Address = address1;
                                    a.PhoneNumber = phoneno1;
                                    a.TypeId = type1;
                                    a.NumOfEntrances = numofentr1;
                                    a.ExpirationDate = z;
                                    a.CardId = i;
                                    a.LastEntrance = lastent1;
                                    a.Gender = gender1;
                                    a.id = k;
                                }
                            }
                            connection.Close();
                            cardid.Text = "---------------------------";
                            cardid.Text = i.ToString();
                        }
                    }
                }
            }
        }

        private void GetInfo()
        {
            try
            {
                Int64 i = Convert.ToInt64(id);

                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT id FROM Member WHERE CardId=@CardId", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardId", i);
                        connection.Open();

                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            string city = o.ToString();
                            int k = Convert.ToInt32(city);
                            using (SqlCommand cmd2 = new SqlCommand("SELECT Name, Surname, Address, PhoneNumber, TypeId, NumOfEntrances, Gender, " +
                                "LastEntrance FROM Member WHERE id=@id", connection))
                            {
                                cmd2.CommandType = CommandType.Text;
                                cmd2.Parameters.AddWithValue("@id", k);

                                SqlDataReader dr = cmd2.ExecuteReader();
                                while (dr.Read())
                                {
                                    string name1 = dr["Name"].ToString();
                                    string surname1 = dr["Surname"].ToString();
                                    string address1 = dr["Address"].ToString();
                                    string phoneno1 = dr["PhoneNumber"].ToString();
                                    string typeid1 = dr["TypeId"].ToString();
                                    int numofentrances1 = Convert.ToInt32(dr["NumOfEntrances"]);
                                    DateTime lastEntr = Convert.ToDateTime(dr["LastEntrance"]);
                                    string gender1 = dr["Gender"].ToString();

                                    DateTime z = methods.GetExpirationDate(k);
                                    if (lastEntr.Date != DateTime.Today)
                                    {
                                        numofentrances1++;
                                        methods.WriteEntrance(k, numofentrances1, name1, surname1);

                                    }
                                    methods.WriteLastEntrance(k);
                                    lastEntr = DateTime.Now;


                                    a.Name = name1;
                                    a.Surname = surname1;
                                    a.Address = address1;
                                    a.PhoneNumber = phoneno1;
                                    a.TypeId = typeid1;
                                    a.NumOfEntrances = numofentrances1;
                                    a.ExpirationDate = z;
                                    a.CardId = i;
                                    a.LastEntrance = lastEntr;
                                    a.Gender = gender1;
                                    a.id = k;

                                }
                            }

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
    }
}
