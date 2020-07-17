using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Method;
using Gsort;
using CustomModels;

namespace GymMembershipSystem
{
    public partial class AccountForm : Form
    {
        public AccountForm()
        {
            InitializeComponent();
        }
        int pickerChanged = 0;

        Methods methods = new Methods();

        List<ReducedAccount> accounts = new List<ReducedAccount>();
        SortableBindingList<ReducedAccount> catSortable;
        List<PartialMember> members = new List<PartialMember>();

        private void accountForm_load(object sender, EventArgs e)
        {
            try
            {
                populateMembersInDropdown();

                dateTimePicker1.Value = (DateTime.Now.AddDays(-30));
                populateDataGridView();

                double price = 0;
                for (int i = 0; i < accounts.Count; i++)
                    price = price + accounts[i].Price;
                label5.Text = price.ToString();
                label6.Text = accounts.Count.ToString();
                comboBox1.SelectedIndex = 0;
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (pickerChanged == 1)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                methods.GetReport(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, accounts);
                BindingSource bindingSource = new BindingSource();
                catSortable = new SortableBindingList<ReducedAccount>(accounts);
                bindingSource.DataSource = catSortable;
                dataGridView1.DataSource = bindingSource;
                setComboBoxPrices(accounts, true);
            }
            pickerChanged = 1;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            methods.GetReport(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, accounts);
            BindingSource b = new BindingSource();
            catSortable = new SortableBindingList<ReducedAccount>(accounts);
            b.DataSource = catSortable;
            dataGridView1.DataSource = b;
            setComboBoxPrices(accounts, true);
        }

        public void ExportToPdf(DataTable ExDataTable)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                string s = DateTime.Today.ToString("dddd, dd MMMM yyyy");
                string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                PdfWriter.GetInstance(pdfDoc, new FileStream(wanted_path + "//Reports//Report_" + s + "-" + Guid.NewGuid().ToString() + ".pdf", FileMode.Create));
                pdfDoc.Open();

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(new System.Uri(wanted_path + "//Images//logo//logo.png"));
                image.ScaleAbsolute(35, 35);
                image.Alignment = Element.ALIGN_CENTER;

                Paragraph guid = new Paragraph("Report ID: " + Guid.NewGuid().ToString(), new
                                                iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)));

                Paragraph date = new Paragraph("Date: " + s, new
                                                iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)));

                Paragraph newline = new Paragraph("\n");

                Paragraph dateaccounts = new Paragraph("From " + dateTimePicker1.Value.ToString("dd/MM/yyyy") + " to " + dateTimePicker2.Value.ToString("dd/MM/yyyy"),
                                                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)));

                Paragraph reportGeneratedFor = new Paragraph("Report Generated For: " + comboBox1.SelectedItem, 
                                                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0)));

                pdfDoc.Add(image);
                pdfDoc.Add(newline);
                pdfDoc.Add(newline);
                pdfDoc.Add(guid);
                pdfDoc.Add(date);

                pdfDoc.Add(dateaccounts);
                pdfDoc.Add(reportGeneratedFor);
                pdfDoc.Add(newline); 
                pdfDoc.Add(newline);
                iTextSharp.text.Font fnt = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));
                DataTable dt = ExDataTable;

                if (dt != null)
                {
                    PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                    PdfTable.SetWidths(new int[] { 35, 135, 135, 35, 80, 80, 55 });
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

                pdfDoc.Add(newline);
                pdfDoc.Add(newline);
                Paragraph p4 = new Paragraph("Total Amount: " + label5.Text + " KM", fnt);
                Paragraph p5 = new Paragraph("Number of Payments: " + label6.Text, fnt);
                p4.Alignment = Element.ALIGN_CENTER;
                p5.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(p4);
                pdfDoc.Add(p5);

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

        private void GetReportName(DateTime p, DateTime d, string fullname)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT id, Name, Surname, Price, PaymentDate, ExpirationDate, MemberId" +
                        " FROM Account WHERE PaymentDate>=@p AND PaymentDate<=@d AND (Name+' '+Surname)=@s", connection))
                    {
                        cmd2.Parameters.AddWithValue("@p", p);
                        cmd2.Parameters.AddWithValue("@d", d);
                        cmd2.Parameters.AddWithValue("@s", fullname);
                        cmd2.CommandType = CommandType.Text;

                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = Convert.ToInt32(dr["id"]);
                            string name = dr["Name"].ToString();
                            string surname = dr["Surname"].ToString();
                            int memberid = Convert.ToInt32(dr["MemberId"]);
                            double price = Convert.ToDouble(dr["Price"].ToString());
                            DateTime paymentdate = Convert.ToDateTime(dr["PaymentDate"]);
                            DateTime expirationdate = Convert.ToDateTime(dr["ExpirationDate"]);

                            ReducedAccount singleAccount = new ReducedAccount();

                            singleAccount.id = id;
                            singleAccount.Name = name;
                            singleAccount.Surname = surname;
                            singleAccount.Price = price;
                            singleAccount.PaymentDate = paymentdate;
                            singleAccount.ExpirationDate = expirationdate;
                            singleAccount.MemberId = memberid;

                            accounts.Add(singleAccount);
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

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            if (comboBox1.SelectedIndex == 0)
            {

                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                methods.GetReport(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, accounts);
                BindingSource b = new BindingSource();
                catSortable = new SortableBindingList<ReducedAccount>(accounts);
                b.DataSource = catSortable;
                dataGridView1.DataSource = b;

                dataGridView1.AutoSizeColumnsMode =
        DataGridViewAutoSizeColumnsMode.Fill;
                setComboBoxPrices(accounts, false);
            }
            else
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                GetReportName(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, comboBox1.SelectedItem.ToString());
                BindingSource b = new BindingSource();
                catSortable = new SortableBindingList<ReducedAccount>(accounts);
                b.DataSource = catSortable;
                dataGridView1.DataSource = b;
                dataGridView1.AutoSizeColumnsMode =
        DataGridViewAutoSizeColumnsMode.Fill;
                setComboBoxPrices(accounts, false);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                dt.Columns.Add(col.HeaderText);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            ExportToPdf(dt);

            List<Label> Labels = new List<Label>();
            Labels.Add(MyLabel.SetOKLabel("Generating Report PDF"));
            Labels.Add(MyLabel.SetOKLabel("Passed Succesfully!"));

            List<Button> Buttons = new List<Button>();
            Buttons.Add(MyButton.SetOKThemeButton());
            MyMessageBox.Show(
                Labels,
                "",
                Buttons,
                MyImage.SetSuccess());
        }

        private void setComboBoxPrices(List<ReducedAccount> accounts, bool shouldUpdateComboBoxes)
        {
            double price = 0;
            for (int i = 0; i < accounts.Count; i++)
                price = price + accounts[i].Price;

            label5.Text = price.ToString();
            label6.Text = accounts.Count.ToString();
            if (shouldUpdateComboBoxes == true)
            {
                int c = comboBox1.SelectedIndex;
                comboBox1.SelectedIndex = 0;
                comboBox1.SelectedIndex = c;
            }
        }

        private void populateMembersInDropdown()
        {
            methods.GetMembers(members);
            comboBox1.Items.Add("Gym Members");

            foreach (PartialMember c in members)
            {
                comboBox1.Items.Add(c.Name + " " + c.Surname);
            }
        }

        private void populateDataGridView()
        {
            methods.GetReport(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, accounts);
            BindingSource bindingSource = new BindingSource();
            catSortable = new SortableBindingList<ReducedAccount>(accounts);
            bindingSource.DataSource = catSortable;
            dataGridView1.DataSource = bindingSource;
        }
    }
}
