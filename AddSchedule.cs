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
    public partial class AddSchedule : Form
    {
        public AddSchedule()
        {
            InitializeComponent();
            tampildataHome();
            tampildataAway();
        }

        private void tampildataAway()
        {
            SqlCommand cmd = new SqlCommand("SELECT id,team_name FROM team", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "team_name";
            comboBox2.ValueMember = "id";

            conn.Close();
            comboBox2.SelectedIndex = 0;
        }

        private void tampildataHome()
        {
            SqlCommand cmd = new SqlCommand("SELECT id ,team_name FROM team" ,conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter dr = new SqlDataAdapter(cmd);
            dr.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "team_name";
            comboBox1.ValueMember = "id";

            conn.Close();
            comboBox1.SelectedIndex = 0;
        }

        SqlConnection conn = UserID.conn;

        private void AddSchedule_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO schedule (home_team_id, away_team_id,time,created_at) VALUES(@home_team_id,@away_team_id,@time,@created_at)", conn);
                command.CommandType = CommandType.Text;
                conn.Open();
                command.Parameters.AddWithValue("home_team_id", comboBox1.SelectedValue);
                command.Parameters.AddWithValue("away_team_id", comboBox2.SelectedValue);
                command.Parameters.AddWithValue("time", dateTimePicker1.Value);
                command.Parameters.AddWithValue("created_at", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Berhasil menambahkan schedule baru", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
