using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Threading;

using System.Diagnostics;

using System.IO;
using System.Reflection; // a File.exist -hez



namespace Beszed_felismero
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> mondott = new List<string>();
        List<string> metodusok = new List<string>();
        List<string> parameterek = new List<string>();

        SpeechSynthesizer beszel = new SpeechSynthesizer();
        PromptBuilder utasit = new PromptBuilder();
        SpeechRecognitionEngine felismero = new SpeechRecognitionEngine();
        SpeechRecognitionEngine felismero2 = new SpeechRecognitionEngine();

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private bool parancs = false;
        private bool zeneparancs = false;
        private bool zeneMegy = false;
        private void felismero_felismer(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text)
            {
                case "exit": rendszerParancsok("exit");
                    break;
                case "sarah": textBox1.Text += " " + e.Result.Text.ToString();
                    utasit.ClearContent();
                    utasit.AppendText("I am ready!");
                    beszel.Speak(utasit);
                    parancs = true;
                    timer1.Stop();
                    index = 0;
                    label1.Visible = false;
                    timer1.Start();
                    break;
                case "open chrome": chromeParancsok("open");
                    break;
                case "OK": rendszerParancsok("OK");
                    break;
                case "stop": textBox1.Text += " " + e.Result.Text.ToString();
                    parancs = false;
                    label1.Visible = true;
                    break;
                case "tabulator": rendszerParancsok("tab");
                    break;
                case "exit from windows": rendszerParancsok("exWin");
                    break;
                case "minimize": this.WindowState = FormWindowState.Minimized;
                    break;
                case "show window": this.WindowState = FormWindowState.Normal;
                    break;
                case "other window": rendszerParancsok("altTab");
                    break;
                case "close window": rendszerParancsok("close");
                    break;
                case "close chrome": chromeParancsok("close");
                    break;
                case "open notepad": rendszerParancsok("notepad");
                    break;
                case "open google": chromeParancsok("google");
                    break;
                case "open facebook": chromeParancsok("facebook");
                    break;
                case "other tab": chromeParancsok("other tab");
                    break;
                case "close tab": chromeParancsok("close tab");
                    break;
                case "right": rendszerParancsok("right");
                    break;
                case "left": rendszerParancsok("left");
                    break;
                case "music": melyikzenet();
                    break;
                case "classic": zenek("classic");
                    break;
                case "rock": zenek("rock");
                    break;
                default: textBox1.Text += " " + e.Result.Text.ToString();
                    break;
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //linkeket bele, hang megjelenitése
            Choices lista = new Choices();
            lista.Add(new string[] { "sarah", "open chrome", "exit", "test", "OK", "stop", "tabulator", "exit from windows", "minimize", "show window", "close window", "other window","close chrome", "open notepad","open facebook","open google","other tab","close tab", "right","left", "music","classic", "rock" });
            Grammar gr = new Grammar(new GrammarBuilder(lista));
            try
            {
                felismero.RequestRecognizerUpdate();
                felismero.LoadGrammar(gr);

                felismero.RecognizeAsyncStop();

                felismero.SetInputToDefaultAudioDevice();
                felismero.SpeechRecognized += felismero_felismer;
                felismero.RecognizeAsync(RecognizeMode.Multiple);
                //felismero.Recognize();
                button1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")//ezt tick eseménybe...
            {
                utasit.ClearContent();
                utasit.AppendText(textBox1.Text);
                beszel.Speak(utasit);
                textBox1.Clear();
            }
        }
        public void melyikzenet()
        {
            textBox1.Text += "Music Listening";
            utasit.ClearContent();
            utasit.AppendText("Wich music");
            beszel.Speak(utasit);
            parancs = false;
            zeneparancs = true;
        }
        List<string> mp3k = new List<string>();
        private WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        public void zenek(string zeneszamHely) //MODOSITANI !!! égetett számok!!! zenei listák!!léptetés,némitás,megállitás!!
        {
            textBox1.Text += "Music kiválasztás";
            if (zeneparancs == true)
            {
                mp3k.Clear();
                /*if (zeneszam == "classic"){Process.Start("C:/Users/Public/Music/Sample Music/Sleep Away.mp3");}
                if (zeneszam == "rock"){Process.Start("C:/Users/Public/Music/Sample Music/Kalimba.mp3");}*/
                string[] fileok = Directory.GetFiles(zeneszamHely);
                string[] dir = Directory.GetDirectories(zeneszamHely);
                string[] fileokPlusz;
                List<string> nagylista = new List<string>();

                foreach (string item2 in dir)
                {
                    fileokPlusz = Directory.GetFiles(item2);
                    nagylista.AddRange(fileokPlusz);
                }
                foreach (string item in fileok)
                {
                    if (item.Substring(item.Length - 3, 3) == "mp3")
                    {
                        mp3k.Add(item);
                    }
                }
                foreach (string nagy in nagylista)
                {
                    if (nagy.Substring(nagy.Length - 3, 3) == "mp3")
                    {
                        mp3k.Add(nagy);
                    }
                }
                WMPLib.IWMPPlaylist playlist = wplayer.playlistCollection.newPlaylist("myplaylist");
                WMPLib.IWMPMedia media;
                foreach (string item in mp3k)
                {
                    media = wplayer.newMedia(item);
                    playlist.appendItem(media);
                }
                wplayer.currentPlaylist = playlist;
                wplayer.controls.play();
                zeneMegy = true;
                axWindowsMediaPlayer1.Visible = true;
            }
        }
        public void zeneAllj()
        {
            if (zeneMegy==true)
            {
               wplayer.controls.pause(); 
            }
                
        }
        public void zeneIndit()
        {
            if (zeneMegy == true)
            { wplayer.controls.play(); }
        }
        public void kovZene()
        {
            if (zeneMegy == true)
            { wplayer.controls.next(); }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Process[] localAll = Process.GetProcesses();
            if (localAll.Length > 0)
            {
                for (int i = 0; i < localAll.Length; i++)
                    textBox1.Text+=localAll[i].ProcessName.ToString()+"\r\n";
            }
            else MessageBox.Show("Nincs megnyitva chrome");
            
            /*try //zene lejátszás eleje
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = "D:/Zene/proba.wav";
                player.Load();
                player.PlaySync();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }*/         //zene lejátszás vége

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog megnyit = new OpenFileDialog();
            if (megnyit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ut = megnyit.FileName;
                Process.Start(ut);
            }
            parancs = true;
            MethodInfo mi = this.GetType().GetMethod("chromeParancsok");
            object[] parameter = new object[] { "google" };
            mi.Invoke(this, parameter);
        }
        private int index = 0;
        private void szamol(object sender, EventArgs e)
        {
            index++;
            if (index > 10)
            {
                parancs = false;
                label1.Visible = true;
                timer1.Stop();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string[]> sor = new List<string[]>();
            System.IO.StreamReader sr = new System.IO.StreamReader("Desktop/Sheet1.csv");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] row = line.Split(',');
                sor.Add(row);
            }
            for (int i = 0; i < sor.Count; i++)
            {
                mondott.Add(sor[i][0]);
                metodusok.Add(sor[i][1]);
                parameterek.Add(sor[i][2]);
            }
            Choices valasztek = new Choices();
            valasztek.Add(mondott.ToArray());

            Grammar gr2 = new Grammar(new GrammarBuilder(valasztek));
            try
            {
                felismero2.RequestRecognizerUpdate();
                felismero2.LoadGrammar(gr2);

                felismero2.RecognizeAsyncStop();

                felismero2.SetInputToDefaultAudioDevice();
                felismero2.SpeechRecognized += felismero_felismer2;
                felismero2.RecognizeAsync(RecognizeMode.Multiple);
                //felismero.Recognize();
                button5.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        public void rendszerParancsok(string kapott)
        {
            if (parancs == true)
            {
                textBox1.Text += " " + kapott;
                switch (kapott)
                {
                    case "exit": this.Close();
                        break;
                    case "OK": SendKeys.Send("~"); 
                        break;
                    case "tab": SendKeys.Send("{TAB}"); 
                        break;
                    case "exWin": System.Diagnostics.Process.Start("shutdown", "/s /t 0"); 
                        break;
                    case "altTab": SendKeys.Send("^"+"%" + "{TAB}"); 
                        break;
                    case "close": SendKeys.Send("%" + "{F4}"); 
                        break;
                    case "notepad": string npath = Properties.Settings.Default.notepad_path;
                        if (File.Exists(npath))
                        {
                            Process.Start(npath);
                        }
                        else
                        {
                            OpenFileDialog megnyit = new OpenFileDialog();
                            if (megnyit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                string utn = megnyit.FileName;
                                Properties.Settings.Default.notepad_path = utn.ToString();
                                Properties.Settings.Default.Save();
                                Process.Start(utn);
                            }
                        } 
                        break;
                    case "right": SendKeys.Send("{RIGHT}");
                        break;
                    case "left": SendKeys.Send("{LEFT}");
                        break;
                }
            }
        }
        public void chromeParancsok(string kapott)
        {
            if (parancs == true)
            {
                textBox1.Text += " " + kapott;
                switch (kapott)
                {
                    case "open":string path = Properties.Settings.Default.chrome_path;
                        if (File.Exists(path))
                        {
                            Process.Start(path);
                        }
                        else
                        {
                            OpenFileDialog megnyit = new OpenFileDialog();
                            if (megnyit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                string ut = megnyit.FileName;
                                Properties.Settings.Default.chrome_path = ut.ToString();
                                Properties.Settings.Default.Save();
                                Process.Start(ut);
                            }
                        }
                        break;
                    case "close": Process[] chrome = Process.GetProcessesByName("chrome");
                        if (chrome.Length > 0)
                        {
                            for (int i = 0; i < chrome.Length; i++)
                                chrome[i].Kill();
                        }
                        else MessageBox.Show("Nincs megnyitva chrome"); 
                        break;
                    case "other tab": SendKeys.Send("^" + "{TAB}");  
                        break;//még nincsenek meg
                    case "close tab":SendKeys.Send("^" +"w");
                        break;
                }
            }
        }
        public void weblapNyitas(string cim)
        {
            if (cim !="")
            {
               System.Diagnostics.Process.Start(cim); 
            }
            
        }
        private void felismero_felismer2(object sender, SpeechRecognizedEventArgs e)
        {
            string hallott = e.Result.Text.ToString();
            if (hallott != "sarah" && hallott != "stop" && hallott != "minimize" && hallott != "show window")
            {
                if (parancs==true)
                {
                   int i=0;
                   //MessageBox.Show(kapott);
                    foreach (string szo in mondott)
                    {
                        //textBox1.Text += kapott + " " + szo+Environment.NewLine;
                        if (szo == hallott)
                        {
                            if (metodusok[i].ToString() != "null")
                            {
                                try
                                {
                                    MethodInfo mi = this.GetType().GetMethod(metodusok[i].ToString());
                                    object[] parameter;
                                    if (parameterek[i].ToString() == "null")
                                    {
                                        //textBox1.Text += metodusok[i].ToString() + parameterek[i].ToString();
                                        parameter = null;
                                    }
                                    else parameter = new object[] { parameterek[i].ToString() };
                                    mi.Invoke(this, parameter);
                                }
                                catch(Exception ex){
                                    MessageBox.Show("hiba: " + ex.Message);
                                }
                            }
                            //textBox1.Text = szo + " " + metodusok[i].ToString() + " " + parameterek[i].ToString(); 
                        }
                    i++;
                    } 
                }
            }
            switch (hallott)
            {
                case "sarah":
            
                textBox1.Text += " " + e.Result.Text.ToString();
                
                utasit.ClearContent();
                utasit.AppendText("I am ready!");
                beszel.Speak(utasit);
                    
                parancs = true;
                timer1.Stop();
                index = 0; //??
                label1.Visible = false;
                timer1.Start();
                break;

                case "stop": textBox1.Text += " " + e.Result.Text.ToString();
                parancs = false;
                label1.Visible = true;
                break;

                case "minimize": this.WindowState = FormWindowState.Minimized;
                break;

                case "show window": this.WindowState = FormWindowState.Normal;
                break;
            }
        }
    }
}
