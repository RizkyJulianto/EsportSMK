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

namespace SimulasiEsport
{
    public partial class TeamForm : Form
    {
        public TeamForm()
        {
            InitializeComponent();
            tampildata();
            button3.Visible = false;
        }

        SqlConnection conn = UserID.conn;

        private void tampildata()
        {
            SqlCommand cmd = new SqlCommand("SELECT id,team_name,company_name,created_at FROM team", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["created_at"].Visible = false;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 1;
            }   

        }


        private void TeamForm_Load(object sender, EventArgs e)
        {
            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            link.Name = "Edit";
            link.Text = "Edit";
            link.UseColumnTextForLinkValue = true;
            dataGridView1.Columns.Add(link);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO team (team_name,company_name,created_at) VALUES (@team_name,@company_name,@created_at)", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("team_name", textBox1.Text);
                cmd.Parameters.AddWithValue("company_name", textBox2.Text);
                cmd.Parameters.AddWithValue("created_at", DateTime.Now);
                cmd.ExecuteNonQuery();
                conn.Close();
                tampildata();
                MessageBox.Show("Data berhasil ditambahkan", "Information", MessageBoxButtons.OK,MessageBoxIcon.Information);
                clear();
            } catch(Exception ex) {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var row = dataGridView1.CurrentRow;
                int id = Convert.ToInt32(row.Cells["id"].Value);

                SqlCommand cmd = new SqlCommand("UPDATE team SET team_name = @team_name, company_name = @company_name, updated_at = @updated_at WHERE id = @id", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("team_name", textBox1.Text);
                cmd.Parameters.AddWithValue("company_name", textBox2.Text);
                cmd.Parameters.AddWithValue("updated_at", DateTime.Now);
                cmd.ExecuteNonQuery();
                conn.Close();
                tampildata();
                MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
                button3.Visible = false;
                button1.Visible = true;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button1.Visible = false;
            button3.Visible = true;
            textBox1.Text = dataGridView1.CurrentRow.Cells["team_name"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["company_name"].Value.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            button1.Visible = true;
            button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ScheduleForm sf = new ScheduleForm();
            sf.Show();
            this.Hide();
        }
    }
}
