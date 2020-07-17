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
    public partial class MeasurementHistoryForm : Form
    {
        public MeasurementHistoryForm(string memberId)
        {
            InitializeComponent();
            string MemberId = memberId;

            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    connection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT Timestamp, Height, Weight, BodyFat, Neck, " +
                        "Shoulders, LeftBicep, LeftForearm, RightBicep, RightForearm, Chest, Waist, Hips, LeftThighs, RightThighs," +
                        "LeftCalves, RightCalves FROM Measurement WHERE MemberId='" + MemberId + "' ORDER BY Timestamp DESC", connection);
                    DataTable db = new DataTable();
                    sqlDataAdapter.Fill(db);
                    dataGridView1.DataSource = db;
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataBindingsCompleted(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            int numberOfCells = dataGridView1.Rows[0].Cells.Count;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for(int i = 1; i < numberOfCells; i++) {
                    if (row.Index < (dataGridView1.Rows.Count - 2))
                    {
                        var rowrow = dataGridView1.Rows[row.Index + 1];
                        if (Convert.ToDouble(row.Cells[i].Value) > Convert.ToDouble(rowrow.Cells[i].Value))
                        {
                            row.Cells[i].Style.BackColor = Color.Green;
                        }
                        else if (Convert.ToDouble(row.Cells[i].Value) < Convert.ToDouble(rowrow.Cells[i].Value))
                        {
                            row.Cells[i].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
    }
}
