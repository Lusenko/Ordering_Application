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
    public partial class AddWork : Form
    {
        Validation val = new Validation();
        string userId = string.Empty;
        public AddWork()
        {
            InitializeComponent();
        }
        public AddWork(string user_id)
        {
            InitializeComponent();
            userId = user_id;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }       
        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void buttonAddWork_Click(object sender, EventArgs e)
        {
            if (whatADeal.Text != "" && priceDeal.Text != "" && dateTimePicker1.Text != "")
            {
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("INSERT INTO `work` (`whatdeal`, `pricedeal`, `day`, `user_id`) VALUES (@deal, @price, @day, @userid)", db.GetConnection());

                if (val.Symbol(whatADeal.Text, 3, 18) == true)
                    command.Parameters.Add("@deal", MySqlDbType.VarChar).Value = whatADeal.Text;
               
                command.Parameters.Add("@price", MySqlDbType.Int32).Value = priceDeal.Text;                

                command.Parameters.Add("@day", MySqlDbType.VarChar).Value = dateTimePicker1.Value.ToString("dd.MM.yyyy");
                command.Parameters.Add("@userid", MySqlDbType.VarChar).Value = userId;

                db.openConnection();

                command.ExecuteNonQuery();

                db.closeConnection();

                this.Hide();
                //MainForm mainForm = new MainForm();
                //mainForm.Show();
            }
            else
                MessageBox.Show("Заполните поля!");
        }
    }
}
