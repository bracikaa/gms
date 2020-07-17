using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymMembershipSystem.Forms
{
    public partial class ShopItemForm : Form
    {
        string path = @"C:\Users\Mehmed\Desktop\GymMembershipSystem\GymMembershipSystem";
        string ext = ".jpg";
        string itemId;
        public ShopItemForm(string id)
        {
            itemId = id;
            InitializeComponent();
        }

        private void ShopItemForm_Load(object sender, EventArgs e)
        {
            try
            {
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
                path = @"" + path + "\\Images\\shopitems\\";

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

            if (itemId == "0")
            {
                groupBox2.Text = "Add new Item:";
                button2.Text = "Add";
                getLastIndex();
            }
            else
            {
                groupBox2.Text = "Update Item";
                button2.Text = "Update";
                updateItem();
            }
        }

        private void getLastIndex()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    //"SELECT TOP 1 * FROM Measurement WHERE MemberId='"+ comboBox1.SelectedValue +"' ORDER BY Timestamp DESC"
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 ItemId FROM Items ORDER BY ItemId DESC", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            int idVlaue = Convert.ToInt32(o) + 1;
                            string itemidstring = idVlaue.ToString();
                            itemid2.Text = itemidstring;
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

        private void updateItem()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    //SELECT TOP 1 * FROM Measurement WHERE MemberId=1 ORDER BY Timestamp DESC;
                    using (SqlCommand cmd2 = new SqlCommand("SELECT ItemName, ItemId, ItemPrice, ItemCount, ItemDescription" +
                        " FROM Items WHERE ItemId = " + itemId, connection))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();

                        while (dr.Read())
                        {
                            string itemname2 = dr["ItemName"].ToString();
                            string itemid33 = dr["ItemId"].ToString();
                            string itemprice = dr["ItemPrice"].ToString();
                            string itemcount = dr["ItemCount"].ToString();
                            string itemdesc2 = dr["ItemDescription"].ToString();

                            itemname.Text = itemname2;
                            itemid2.Text = itemid33;
                            itemprice2.Text = itemprice;
                            itemcount1.Text = itemcount;
                            itemdesc1.Text = itemdesc2;
                            pictureBox1.Image = LoadBitmap(path + "\\" + itemid33 + ext);
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (itemId == "0")
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                    {
                        using (SqlCommand command2 = new SqlCommand("INSERT INTO Items(ItemName, ItemId, ItemPrice, ItemCount, ItemDescription) " +
                            "VALUES(@ItemName, @ItemId, @ItemPrice, @ItemCount, @ItemDescription)", connection))
                        {

                            command2.Parameters.AddWithValue("@ItemName", itemname.Text.ToUpper());
                            command2.Parameters.AddWithValue("@ItemId", Convert.ToInt32(itemid2.Text));
                            command2.Parameters.AddWithValue("@ItemPrice", itemprice2.Text);
                            command2.Parameters.AddWithValue("@ItemCount", Convert.ToInt32(itemcount1.Text));
                            command2.Parameters.AddWithValue("@ItemDescription", itemdesc1.Text);

                            command2.Connection.Open();
                            command2.ExecuteNonQuery();
                            command2.Connection.Close();

                            saveImage();

                            itemname.Clear();
                            getLastIndex();
                            itemprice2.Clear();
                            itemcount1.Clear();
                            itemdesc1.Clear();

                            List<Label> Labels = new List<Label>();
                            Labels.Add(MyLabel.SetOKLabel("Adding Items"));
                            Labels.Add(MyLabel.SetOKLabel("Sucessfully added item."));

                            List<Button> Buttons = new List<Button>();
                            Buttons.Add(MyButton.SetOKThemeButton());
                            MyMessageBox.Show(
                                Labels,
                                "",
                                Buttons,
                                MyImage.SetSuccess());
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
            } else
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE Items SET ItemCount" +
                                "=@Count, ItemPrice=@Price WHERE ItemId=@itemid", connection))
                        {
                            command.Parameters.AddWithValue("@Count", itemcount1.Text);
                            command.Parameters.AddWithValue("@Price", itemprice2.Text);
                            command.Parameters.AddWithValue("@itemid", itemId);
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                            command.Connection.Close();

                            List<Label> Labels = new List<Label>();
                            Labels.Add(MyLabel.SetOKLabel("Updating Items"));
                            Labels.Add(MyLabel.SetOKLabel("Sucessfully updated item."));

                            List<Button> Buttons = new List<Button>();
                            Buttons.Add(MyButton.SetOKThemeButton());
                            MyMessageBox.Show(
                                Labels,
                                "",
                                Buttons,
                                MyImage.SetSuccess());

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

        private void saveImage()
        {
            Bitmap bmp1 = new Bitmap(pictureBox1.Image);

            if (System.IO.File.Exists(path.Trim() + itemid2.Text.Trim() + ext))
                System.IO.File.Delete(path.Trim() + itemid2.Text.Trim() + ext);

            bmp1.Save(path.Trim() + itemid2.Text.Trim() + ext);
            bmp1.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
