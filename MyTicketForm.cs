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
    public partial class MyTicketForm : Form
    {
        public MyTicketForm()
        {
            InitializeComponent();
            tampildata();
        }
        SqlConnection conn = new SqlConnection("data source = DESKTOP-18L8S2S; initial catalog = EsemkaEsport; integrated security = true");

        private void tampildata()
        {
          
           

            SqlCommand command = new SqlCommand("SELECT  team_home.team_name AS HomeTeam, team_away.team_name AS AwayTeam,schedule.time AS Time, schedule_detail.total_ticket" +
                                                " FROM schedule_detail " +
                                                "INNER JOIN schedule ON schedule_detail.schedule_id = schedule.id " +
                                                "INNER JOIN team AS team_home ON schedule.home_team_id = team_home.id " +
                                                "INNER JOIN team AS team_away ON schedule.away_team_id = team_away.id " +
                                                 "WHERE user_id = @userid", conn);
            command.CommandType = CommandType.Text;
            conn.Open();
            command.Parameters.AddWithValue("@userid", UserID.userid);
            DataTable dt = new DataTable();
            SqlDataReader dr = command.ExecuteReader();
            dt.Load(dr);
            conn.Close();

            dt.Columns.Add("Match", typeof(string));
            dt.Columns.Add("FormattedTime", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                string homeTeam = row["HomeTeam"].ToString();
                string awayTeam = row["AwayTeam"].ToString();
                string match = $"{homeTeam} VS {awayTeam}";


                row["Match"] = match;


                DateTime originalTime = (DateTime)row["time"];
                string format = originalTime.ToString("dddd, dd MMMM yyyy(HH:mm)");

                row["FormattedTime"] = format;
            }

            dataGridView1.DataSource = dt;
            dt.Columns["FormattedTime"].ColumnName = "Match Time";
            dataGridView1.Columns["HomeTeam"].Visible = false;
            dataGridView1.Columns["AwayTeam"].Visible = false; 
            dataGridView1.Columns["time"].Visible = false;

        }

        private void MyTicketForm_Load(object sender, EventArgs e)
        {
            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            link.Name = "Print";
            link.Text = "Print";
            link.UseColumnTextForLinkValue = true;
            dataGridView1.Columns.Add(link);


            foreach (DataGridViewColumn column in dataGridView1.Columns )
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 1;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Print"].Index && e.RowIndex >= 0)
            {
                string homeTeam = dataGridView1.Rows[e.RowIndex].Cells["HomeTeam"].Value.ToString();
                string awayTeam = dataGridView1.Rows[e.RowIndex].Cells["AwayTeam"].Value.ToString();
                string time = dataGridView1.Rows[e.RowIndex].Cells["Match Time"].Value.ToString();
                string tiket = dataGridView1.Rows[e.RowIndex].Cells["total_ticket"].Value.ToString();

                Print p = new Print(homeTeam, awayTeam, time, tiket);
                p.Show();
                this.Hide();

            }


                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm mf = new MainForm();
            mf.Show();
            this.Hide();
        }
    }
}
