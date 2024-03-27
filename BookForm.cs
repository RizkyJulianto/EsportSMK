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
    public partial class BookForm : Form
    {

        private string homeTeam;
        private string awayTeam;
        private string time;
        private string schedule_id;
        public BookForm(string homeTeam, string awayTeam, string time, string schedule_id)
        {
            InitializeComponent();
            this.homeTeam = homeTeam;
            this.awayTeam = awayTeam;
            this.time = time;
            this.schedule_id = schedule_id;

            loadPlayerHome();
            loadPlayerAway();
            loadPlayerHomeAway();
            loadPlayerHomeData();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT team_name FROM team WHERE id = @hometeamid", conn);
            cmd.Parameters.AddWithValue("@hometeamid", homeTeam);
            string hometeam = (string)cmd.ExecuteScalar();

            SqlCommand c = new SqlCommand("SELECT team_name FROM team WHERE id = @awayteamid", conn);
            c.Parameters.AddWithValue("@awayteamid", awayTeam);
            string awayteam = (string)c.ExecuteScalar();

            label1.Text = hometeam;
            label2.Text = awayteam;
            conn.Close();
        }

        private void loadPlayerHomeData()
        {
            DataTable player = loadPlayerHome();
            dataGridView1.Rows.Clear();
            dataGridView1.DataSource = player;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 1;
            }

        }

        private void loadPlayerHomeAway()
        {
            DataTable player = loadPlayerAway();
            dataGridView2.Rows.Clear();
            dataGridView2.DataSource = player;

            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 1;
            }

        }

        SqlConnection conn = new SqlConnection("data source = DESKTOP-18L8S2S; initial catalog = EsemkaEsport; integrated security = true");

        private DataTable loadPlayerHome()
        {
            DataTable player = new DataTable();

            conn.Open();

            SqlCommand ch = new SqlCommand("SELECT team_name FROM team WHERE id = @hometeamid", conn);
            ch.Parameters.AddWithValue("@hometeamid", homeTeam);
            string hometeam = (string)ch.ExecuteScalar();

            SqlCommand command = new SqlCommand("SELECT player.nick_name " + 
                " FROM team_detail " + 
                " INNER JOIN player ON team_detail.player_id = player.id " + 
                " INNER JOIN schedule ON team_detail.team_id IN (schedule.home_team_id, schedule.away_team_id)" +
                " INNER JOIN team AS team_home ON schedule.home_team_id = team_home.id " + 
                " WHERE team_home.team_name = @hometeamid", conn);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@hometeamid", hometeam);
            SqlDataReader dr = command.ExecuteReader();

            player.Columns.Add("NickName", typeof(string));

            if (dr.HasRows)
            {
                while(dr.Read())
                {
                    string nick = dr["nick_name"].ToString();
                    player.Rows.Add(nick);
                }
            }

            dr.Close();
            conn.Close();
            return player;

        }


        private DataTable loadPlayerAway()
        {
            DataTable player = new DataTable();

            conn.Open();

            SqlCommand c = new SqlCommand("SELECT team_name FROM team WHERE id = @awayteamid", conn);
            c.Parameters.AddWithValue("@awayteamid", awayTeam);
            string awayteam = (string)c.ExecuteScalar(); 

            SqlCommand command = new SqlCommand("SELECT player.nick_name " + 
                " FROM team_detail " + 
                " INNER JOIN player ON team_detail.player_id = player.id " + 
                " INNER JOIN schedule ON team_detail.team_id IN (schedule.home_team_id, schedule.away_team_id) " + 
                " INNER JOIN team AS team_away ON schedule.away_team_id = team_away.id " + 
                " WHERE team_away.team_name = @awayteamid", conn);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@awayteamid", awayteam);
            SqlDataReader dr = command.ExecuteReader();

            player.Columns.Add("Nickname", typeof(string));

            if (dr.HasRows)
            {
                while(dr.Read())
                {
                    string nick = dr["nick_name"].ToString();
                    player.Rows.Add(nick);
                }
            }

            dr.Close();
            conn.Close();
            return player;

        }

        private void BookForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mf = new MainForm();
            mf.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int booking = Convert.ToInt32(numericUpDown1.Value);
            int total_tiket = 60;
            int totalbooking = total_tiket - booking;

            label6.Text = totalbooking.ToString() + "Tiket";


            SqlCommand command = new SqlCommand("INSERT INTO schedule_detail(schedule_id,user_id,total_ticket,created_at) VALUES (@schedule_id,@user_id,@total_ticket,@created_at)", conn);
            command.CommandType = CommandType.Text;
            conn.Open();

            command.Parameters.AddWithValue("schedule_id", schedule_id);
            command.Parameters.AddWithValue("user_id", UserID.userid);
            command.Parameters.AddWithValue("total_ticket", booking);
            command.Parameters.AddWithValue("created_at", DateTime.Now);
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("selamat kamu sudah membooking tiket", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clear();
        }

        private void clear()
        {
            numericUpDown1.Value = 0;
        }
    }
}
