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
using static System.Windows.Forms.LinkLabel;

namespace SimulasiEsport
{
    public partial class Addplayer : Form
    {
        public Addplayer()
        {
            InitializeComponent();
            tampildata();
            button3.Visible = false;
        }

        private void tampildata()
        {
            SqlCommand cmd = new SqlCommand("SELECT id,name,nick_name,birthdate,created_at FROM player",conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dataTable = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dataTable.Load(dr);

            dataTable.Columns.Add("Birthdate",typeof(string));
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime originalTime = (DateTime)row["birthdate"];
                string format = originalTime.ToString("(dddd, dd MMMM yyyy (HH:mm)");

                row["Birthdate"] = format;
            }


            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns["created_at"].Visible = false;
            dataGridView1.Columns["birthdate"].Visible = false;

         

            foreach (DataGridViewColumn column  in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 1;
            }

            conn.Close();

        }

        SqlConnection conn = new SqlConnection("data source = DESKTOP-18L8S2S; initial catalog = EsemkaEsport; integrated security = true");


        private void Addplayer_Load(object sender, EventArgs e)
        {

            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            link.Name = "Edit";
            link.Text = "Edit";
            link.UseColumnTextForLinkValue = true;
            dataGridView1.Columns.Add(link);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button1.Visible = false;
            button3.Visible = true;
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells["birthdate"].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand nick = new SqlCommand("SELECT COUNT (*) FROM player WHERE nick_name = @nick_name", conn);
                nick.CommandType = CommandType.Text;
                conn.Open();
                nick.Parameters.AddWithValue("nick_name", textBox2.Text);

                int nicksudahada = (int)nick.ExecuteScalar();
                conn.Close();
                if(nicksudahada > 0)
                {
                    MessageBox.Show("NickName sudah ada silakan pilih nickname yang lain", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    return;
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO player (name,nick_name,birthdate,created_at) VALUES (@name,@nick_name,@birthdate,@created_at)", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.Parameters.AddWithValue("nick_name", textBox2.Text);
                cmd.Parameters.AddWithValue("birthdate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("created_at", DateTime.Now);
                cmd.ExecuteNonQuery();
                conn.Close();
                tampildata();
                MessageBox.Show("Data berhasil ditambahkan", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
             } catch(Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            dateTimePicker1.Value = DateTime.Now;

        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            button3.Visible = false;
            button1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var row = dataGridView1.CurrentRow;
                int id = Convert.ToInt32(row.Cells["id"].Value);

                SqlCommand cmd = new SqlCommand("UPDATE player SET name = @name, nick_name = @nick_name, birthdate = @birthdate, updated_at = @updated_at WHERE id = @currentid", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@currentid", id);
                cmd.Parameters.AddWithValue("name", textBox1.Text);
                cmd.Parameters.AddWithValue("nick_name", textBox2.Text);
                cmd.Parameters.AddWithValue("birthdate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("updated_at", DateTime.Now);
                cmd.ExecuteNonQuery();
                conn.Close();
                tampildata();
                MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
                button1.Visible = true;
                button3.Visible = false;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ScheduleForm sf = new ScheduleForm();
            sf.Show();
            this.Hide();
        }
    }
}
