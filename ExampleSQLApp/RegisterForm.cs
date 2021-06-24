using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExampleSQLApp
{
    public partial class RegisterForm : Form
    {
        Validation val = new Validation();
        public RegisterForm()
        {
            InitializeComponent();
        }
        Point lastPoint;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mf = new MainForm();
            mf.Show();
        }
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (userCountryField.Text != "" && userNameField.Text != "" && userPhoneNumberField.Text != ""
                && userFatheNameField.Text != "" && userSurnameField.Text != "" && userVberField.Text != ""
                && userTelegramField.Text != "" && userMailField.Text != "")
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("INSERT INTO `log` (`Name`, `Surname`, `FatherName`," +
                    "`Country`, `Phone number`, `Viber`, `Telegram`, `e-mail`) VALUES (@name, @surname, @fathername," +
                    "@country, @phonenumber, @viber, @telegram, @email)", db.GetConnection());

                if (val.Symbol(userNameField.Text, 3, 18) == true && val.Symbol(userSurnameField.Text, 3, 18) == true
                    && val.Symbol(userFatheNameField.Text, 3, 18) == true && val.Symbol(userCountryField.Text, 3, 18) == true)
                {
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = userNameField.Text;
                    command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = userSurnameField.Text;
                    command.Parameters.Add("@fathername", MySqlDbType.VarChar).Value = userFatheNameField.Text;
                    command.Parameters.Add("@country", MySqlDbType.VarChar).Value = userCountryField.Text;
                }
                if (val.Numer(userPhoneNumberField.Text) == true && val.Numer(userVberField.Text) == true)
                {
                     command.Parameters.Add("@phonenumber", MySqlDbType.VarChar).Value = userPhoneNumberField.Text;
                     command.Parameters.Add("@viber", MySqlDbType.VarChar).Value = userVberField.Text;
                }             
    
                command.Parameters.Add("@telegram", MySqlDbType.VarChar).Value = userTelegramField.Text;

                if (val.Mail(userMailField.Text))
                {
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = userMailField.Text;
                }
                db.openConnection();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    MessageBox.Show("Введіть значення коректно");
                }
                

                db.closeConnection();

                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.Show();
            }
            else
                MessageBox.Show("Заполните поля!");
        }      
    }
}
