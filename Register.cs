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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            radioButton1.Checked = false;   
            radioButton2.Checked = false;   
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
            if (textBox2.Text.Length < 6)
            {
                MessageBox.Show("Data password minimal 6 karakter", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (textBox3.Text != textBox2.Text)
            {
                MessageBox.Show("Data password harus sama", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            return true;
        }

        private void Register_Load(object sender, EventArgs e)
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

                    int akunsudahAda = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (akunsudahAda > 0)
                    {
                        MessageBox.Show("Username sudah digunakan", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    } else
                    {
                        SqlCommand command = new SqlCommand("INSERT INTO [user] (username,password,birthdate,gender,Role,created_at) VALUES(@username,@password,@birthdate,@gender,1,@created_at)", conn);
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("username", textBox1.Text);
                        command.Parameters.AddWithValue("password", textBox2.Text);
                        command.Parameters.AddWithValue("birthdate", dateTimePicker1.Value);

                        if (radioButton1.Checked)
                        {
                            command.Parameters.AddWithValue("gender", 1);
                        }
                        else if (radioButton2.Checked)
                        {
                            command.Parameters.AddWithValue("gender", 0);
                        }

                        command.Parameters.AddWithValue("created_at", DateTime.Now);

                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Selamat kamu berhasil melakukan registrasi", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }

                  
                } catch (Exception ex) { 
                    conn.Close();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}
