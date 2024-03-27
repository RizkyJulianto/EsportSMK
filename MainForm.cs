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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            tampildata();
        }
        SqlConnection conn = new SqlConnection("data source = DESKTOP-18L8S2S; initial catalog = EsemkaEsport; integrated security = true");

        private void tampildata()
        {
            SqlCommand command = new SqlCommand("SELECT schedule.id, schedule.home_team_id, schedule.away_team_id, team_home.team_name AS HomeTeam, team_away.team_name AS AwayTeam, schedule.time AS Time " +
                                                "FROM schedule " +
                                                "INNER JOIN team AS team_home ON schedule.home_team_id = team_home.id " +
                                                "INNER JOIN team AS team_away ON schedule.away_team_id = team_away.id ", conn);
            command.CommandType = CommandType.Text;
            conn.Open();
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


                DateTime originalTime = (DateTime)row["Time"];
                string format = originalTime.ToString("dddd, dd MMMM yyyy (HH:mm)");

                row["FormattedTime"] = format;
            }

            dataGridView1.DataSource = dt;
            dt.Columns["FormattedTime"].ColumnName = "MatchTime";
            dataGridView1.Columns["HomeTeam"].Visible = false;
            dataGridView1.Columns["home_team_id"].Visible = false;
            dataGridView1.Columns["away_team_id"].Visible = false;
            dataGridView1.Columns["AwayTeam"].Visible = false;
            dataGridView1.Columns["time"].Visible = false;
            dataGridView1.Columns["id"].Visible = false;
            


        }
        private  void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dataGridView1.Columns["Book"].Index && e.RowIndex >= 0)
            {
                string homeTeam = dataGridView1.Rows[e.RowIndex].Cells["home_team_id"].Value.ToString();
                string awayTeam = dataGridView1.Rows[e.RowIndex].Cells["away_team_id"].Value.ToString();
                string time = dataGridView1.Rows[e.RowIndex].Cells["time"].Value.ToString();
                string schedule_id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();


                BookForm bf = new BookForm(homeTeam, awayTeam, time, schedule_id);
                bf.Show();
                this.Hide();
            }

          
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            button.Name = "Book";
            button.Text = "Book";
            button.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(button);

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 1;

              
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyTicketForm myTicketForm = new MyTicketForm();
            myTicketForm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin logout", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(konfirmasi == DialogResult.Yes)
            {
                LoginForm lf = new LoginForm();
                lf.Show();
                this.Hide();
            }

        
        }
    }
}
