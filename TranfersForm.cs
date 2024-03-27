using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulasiEsport
{
    public partial class TranfersForm : Form
    {
        public TranfersForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndexChanged -= comboBox1_SelectedIndexChanged;

            tampilcombo();
            tampildataFree();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        SqlConnection conn = UserID.conn;

        private void tampilcombo()
        {
            SqlCommand command = new SqlCommand("SELECT id, team_name FROM team", conn);
            command.CommandType = CommandType.Text;
            conn.Open();
            SqlDataAdapter reader = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            reader.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "team_name";
            comboBox1.ValueMember = "id";
            conn.Close();

            comboBox1.SelectedIndex = 0;
        }

        private void tampildataFree()
        {
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT id, nick_name FROM player WHERE id NOT IN (SELECT player_id FROM team_detail)", conn);
            command.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            conn.Close();


            dataGridView1.DataSource = dt;
            dataGridView1.Columns["id"].Visible = false;
        }
        

        private void TranfersForm_Load(object sender, EventArgs e)
        {

        }

        private void tampildatatim()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                return;
            }

            try
            {
                conn.Open();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                SqlCommand command = new SqlCommand("SELECT player.nick_name, player.id " +
                " FROM team_detail " +
                " INNER JOIN player ON team_detail.player_id = player.id " + " WHERE team_id = " + comboBox1.SelectedValue.ToString(), conn);
                command.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                SqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);
                conn.Close();


                dataGridView2.DataSource = dt;
                dataGridView2.Columns["id"].Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tampildatatim();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            var row = dataGridView1.CurrentRow;
            string playerid = row.Cells["id"].Value.ToString();

            SqlCommand command = new SqlCommand("INSERT INTO team_detail (team_id, player_id, created_at) VALUES (@team_id,@player_id,@created_at)", conn);
            command.CommandType = CommandType.Text;
            conn.Open();
            command.Parameters.AddWithValue("team_id", comboBox1.SelectedValue);
            command.Parameters.AddWithValue("player_id",  playerid);
            command.Parameters.AddWithValue("created_at", DateTime.Now);
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("berhasil", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            tampildatatim();
            tampildataFree();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            var row = dataGridView2.CurrentRow;
            string playerid = row.Cells["id"].Value.ToString();

            SqlCommand command = new SqlCommand("DELETE FROM team_detail WHERE team_id = @team_id AND player_id = @player_id", conn);
            command.CommandType = CommandType.Text;
            conn.Open();
            command.Parameters.AddWithValue("team_id", comboBox1.SelectedValue);
            command.Parameters.AddWithValue("player_id", playerid);
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("berhasil", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            tampildatatim();
            tampildataFree();
        }
    }
}
