using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Method;
using Gsort;
using CustomModels;
using System.IO.Ports;
using System.IO;

namespace GymMembershipSystem
{
    public partial class SearchMembersForm : Form
    {
        string path = @"C:\Users\Mehmed\Desktop\GymMembershipSystem\";
        string ext = ".jpg";
        public SerialPort sport = new SerialPort();
        string tmp = "";
        string globalId;
        int male = 0;
        int female = 0;
        Methods method = new Methods();

        public SearchMembersForm()
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
        List<PartialMember> members = new List<PartialMember>();
        List<PartialMember> members2 = new List<PartialMember>();
        PartialMember a = new PartialMember();
        Methods methods = new Methods();

        private void GetMembers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT id, Name, Surname, Address, PhoneNumber, CardId, TypeId," +
                        " NumOfEntrances, Gender, LastEntrance FROM Member", connection))
                    {
                        cmd2.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            long id = Convert.ToInt32(dr["id"]);
                            string name = dr["Name"].ToString();
                            string surname = dr["Surname"].ToString();
                            string address = dr["Address"].ToString();
                            string phonenum = dr["PhoneNumber"].ToString();
                            string typeid = dr["TypeId"].ToString();
                            int numofentr = Convert.ToInt32(dr["NumOfEntrances"]);
                            Int64 cardid = Convert.ToInt64(dr["CardId"]);
                            DateTime lastEntrance = Convert.ToDateTime(dr["LastEntrance"]);
                            string gender = dr["Gender"].ToString();

                            DateTime expDate = method.GetExpirationDate(id);

                            PartialMember newMember = new PartialMember();

                            newMember.Name = name;
                            newMember.Surname = surname;
                            newMember.Address = address;
                            newMember.PhoneNumber = phonenum;
                            newMember.TypeId = typeid;
                            newMember.NumOfEntrances = numofentr;
                            newMember.ExpirationDate = expDate;
                            newMember.CardId = cardid;
                            newMember.LastEntrance = lastEntrance;
                            newMember.Gender = gender;
                            if (gender.Trim() == "Female")
                            {
                                female++;
                            }
                            else
                            {
                                male++;
                            }
                            newMember.id = id;
                            newMember.NumOfDays = method.GetNumberOfEntrances(cardid.ToString());
                            members.Add(newMember);
                        }

                        label9.Text = members.Count.ToString();
                        label2.Text = male.ToString();
                        label10.Text = female.ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchMembersForm_Load(object sender, EventArgs e)
        {
            GetMembers();
        }

        public void populate()
        {
            listView1.Items.Clear();
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(125, 165);
            imgs.ColorDepth = ColorDepth.Depth16Bit;

            try
            {
                listView1.LargeImageList = imgs;

                for (int i = 0; i < members.Count; i++)
                {
                    string memberImagePath = path.Trim() + "Images\\" + members[i].CardId.ToString() + ext;
                    if (!File.Exists(memberImagePath))
                    {
                        memberImagePath = path.Trim() + "Images\\av2.png";
                    }

                    System.Drawing.Image img = new Bitmap(System.Drawing.Image.FromFile(memberImagePath));

                    using (Graphics g = Graphics.FromImage(img))
                    {
                        g.DrawRectangle(Pens.Black, 0, 0, img.Width - 2, img.Height - 2);
                    }
                    imgs.Images.Add(img);
                    listView1.Items.Add(members[i].id.ToString(), members[i].Name + " " + members[i].Surname, i);
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
            globalId = listView1.SelectedItems[0].SubItems[0].Name;
            GetInfo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                ExtendMemberShipForm extendmembershipform = new ExtendMemberShipForm(a);
                extendmembershipform.ShowDialog();
        }
        private void GetInfo()
        {
            try
            {
                int i = Convert.ToInt32(globalId);

                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT Name, Surname, CardId, Address, PhoneNumber, TypeId, NumOfEntrances, Gender, " +
                        "LastEntrance FROM Member WHERE id=@id", connection))
                    {
                        connection.Open();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Parameters.AddWithValue("@id", i);

                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            string name1 = dr["Name"].ToString();
                            string surname1 = dr["Surname"].ToString();
                            string address1 = dr["Address"].ToString();
                            string phoneno1 = dr["PhoneNumber"].ToString();
                            string typeid1 = dr["TypeId"].ToString();
                            long cardId1 = Convert.ToInt64(dr["CardId"]);
                            int numofentrances1 = Convert.ToInt32(dr["NumOfEntrances"]);
                            DateTime lastEntr = Convert.ToDateTime(dr["LastEntrance"]);
                            string gender1 = dr["Gender"].ToString();

                            DateTime z = methods.GetExpirationDate(i);
                            if (lastEntr.Date != DateTime.Today)
                            {
                                numofentrances1++;
                                methods.WriteEntrance(i, numofentrances1, name1, surname1);

                            }
                            string date = z.Day + "/" + z.Month + "/" + z.Year;
                            methods.WriteLastEntrance(i);
                            lastEntr = DateTime.Now;


                            panel1.Visible = true;
                            label14.Text = name1;
                            label8.Text = surname1;
                            label4.Text = gender1;
                            label24.Text = address1;
                            label22.Text = phoneno1;
                            label21.Text = date;
                            label20.Text = cardId1.ToString();
                            label15.Text = typeid1;
                            label17.Text = numofentrances1.ToString();


                            a.Name = name1;
                            a.Surname = surname1;
                            a.Address = address1;
                            a.PhoneNumber = phoneno1;
                            a.TypeId = typeid1;
                            a.NumOfEntrances = numofentrances1;
                            a.ExpirationDate = z;
                            a.CardId = cardId1;
                            a.LastEntrance = lastEntr;
                            a.Gender = gender1;
                            a.id = i;

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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                InformationForm ev = new InformationForm(a.CardId.ToString());
                ev.ShowDialog();
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            members.Clear();
            string genderCheckBox = "";
            string query = "";
            if(radioButton5.Checked)
            {
                genderCheckBox = "Male";
            } else if(radioButton6.Checked)
            {
                genderCheckBox = "Female";
            }

            if(genderCheckBox != "")
            {
                 query = "SELECT id, Name, Surname, CardId, TypeId, NumOfEntrances, Gender, " +
                        "LastEntrance, PhoneNumber, Address FROM Member WHERE (Name LIKE '%' + @d + '%' OR Surname LIKE '%' +  @d +'%') AND Gender = @g";
            } else
            {
                query = "SELECT id, Name, Surname, CardId, TypeId, NumOfEntrances, Gender, " +
                        "LastEntrance, PhoneNumber, Address FROM Member WHERE Name LIKE '%' + @d + '%' OR Surname LIKE '%' +  @d +'%'";
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand(query, connection))
                    {

                        cmd2.Parameters.AddWithValue("@d", name.Text);
                        if(genderCheckBox != "")
                        {
                            cmd2.Parameters.AddWithValue("@g", genderCheckBox);
                        }
                        cmd2.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            int id2 = Convert.ToInt32(dr["id"]);
                            string name = dr["Name"].ToString();
                            string surname = dr["Surname"].ToString();
                            string address = dr["Address"].ToString();
                            string phonenum = dr["PhoneNumber"].ToString();
                            string typeid = dr["TypeId"].ToString();
                            int numofentr = Convert.ToInt32(dr["NumOfEntrances"]);
                            Int64 cardid = Convert.ToInt64(dr["CardId"]);
                            DateTime lastEntrance = Convert.ToDateTime(dr["LastEntrance"]);
                            string gender = dr["Gender"].ToString();

                            DateTime expDate = method.GetExpirationDate(id2);

                            PartialMember newMember = new PartialMember();

                            newMember.Name = name;
                            newMember.Surname = surname;
                            newMember.Address = address;
                            newMember.PhoneNumber = phonenum;
                            newMember.TypeId = typeid;
                            newMember.NumOfEntrances = numofentr;
                            newMember.ExpirationDate = expDate;
                            newMember.CardId = cardid;
                            newMember.LastEntrance = lastEntrance;
                            newMember.Gender = gender;
                            newMember.id = id2;
                            newMember.NumOfDays = method.GetNumberOfEntrances(cardid.ToString());
                            members.Add(newMember);
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
    }

}
