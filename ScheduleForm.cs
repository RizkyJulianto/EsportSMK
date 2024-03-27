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
    public partial class ScheduleForm : Form
    {
    
        public ScheduleForm()
        {
            InitializeComponent();
            tampildata();
        }

        SqlConnection conn = new SqlConnection("data source = DESKTOP-18L8S2S; initial catalog = EsemkaEsport; integrated security = true");

        private void tampildata()
        {
         
        }

        private void ScheduleForm_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column  in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                column.FillWeight = 1;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin logout?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(konfirmasi == DialogResult.Yes)
            {

                LoginForm lf = new LoginForm();
                lf.Show();
                this.Hide();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Addplayer ad = new Addplayer();
            ad.Show();
            this.Hide();    
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            TeamForm tf = new TeamForm();
            tf.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           AddSchedule addSchedule = new AddSchedule(); 
           addSchedule.ShowDialog();
            
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            Addplayer addplayer = new Addplayer();
            addplayer.Show();
            this.Hide();
        }
    }
}
