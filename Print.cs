using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulasiEsport
{
    public partial class Print : Form
    {
        private string homeTeam;
        private string awayTeam;
        private string time;
        private string tiket;

        public Print(string homeTeam, string awayTeam, string time , string tiket)
        {
            InitializeComponent();
            this.homeTeam = homeTeam;
            this.awayTeam = awayTeam;
            this.time = time;
            this.tiket = tiket;

            string html = print();
            webBrowser1.DocumentText = html;
        }

        private string print()
        {
            return "<!DOCTYPE html> <html lang='en'> <head> <meta charset='UTF-8'> <meta http-equiv='X-UA-Compatible' content='IE=edge'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> <title>Document</title> <style> .container { text-align: center; margin-top:70px; } </style> </head> <body>" +
                    $"<div class='container'>  ------------------------------------- <br> <span>{homeTeam} VS {awayTeam }</span><br> <span>Time : {time}</span><br> <span>Total Ticket : {tiket} </span><br>  ------------------------------------- </div> </body> </html>";
        }

        private void Print_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string html = print();
            html += "<script> window.print() </script>";
            webBrowser1.DocumentText = html;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MyTicketForm ticketForm = new MyTicketForm();
            ticketForm.Show();
            this.Hide();
        }
    }
}
