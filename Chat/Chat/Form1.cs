using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Chat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = Properties.Settings.Default.Nev;
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Nev = textBox3.Text;
            Properties.Settings.Default.Save();
        }
        private MySqlConnection kapcs = new MySqlConnection();
        private string kapcsString2 = ConfigurationManager.ConnectionStrings["chatString"].ToString();
        private StringBuilder olvaso = new StringBuilder();
        string ido = "";
        bool elso = true;
        int id=0;
        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(kapcsString2);
            string nev = textBox3.Text;
            kapcs.ConnectionString = kapcsString2;

            MySqlCommand command = new MySqlCommand();
            command.Connection = kapcs;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO Beszelgetes (uzenet,felado,ido) VALUES(@uzenet,@felado,@ido)" ;
            command.Parameters.Add("@uzenet", MySqlDbType.VarChar, 40).Value=textBox2.Text;
            command.Parameters.Add("@felado", MySqlDbType.VarChar, 40).Value=textBox3.Text;
            command.Parameters.Add("@ido", MySqlDbType.VarChar, 40).Value = DateTime.UtcNow.ToString("HH:mm:ss");//"hh:mm:ss"
            command.Connection.Open();
            //command.ExecuteNonQuery();
            command.ExecuteScalar();

            //textBox1.Text = "sikerült";
            command.Connection.Close();
            //lekerElso();
            textBox2.Clear();
            textBox2.Text = "";
        }

        private void lekerElso(){
            kapcs.ConnectionString = kapcsString2;

            MySqlCommand command = new MySqlCommand();
            command.Connection = kapcs;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT ID,ido,felado,uzenet FROM Beszelgetes";
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                id = int.Parse(reader[0].ToString());
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    olvaso.Append(reader[i].ToString() + '\t');
                    
                }
                olvaso.Append(Environment.NewLine);
                ido = reader[0].ToString();
            }
            command.Connection.Close();
            reader.Close();
            //olvaso.Append("Masodik");
            textBox1.Text = olvaso.ToString();
            //textBox2.Text = ido;
            //textBox1.ScrollToCaret();
            textBox1.SelectionStart = textBox1.TextLength;
            textBox1.ScrollToCaret();
            //maxid();
            textBox2.Focus();
        }
        private void lekerFolyamat()
        {
            kapcs.ConnectionString = kapcsString2;

            MySqlCommand command = new MySqlCommand();
            command.Connection = kapcs;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT id,ido,felado,uzenet FROM Beszelgetes WHERE id>"+id.ToString();
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                id = int.Parse(reader[0].ToString());
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    olvaso.Append(reader[i].ToString() + '\t');

                }
                olvaso.Append(Environment.NewLine);
                ido = reader[0].ToString();
            }
            command.Connection.Close();
            reader.Close();
            //olvaso.Append("kövi");
            textBox1.Text = olvaso.ToString();
            textBox1.SelectionStart = textBox1.TextLength;
            textBox1.ScrollToCaret();
            //maxid();
        }
        private void ora(object sender, EventArgs e)
        {
            if (elso)
            {
                lekerElso();
                //MessageBox.Show("elso");
                elso = false;
            }
            else
            {
                //MessageBox.Show("Masodik");
                lekerFolyamat();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lekerFolyamat();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            olvaso.Clear();
        }
        private void maxid()
        {
            kapcs.ConnectionString = kapcsString2;

            MySqlCommand command = new MySqlCommand();
            command.Connection = kapcs;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT max(id) FROM Beszelgetes ";
            command.Connection.Open();
            id = (int)command.ExecuteScalar();
            command.Connection.Close();
        }
        private void enter(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{BACKSPACE}");
                button1.PerformClick();
                textBox2.Clear();
                textBox2.Text = "";
            }
        }
    }
}
