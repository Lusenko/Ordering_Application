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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            this.PassField.AutoSize = false;
            this.PassField.Size = new Size(this.PassField.Width,33);
            PassField.Text = "";
            PassField.PasswordChar = '*';
            PassField.MaxLength = 15;

        }
        Point lastPoint;
        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

       

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            String loginUser = LoginField.Text;
            String passUser = PassField.Text;

            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL AND `pass` = @uP",db.GetConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.Show();
            }
            else
                MessageBox.Show("Неверный логин или пароль!");
        }

        private void pictureBoxEye_Click(object sender, EventArgs e)
        {
            if (pictureBoxEye.Visible)
            {
                pictureBoxEye.Visible = false;
                pictureBoxEyeC.Visible = true;
                PassField.UseSystemPasswordChar = true;
            }
        }
        private void pictureBoxEyeC_Click(object sender, EventArgs e)
        {
            if (pictureBoxEyeC.Visible)
            {
                pictureBoxEyeC.Visible = false;
                pictureBoxEye.Visible = true;
                PassField.UseSystemPasswordChar = false;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(374, 488);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "Login";
        }
    }
}
