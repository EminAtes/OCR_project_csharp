using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tessnet2;
using Hotkeys;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SQLite;
namespace OCR_project
{
    public partial class Form1 : Form
    {
        private Hotkeys.GlobalHotkey ghk;
        string name,query,level;
        public Form1(string value, string level)
        {
            InitializeComponent();
            ghk = new Hotkeys.GlobalHotkey(Constants.CTRL, Keys.S, this);
            label1.Text = "Hi, " + value;
            name = value;
            this.level = level;
        }
        
        string[] ing = new string[10];
        string[] tur = new string[10];
        int i=0;
        string dropfile;
        private void HandleHotkey()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(1200,300,PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(150,650, Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);
            int advance = 0;
            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save("Screenshot"+i+".png", ImageFormat.Png);
            var image = new Bitmap("Screenshot"+i+".png");
            var ocr = new Tesseract();
            ocr.Init(@"C:/Users/Emin/Desktop/OCR_project/packages/NuGet.Tessnet2.1.1.1/content/Content/tessdata", "eng", false);
            var result = ocr.DoOCR(image, Rectangle.Empty);
            int t = 0;
            int founded = 0;
            foreach (tessnet2.Word word in result)
            {
                
                //textBox1.AppendText(word.Text);
                word.Text = RemoveSpecialCharacters(word.Text);
                word.Text = word.Text.Replace("0", "o");
                word.Text = word.Text.Replace("1", "l");
                word.Text = word.Text.Replace(".","");
                word.Text = word.Text.ToLower();
                using (SQLiteConnection conn = new SQLiteConnection("data source=database.db"))
                {
                    using (SQLiteCommand sqliteCommand = new SQLiteCommand(conn))
                    {
                        conn.Open();
                        string[] levels = new string[] { "A1", "A2", "B1", "B2", "C1", "C2" };
                        
                        
                        for (int i = 0; i < 6; i++)
                        {
                            
                            sqliteCommand.CommandText = "SELECT ing,tur from "+levels[i]+" where ing='" + word.Text + "'";
                            using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                            {
                                int count = 0;
                                while (sqliteReader.Read())
                                {

                                    try
                                    {
                                        ing[t] = sqliteReader[0].ToString();
                                        tur[t] = sqliteReader[1].ToString();
                                    }
                                    catch (Exception )
                                    {

                                        
                                    }
                                    //MessageBox.Show(ing+" "+tur);
                                    count++;
                                }
                                if (count == 1)
                                {
                                    founded = i;
                                    
                                    
                                }
                                
                                
                                for (int j = 0; j < 6; j++)
                                {
                                    if (level == levels[j])
                                        advance = j;
                                }

                                
                            }
                           

                        }
                        if (founded > advance)
                        {
                            try
                            {

                                if (ing[t] != "" && tur[t] != "")
                                {
                                    //MessageBox.Show(word.Text);
                                    query = "INSERT INTO " + name + "(eng, tur) VALUES ('" + ing[t] + "','" + tur[t] + "')";
                                    sqliteCommand.CommandText = query;
                                    //sqliteCommand.Parameters.AddWithValue("@name", name);
                                    //sqliteCommand.Parameters.AddWithValue("@ing", ing);
                                    //sqliteCommand.Parameters.AddWithValue("@tur", tur);
                                    sqliteCommand.ExecuteNonQuery();
                                }
                            }
                            catch (Exception)
                            {

                                
                            }
                            sqliteCommand.CommandText = "DELETE FROM "+name+" where eng=''";
                            sqliteCommand.ExecuteNonQuery();
                        }
                    }
                }

                
                
                t++;
            }

            i++; 

        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Hotkeys.Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }
        


        Form2 form2 = new Form2();

    private void button1_Click(object sender, EventArgs e)
        {
            form2.Show();
            /*foreach (tessnet2.Word word in getText(textBox2.Text))
            {
                textBox1.AppendText(word.Text);
            }*/
          

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            using (SQLiteConnection conn = new SQLiteConnection("data source=database.db"))
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(conn))
                {
                    conn.Open();
                    sqliteCommand.CommandText = "SELECT level from "+ name + "";
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        
                        while (sqliteReader.Read())
                        {
                            
                        }
                        
                    }
                }
            }





            this.MaximizeBox = false;
            notifyIcon1.BalloonTipText = "Application is running on.";
            notifyIcon1.BalloonTipTitle = "OCR_project";
            ghk.Register();
            
            while (File.Exists("Screenshot"+i+".png"))
            {
                File.Delete("Screenshot" + i + ".png");
                i++;
            }
            checkBox1.Checked = true;
            this.AllowDrop = true;
            
            

        }
        
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        string DosyaYolu;
        string DosyaAdi;
        private void button2_Click(object sender, EventArgs e)
        {

            //file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  
            openFileDialog1.Filter = "Jpeg files |*.jpg |Png files |*.png|Bitmap files |*.bmp|Icon files |*.ico |All image files |*.jpg;*.png;*.bmp;*.ico"; //"Excel Files|(*.xlsx, *.xls)|*.xlsx;*.xls";
            openFileDialog1.FilterIndex = 5;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Title = "Sellect image file";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            DosyaYolu = openFileDialog1.FileName;
            DosyaAdi = openFileDialog1.SafeFileName;
            textBox2.Text = DosyaYolu;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox2.Text=="")
            {
                MessageBox.Show("Browse for image! or Drag and Drop image!");
            }
            else
            {
                foreach (tessnet2.Word word in getText(DosyaYolu))
                {
                    textBox1.AppendText(word.Text+" ");
                }
            }
        }
        public System.Collections.Generic.List<Word> getText(string path)
        {

           if(path!="")
            {
                try
                {
                    var image = new Bitmap(path);
                    var ocr = new Tesseract();
                    ocr.Init(@"C:/Users/Emin/Desktop/OCR_project/packages/NuGet.Tessnet2.1.1.1/content/Content/tessdata", "eng", false);
                    var result = ocr.DoOCR(image, Rectangle.Empty);
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
           else
            {
                MessageBox.Show("Invalid file");
                try
                {
                    var image = new Bitmap(path);
                    var ocr = new Tesseract();
                    ocr.Init(@"C:/Users/Emin/Desktop/OCR_project/packages/NuGet.Tessnet2.1.1.1/content/Content/tessdata", "eng", false);
                    var result = ocr.DoOCR(image, Rectangle.Empty);
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                this.AllowDrop = true;
            }
            else
            {
                this.AllowDrop = false;
            }
        }


        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in files)
                {
                    textBox2.Text = filePath;
                }
                
            }
            
        }
        
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
           e.Effect=DragDropEffects.Copy;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void showCapturedİmageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                if (i == 0)
                {
                    MessageBox.Show("There is no captured image");
                    checkBox2.CheckState = CheckState.Unchecked;
                }
                if (i != 0)
                {
                    System.Drawing.Image Screenshot = System.Drawing.Image.FromFile(Application.StartupPath + "/Screenshot" + (i - 1) + ".png");
                    Form4 form4 = new Form4(Screenshot.Height, Screenshot.Width, i,Name);
                    form4.Show();
                }
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5(name);
            form5.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
    }

