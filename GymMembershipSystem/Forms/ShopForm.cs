using CustomModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Method;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymMembershipSystem.Forms
{
    public partial class ShopForm : Form
    {
        public SerialPort sport = new SerialPort();
        string tmp = "";
        int itemcount;
        string itemId;
        Methods method = new Methods();
        List<FullItem> items = new List<FullItem>();
        string path = @"C:\Users\Mehmed\Desktop\GymMembershipSystem\";
        string ext = ".jpg";
        string datareceived = "";
        long theNewId;
        public ShopForm()
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

        public void ShopForm_Load(object sender, EventArgs e)
        {
            sport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            sport.Open();
            listView1.View = View.LargeIcon;
            listView1.Columns.Add("", 200);

            getItems();
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


        private void button1_Click(object sender, EventArgs e)
        {
            string[] name;
            if (label12.Text != "----------" && label8.Text != "----------")
            {
                if (label10.Text == "OUT OF STOCK")
                {
                    List<Label> Labels = new List<Label>();
                    Labels.Add(MyLabel.SetOKLabel("General Error"));
                    Labels.Add(MyLabel.SetOKLabel("Item Out of Stock"));

                    List<Button> Buttons = new List<Button>();
                    Buttons.Add(MyButton.SetOKThemeButton());
                    MyMessageBox.Show(
                        Labels,
                        "",
                        Buttons,
                        MyImage.SetFailed());
                    sport.Write("#ERROR: Item Out Of Stock!\n");
                }
                else
                {

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                        {
                            using (SqlCommand command2 = new SqlCommand("INSERT INTO ShopPayment(MemberId, PaymentDate, Name, Surname, ItemId, ItemName, ItemPrice) VALUES(@MemberId, @EntranceDate, @Name, @Surname, @ItemId, @ItemName, @ItemPrice)", connection))
                            {
                                name = label13.Text.Split(null);
                                command2.Parameters.AddWithValue("@MemberId", label12.Text);
                                command2.Parameters.AddWithValue("@EntranceDate", DateTime.Now);
                                command2.Parameters.AddWithValue("@Name", name[0]);
                                command2.Parameters.AddWithValue("@Surname", name[1]);
                                command2.Parameters.AddWithValue("@ItemId", Convert.ToInt32(itemId));
                                command2.Parameters.AddWithValue("@ItemName", label7.Text);
                                command2.Parameters.AddWithValue("@ItemPrice", label9.Text);

                                command2.Connection.Open();
                                command2.ExecuteNonQuery();
                                command2.Connection.Close();
                            }

                            List<Label> Labels = new List<Label>();
                            Labels.Add(MyLabel.SetOKLabel("Succesfull Payment"));
                            Labels.Add(MyLabel.SetOKLabel("Item " + label7.Text + " sold successfully." +
                                " Generated Receipt."));

                            method.UpdateItemCount(Convert.ToInt32(itemId));
                            sport.Write("#SUCCS: Item Sold! Price is:"+ label9.Text +"KM\n");
                            List<Button> Buttons = new List<Button>();
                            Buttons.Add(MyButton.SetOKThemeButton());
                            MyMessageBox.Show(
                                Labels,
                                "",
                                Buttons,
                                MyImage.SetSuccess());

                            label10.Text = (Convert.ToInt32(label10.Text) - 1).ToString();
                            if (Convert.ToInt32(label10.Text) == 0)
                            {
                                label10.Text = "OUT OF STOCK";
                            }

                            DataTable dt = new DataTable();
                            dt.Columns.Add("Receipt Information");
                            dt.Columns.Add("Receipt Data");

                            DataRow dRow = dt.NewRow();
                            dRow["Receipt Information"] = "Member Id";
                            dRow["Receipt Data"] = label12.Text;
                            dt.Rows.Add(dRow);
                            dRow = dt.NewRow();
                            dRow["Receipt Information"] = "Member Name";
                            dRow["Receipt Data"] = name[0] + " " + name[1];
                            dt.Rows.Add(dRow);
                            dRow = dt.NewRow();
                            dRow["Receipt Information"] = "Item Id";
                            dRow["Receipt Data"] = itemId;
                            dt.Rows.Add(dRow);
                            dRow = dt.NewRow();
                            dRow["Receipt Information"] = "Item Name";
                            dRow["Receipt Data"] = label7.Text;
                            dt.Rows.Add(dRow);
                            dRow = dt.NewRow();
                            dRow["Receipt Information"] = "Item Price";
                            dRow["Receipt Data"] = label9.Text;
                            dt.Rows.Add(dRow);
                            GenerateReceipt(dt);

                            Task ignoredAwaitableResult;
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
            else
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel("Missing user and/or item to sell."));
                sport.Write("#ERROR: Missing user/item to sell!\n");
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
            imgs.ImageSize = new Size(150, 150);

            try
            {

                listView1.LargeImageList = imgs;

                for (int i = 0; i < items.Count; i++)
                {
                    System.Drawing.Image img = new Bitmap(System.Drawing.Image.FromFile(items[i].ItemUrl));
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        g.DrawRectangle(Pens.Black, 0, 0, img.Width - 2, img.Height - 2);
                    }
                    imgs.Images.Add(img);
                    listView1.Items.Add(items[i].ItemId.ToString(), "", i);
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
                    //SELECT TOP 1 * FROM Measurement WHERE MemberId=1 ORDER BY Timestamp DESC;
                    using (SqlCommand cmd2 = new SqlCommand("SELECT id, ItemName, ItemId, ItemPrice, ItemCount, ItemDescription" +
                        " FROM Items WHERE ItemId = " + selected, connection))
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.Connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();

                        while (dr.Read())
                        {
                            itemId = dr["id"].ToString();
                            string itemname = dr["ItemName"].ToString();
                            string itemid = dr["ItemId"].ToString();
                            string itemprice = dr["ItemPrice"].ToString();
                            string itemcount = dr["ItemCount"].ToString();
                            string itemdesc = dr["ItemDescription"].ToString();

                            label7.Text = itemname;
                            label8.Text = itemid;
                            label9.Text = itemprice;
                            label10.Text = itemcount;
                            if (Convert.ToInt32(label10.Text) == 0)
                            {
                                label10.Text = "OUT OF STOCK";
                            }
                            label11.Text = itemdesc;
                            this.button3.Visible = true;
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

        public void getItems()
        {
            items.Clear();
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("SELECT id, ItemName, ItemId, ItemPrice, ItemCount, ItemDescription" +
                        " FROM Items", connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = Convert.ToInt32(dr["id"]);
                            string name = dr["ItemName"].ToString();
                            int itemid = Convert.ToInt32(dr["Itemid"]);
                            decimal itemprice = Convert.ToDecimal(dr["ItemPrice"]);
                            int itemcount = Convert.ToInt32(dr["ItemCount"]);
                            string itemdesc = dr["ItemDescription"].ToString();


                            string itemurl = path.Trim() + "Images\\shopitems\\" + itemid.ToString() + ext;

                            FullItem newItem = new FullItem();

                            newItem.id = id;
                            newItem.ItemName = name;
                            newItem.ItemId = itemid;
                            newItem.ItemPrice = itemprice;
                            newItem.ItemCount = itemcount;
                            newItem.ItemDescription = itemdesc;
                            newItem.ItemUrl = itemurl;
                            items.Add(newItem);
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

        public void GenerateReceipt(DataTable ExDataTable)
        {
            Document pdfDoc = new Document(PageSize.A5, 10, 10, 10, 10);

            try
            {
                string s = DateTime.Today.ToString("dddd, dd MMMM yyyy");
                string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                PdfWriter.GetInstance(pdfDoc, new FileStream(wanted_path + "//Reports//Receipt_" + s + ".pdf", FileMode.Create));
                pdfDoc.Open();

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(new System.Uri(wanted_path + "//Images//logo//logo.png"));
                image.ScaleAbsolute(35, 35);
                image.Alignment = Element.ALIGN_CENTER;

                Paragraph guid = new Paragraph("Receipt ID: " + Guid.NewGuid().ToString(), new
                                                iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)));

                Paragraph address = new Paragraph("Paromlinska 34, 71000, Sarajevo", new
                                                iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)));

                Paragraph p1 = new Paragraph("Date: " + s, new
                                                iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)));
                Paragraph p2 = new Paragraph("\n");

                Random rnd = new Random();


                Paragraph jib = new Paragraph("JIB: " + rnd.Next(10000000, 99999999).ToString(), new
                                                iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)));

                Paragraph pib = new Paragraph("PIB: " + rnd.Next(10000000, 99999999).ToString(), new
                                                iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)));

                pdfDoc.Add(image);
                pdfDoc.Add(p2);
                pdfDoc.Add(p2);
                pdfDoc.Add(guid);
                pdfDoc.Add(address);
                pdfDoc.Add(p1);
                pdfDoc.Add(jib);
                pdfDoc.Add(pib);

                pdfDoc.Add(p2); pdfDoc.Add(p2);
                iTextSharp.text.Font fnt = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
                DataTable dt = ExDataTable;

                if (dt != null)
                {
                    PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                    PdfPCell PdfPCell = null;

                    for (int rows = 0; rows < dt.Rows.Count; rows++)
                    {
                        if (rows == 0)
                        {
                            for (int column = 0; column < dt.Columns.Count; column++)
                            {
                                PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[column].ColumnName.ToString(), fnt)));
                                PdfTable.AddCell(PdfPCell);
                            }
                        }
                        for (int column = 0; column < dt.Columns.Count; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), fnt)));
                            PdfTable.AddCell(PdfPCell);
                        }
                    }
                    pdfDoc.Add(PdfTable);
                }

                pdfDoc.Add(p2);
                double pdvAmount = (Convert.ToDouble(label9.Text) / 100 * 14);
                double osnEAmount = Convert.ToDouble(label9.Text) - pdvAmount;
                Paragraph pdv = new Paragraph("PDV 17,00%: " + osnEAmount + " KM", fnt);
                Paragraph osne = new Paragraph("Osn E: " + pdvAmount + " KM", fnt);
                Paragraph p4 = new Paragraph("Total Amount: " + label9.Text + " KM", fnt);
                pdfDoc.Add(pdv);
                pdfDoc.Add(osne);
                pdfDoc.Add(p2);
                pdfDoc.Add(p4);
                p4.Alignment = Element.ALIGN_CENTER;
                Paragraph blank = new Paragraph("\n");
                for (int i = 0; i <= 5; i++)
                {
                    pdfDoc.Add(blank);
                }

                Paragraph gymmembershipsystem = new Paragraph("Arnold's Gym", fnt);
                Paragraph CEOandOwner = new Paragraph("CEO And Owner:", fnt);
                Paragraph Arnold = new Paragraph("Arnold Schwarzenegger", fnt);
                Paragraph spaces = new Paragraph("______________________________", fnt);

                pdfDoc.Add(gymmembershipsystem);
                pdfDoc.Add(CEOandOwner);
                pdfDoc.Add(Arnold);
                pdfDoc.Add(spaces);
                pdfDoc.Close();
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
                sport.Close();

                ShopItemForm formDialog = new ShopItemForm("0");
                formDialog.ShowDialog();
                sport.Open();
                getItems();

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
            try
            {
                sport.Close();

                ShopItemForm formDialog = new ShopItemForm(label8.Text);
                formDialog.ShowDialog();
                sport.Open();
                getItems();

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
