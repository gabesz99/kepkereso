using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.IO;
using System.Web.Mail;
using System.Net.Mail;

using Limilabs.Client.IMAP;
using Limilabs.Mail;


namespace Mail_felismero
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<string> subject = new List<string>();
        private List<string> subject2 = new List<string>();
        private List<string> textData = new List<string>();
        private List<string> Tartalma = new List<string>();
        private List<int> fizetos = new List<int>();
        private List<int> listaIndexlista= new List<int>();
        private void button1_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.jelszo == "")
            {
                Properties.Settings.Default.jelszo = Convert.ToBase64String(Encoding.Unicode.GetBytes(textBox2.Text));
            }
            if (Properties.Settings.Default.emailcim == "")
            {
                Properties.Settings.Default.emailcim = Convert.ToBase64String(Encoding.Unicode.GetBytes(textBox3.Text));
            }
            //string kod = "dwBpADIAaQA4AG4ALAAuAA==";
            string jelszo = Encoding.Unicode.GetString(Convert.FromBase64String(Properties.Settings.Default.jelszo));
            string emailcim = Encoding.Unicode.GetString(Convert.FromBase64String(Properties.Settings.Default.emailcim));
            //textBox1.Text +="\n"+ Convert.ToBase64String(Encoding.Unicode.GetBytes(textBox1.Text));
            

            using(Imap imap = new Imap() )
            {
                //MessageBox.Show(jelszo);
                bool betoltve = false;
                
                try
                {
                    imap.ConnectSSL("imap.gmail.com");
                    imap.Login(emailcim, jelszo);
                    imap.SelectInbox();
                    List<long> lista = imap.SearchFlag(Flag.All);
                    
                    int listaindex=0;
                    int index = 0;
                    foreach (long uids in lista)
                    {
                        
                        if (lista.Count - 20 < index)
                        {
                            string eml = imap.GetMessageByUID(uids);
                            IMail email = new MailBuilder().CreateFromEml(eml);

                            
                            if (email.Subject == "Please purchase Mail.dll license at http://www.limilabs.com/mail")
                            {
                                listaIndexlista.Add(listaindex);
                                fizetos.Add(index);
                            }
                            subject.Add(email.Subject.ToString());
                            listaindex++;
                            /*if (email.TextData != null)
                                textData.Add(email.TextDataString);
                            if (email.Text != null)
                                Tartalma.Add(email.Text);*/
                        }
                        index++;
                        betoltve=true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nem jó az email cím vagy a jelszó!!!" + Environment.NewLine + ex.Message);
                }
                imap.Close(true);
                //if(betoltve) MessageBox.Show("Betöltve");
            }
            /*Imap client = new Imap(); // add references System.Web
            // connect to server

            client.Connect("imap.gmail.com", 993, SslMode.Implicit);

            // authenticate
            client.Login("username", "password");

            // select folder
            client.SelectFolder("Inbox");

            int NoOfEmailsPerPage = 10;
            int totalEmails = client.CurrentFolder.TotalMessageCount;
            // get message list - envelope headers
            ImapMessageCollection messages = client.GetMessageList(ImapListFields.Envelope);

            // display info about each message
            foreach (ImapMessageInfo message in messages)
            {
                TableCell noCell = new TableCell();
                noCell.CssClass = "emails-table-cell"
                noCell.Text = Convert.ToString(message.To);
                TableCell fromCell = new TableCell();
                fromCell.CssClass = "emails-table-cell";
                fromCell.Text = Convert.ToString(message.From);
                TableCell subjectCell = new TableCell();
                subjectCell.CssClass = "emails-table-cell";
                subjectCell.Style["width"] = "300px";
                subjectCell.Text = Convert.ToString(message.Subject);
                TableCell dateCell = new TableCell();
                dateCell.CssClass = "emails-table-cell";
                if (message.Date.OriginalTime != DateTime.MinValue)
                    dateCell.Text = message.Date.OriginalTime.ToString();
                TableRow emailRow = new TableRow();
                emailRow.Cells.Add(noCell);
                emailRow.Cells.Add(fromCell);
                emailRow.Cells.Add(subjectCell);
                emailRow.Cells.Add(dateCell);
                EmailsTable.Rows.AddAt(2 + 0, emailRow);
            }
            int totalPages;
            int mod = totalEmails % NoOfEmailsPerPage;
            if (mod == 0) totalPages = totalEmails / NoOfEmailsPerPage;
            else totalPages = ((totalEmails - mod) / NoOfEmailsPerPage) + 1;*/
            lefut2();
        }
        private void lefut2()
        {
            string jelszo = Encoding.Unicode.GetString(Convert.FromBase64String(Properties.Settings.Default.jelszo));
            string emailcim = Encoding.Unicode.GetString(Convert.FromBase64String(Properties.Settings.Default.emailcim));
            using (Imap imap2 = new Imap())
            {
                //MessageBox.Show(jelszo);
                bool betoltve = false;

                try
                {
                    imap2.ConnectSSL("imap.gmail.com");
                    imap2.Login(emailcim, jelszo);
                    imap2.SelectInbox();
                    List<long> lista2 = imap2.SearchFlag(Flag.All);
                    int i = 0;
                    int index2 = 0;
                    foreach (long uids in lista2)
                    {
                        
                        
                        foreach (int rosszIndex in fizetos)
                        {
                            if (rosszIndex == index2)
                            {
                                string eml2 = imap2.GetMessageByUID(uids);
                                IMail email2 = new MailBuilder().CreateFromEml(eml2);

                                subject[listaIndexlista[i]] = email2.Subject.ToString();
                                i++;
                                subject2.Add(email2.Subject.ToString());
                                /*if (email.TextData != null)
                                    textData.Add(email.TextDataString);
                                if (email.Text != null)
                                    Tartalma.Add(email.Text);*/
                            }
                        }
                        index2++;
                        betoltve = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nem jó az email cím vagy a jelszó!!!" + Environment.NewLine + ex.Message);
                }
                imap2.Close(true);
                if (betoltve) MessageBox.Show("Betöltve");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            foreach(string szoveg in subject){
                textBox1.Text += szoveg + "\r\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            foreach (int szam in fizetos)
            {
                textBox1.Text += szam.ToString() + "\r\n";
            }
            textBox1.Text += "----------------------------------------" + "\r\n";
            foreach (int szam2 in listaIndexlista)
            {
                textBox1.Text += szam2.ToString() + "\r\n";
            }
            textBox1.Text += "----------------------------------------" + "\r\n";
            foreach (string szoveg in subject2)
            {
                textBox1.Text += szoveg + "\r\n";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.emailcim = "";
            Properties.Settings.Default.jelszo = "";
            textBox1.Clear();
            foreach (string szoveg in Tartalma)
            {
                textBox1.Text += szoveg + "\r\n";
            }
        }
    }
}
