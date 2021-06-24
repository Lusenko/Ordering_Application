using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ExampleSQLApp
{
    public partial class MainForm : Form
    {
        DataTable dt = new DataTable();
        SqlConnection connection = new SqlConnection();
        private string[] array = new string[10];
        public MainForm()
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
            Application.Exit();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void RegisterForm_Click(object sender, EventArgs e)
        {
            RegisterForm rf = new RegisterForm();
            rf.ShowDialog();
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = LoadInfo();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;            

            addButtonToDataGrid(13, "Add", Color.Green);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dateTimePicker1.Visible = true;
            button1.Visible = true;
            FillInfo(e);
            FillDeal(e);
            ew = e;
            
        }

        DataGridViewCellEventArgs ew;
        private void FillInfo(DataGridViewCellEventArgs e)
        {
          
            for (int i = 0; i < 9; i++)
                array[i] = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();

            label8.Text = array[1];
            label9.Text = array[2];
            label10.Text = array[3];
            label11.Text = array[4];
            label12.Text = array[5];
            label13.Text = array[6];
            label14.Text = array[7];
            label15.Text = array[8];
            
            
        }
        private void FillDeal(DataGridViewCellEventArgs e)
        {          
            dt = Deal(e);
            if(dt.Rows.Count == 0)
            {
                comboDeals.DataSource = null;
                dateTimePicker1.Visible = false;
                button1.Visible = false;
                label22.Visible = false;
                label23.Visible = false;
                label24.Visible = false;
                label26.Visible = false;
                
            }
            List<string> arr = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arr.Add(dt.Rows[i]["whatdeal"].ToString());
                          
            }
            if (dt.Rows.Count > 0)
            {
                comboDeals.DataSource = arr;
            }

        }
        private DataTable Deal(DataGridViewCellEventArgs e)
        {
            DataTable DT = new DataTable();
            DB db = new DB();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `work` WHERE `user_id` = @userid", db.GetConnection());
            cmd.Parameters.Add("@userid", MySqlDbType.VarChar).Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            db.openConnection();
            
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(DT);
            db.closeConnection();

            return DT;
        }
        private void addButtonToDataGrid(int dataSize, string text, Color color)
        {
            if (dataGridView1.Columns.Count < dataSize)
            {
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                btn.Text = text;
                btn.UseColumnTextForButtonValue = true;
                btn.FlatStyle = FlatStyle.Flat;
                btn.CellTemplate.Style.BackColor = color;
                btn.CellTemplate.Style.ForeColor = Color.Black;
                dataGridView1.Columns.Add(btn);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
                if (dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() == "Add")
                {
                    AddWork aw = new AddWork(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());                   
                    aw.ShowDialog(this);
                    aw.BringToFront();
                }
        }
        private DataTable LoadInfo()
        {
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `log`", db.GetConnection());

            DataTable DT = new DataTable();
            MySqlDataAdapter ad = new MySqlDataAdapter();
            ad.SelectCommand = command;
            ad.Fill(DT);
            db.closeConnection();
            return DT;
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            Request(maskedTextBox1.Text);
            
        }
        private void Request(string search)
        {
            try
            {
                DataView DV = new DataView(LoadInfo());
                DV.RowFilter = string.Format(comboBox1.Text + " LIKE'" + search + "%'");
                dataGridView1.DataSource = DV;
            }
            catch (SyntaxErrorException)
            {
                MessageBox.Show("Select a field!");
            }
        }

        private void comboDeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(dt.Rows.Count > 0)
            {
                label22.Visible = true;
                label23.Visible = true;
                label24.Visible = true;
                label26.Visible = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["whatdeal"].ToString() == comboDeals.Text)
                    {
                        label22.Text = dt.Rows[i]["whatdeal"].ToString();
                        label23.Text = dt.Rows[i]["pricedeal"].ToString();
                        label24.Text = dt.Rows[i]["day"].ToString();
                                                        
                        if(dt.Rows[i]["finishday"].ToString() == "")
                        {
                            label26.Visible = false;
                        }
                        else
                        {
                            dateTimePicker1.Visible = false;
                            label26.Visible = true;
                            label26.Text = dt.Rows[i]["finishday"].ToString();
                            button1.Visible = false;
                        }
                    }
                }
            }
                       
            if (dateTimePicker1.Visible == false && label26.Visible == false)
            {
                dateTimePicker1.Visible = true;
                button1.Visible = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DB db = new DB();
            string id = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["whatdeal"].ToString() == comboDeals.Text)
                {
                    id = dt.Rows[i]["id_w"].ToString();

                }
            }
            MySqlCommand command = new MySqlCommand("UPDATE `work` SET `finishday`= @finishday WHERE `id_w` = @id", db.GetConnection());
            command.Parameters.Add("@finishday", MySqlDbType.VarChar).Value = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            
            db.openConnection();

            command.ExecuteNonQuery();

            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FillInfo(ew);
            FillDeal(ew);
        }
    }
}
