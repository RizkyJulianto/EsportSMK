using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulasiEsport
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("data source = DESKTOP-18L8S2S; initial catalog = EsemkaEsport; integrated security = true");

        private bool validasi()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Data username tidak boleh kosong", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Data password tidak boleh kosong", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (validasi())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT (*) FROM [user] WHERE username = @username", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.Parameters.AddWithValue("username", textBox1.Text);

                    int akunBelumada = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (akunBelumada == 0)
                    {
                        MessageBox.Show("Username tidak dapat ditemukan", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                      
                        SqlCommand command = new SqlCommand("SELECT * FROM [user] WHERE username = @username AND password = @password", conn);
                        command.CommandType = CommandType.Text;
                        conn.Open();
                        command.Parameters.AddWithValue("username", textBox1.Text);
                        command.Parameters.AddWithValue("password", textBox2.Text);
                        command.ExecuteNonQuery();
                        SqlDataReader dr = command.ExecuteReader();
                        if (dr.Read())
                        {
                            int role = Convert.ToInt32(dr["role"]);
                            UserID.userid = Convert.ToInt32(dr["id"]);
                            if (role == 1)
                            {
                                MessageBox.Show("Selamat kamu berhasil login", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                MainForm mf = new MainForm();
                                mf.Show();
                                this.Hide();
                            } else if(role == 0)
                            {
                                MessageBox.Show("Selamat datang admin", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ScheduleForm sf = new ScheduleForm();
                                sf.Show();
                                this.Hide();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Login gagal harap periksa username dan password anda", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }


                } catch(Exception ex)
                {
                    conn.Close();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                }

        }

            private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            } else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register r = new Register();
            r.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
