using CustomModels;
using GymMembershipSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Method
{
    partial class Methods
    {
        public string getComm()
        {
            using (SqlConnection connection = new SqlConnection(
                global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COMM FROM Settings WHERE Id=1", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    connection.Open();

                    object o = cmd.ExecuteScalar();
                    if (o != null)
                    {
                        return o.ToString();

                    }
                    connection.Close();
                    return "0";
                }
            }

        }

        public bool CheckId(String k)
        {

            if (k == "----------------------------")
                return false;
            Int64 i = Convert.ToInt64(k);
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
                        cmd.Connection.Close();
                        return false;
                    }
                    cmd.Connection.Close();
                    return true;
                }
            }

        }

        public int GetMemberId(String k)
        {
            try
            {
                Int64 i = Convert.ToInt64(k);
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
                            int s = Convert.ToInt32(city);
                            cmd.Connection.Close();
                            return s;
                        }
                        cmd.Connection.Close();

                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel(e.Message));

                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
                return 0;
            }
        }

        public int GetNumberOfEntrances(String k)
        {

            try
            {
                Int64 i = Convert.ToInt64(k);

                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT NumOfEntrances FROM Member WHERE CardId=@CardId", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardId", i);
                        connection.Open();

                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            string id = o.ToString();
                            int idToReturn = Convert.ToInt32(id);
                            return idToReturn;
                        }
                        return 0;
                    }
                }
            }
            catch (Exception e)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel(e.Message));

                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
                return 0;
            }
        }

        public DateTime GetExpirationDate(long k)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT ExpirationDate FROM Account WHERE MemberId=@CardId ORDER BY id DESC;", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CardId", k);
                        connection.Open();

                        object o = cmd.ExecuteScalar();
                        if (o != null)
                        {
                            string id = o.ToString();
                            DateTime d = Convert.ToDateTime(id);
                            connection.Close();
                            return d;
                        }
                        connection.Close();

                        return DateTime.Now;

                    }
                }
            }
            catch (Exception e)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.SetOKLabel("General Error"));
                Labels.Add(MyLabel.SetOKLabel(e.Message));
                List<Button> Buttons = new List<Button>();
                Buttons.Add(MyButton.SetOKThemeButton());
                MyMessageBox.Show(
                    Labels,
                    "",
                    Buttons,
                    MyImage.SetFailed());
                return DateTime.Now;
            }
        }

        public void WriteEntrance(int id, int num, string name, string surname)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Member SET NumOfEntrances" +
                            "=@NumOfEntrances,LastEntrance=@LastEntrance WHERE id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@NumOfEntrances", num);
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@LastEntrance", DateTime.Now);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();

                    }
                    using (SqlCommand command2 = new SqlCommand("INSERT INTO Report(MemberId, EntranceDate, Name, Surname) VALUES(@MemberId, @EntranceDate, @Name, @Surname)", connection))
                    {

                        command2.Parameters.AddWithValue("@MemberId", id);
                        command2.Parameters.AddWithValue("@EntranceDate", DateTime.Now);
                        command2.Parameters.AddWithValue("@Name", name);
                        command2.Parameters.AddWithValue("@Surname", surname);

                        command2.Connection.Open();
                        command2.ExecuteNonQuery();
                        command2.Connection.Close();
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

        public void WriteLastEntrance(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Member SET " +
                            "LastEntrance=@LastEntrance WHERE id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@LastEntrance", DateTime.Now);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
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

        public void GetMembers(List<PartialMember> members)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("SELECT id, Name, Surname, Address, PhoneNumber, CardId, TypeId," +
                        " NumOfEntrances, Gender, LastEntrance FROM Member ORDER BY Surname ASC", connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        SqlDataReader dr = command.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = Convert.ToInt32(dr["id"]);
                            string name = dr["Name"].ToString();
                            string surname = dr["Surname"].ToString();
                            string address = dr["Address"].ToString();
                            string phonenum = dr["PhoneNumber"].ToString();
                            string typeid = dr["TypeId"].ToString();
                            int numofentr = Convert.ToInt32(dr["NumOfEntrances"]);
                            Int64 cardid = Convert.ToInt64(dr["CardId"]);
                            DateTime lastEntrance = Convert.ToDateTime(dr["LastEntrance"]);
                            string gender = dr["Gender"].ToString();

                            DateTime expDate = GetExpirationDate(id);

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
                            newMember.id = id;
                            newMember.NumOfDays = GetNumberOfEntrances(cardid.ToString());
                            members.Add(newMember);
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

        public void GetReport(DateTime p, DateTime d, List<ReducedAccount> accounts)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT id, Name, Surname, Price, PaymentDate, ExpirationDate, MemberId FROM" +
                        " Account WHERE PaymentDate>=@p AND PaymentDate<=@d ", connection))
                    {
                        cmd2.Parameters.AddWithValue("@p", p);
                        cmd2.Parameters.AddWithValue("@d", d);
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

        public void UpdateItemCount(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Items SET " +
                            "ItemCount=ItemCount - 1 WHERE id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
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

        public void UpdateEnrollment(int id) {
            try
            {
                using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Exercise SET " +
                            "Enrolled=Enrolled + 1 WHERE id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
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

        public string checkTrainingEnrollment(string id)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(
                  global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand cmd2 = new SqlCommand("SELECT MemberId, ExerciseId FROM TrainingEnrollment WHERE ExerciseId=@id", connection))
                    {
                        string memberid;
                        cmd2.Parameters.AddWithValue("id", id);
                        cmd2.CommandType = CommandType.Text;

                        connection.Open();
                        SqlDataReader dr = cmd2.ExecuteReader();
                        while (dr.Read())
                        {
                            memberid = dr["MemberId"].ToString();
                            int exerciseId = Convert.ToInt32(dr["ExerciseId"]);
                            return memberid;
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

            return "0";
        }

        public void ReduceEnrollment(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(global::GymMembershipSystem.Properties.Settings.Default.GymMembershipSystemDatabase))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE Exercise SET " +
                            "Enrolled=Enrolled - 1 WHERE id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
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
