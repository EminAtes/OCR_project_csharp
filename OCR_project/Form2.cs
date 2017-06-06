using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace OCR_project
{
    public partial class Form2 : Form
    {
        public string name;
        string level;
        public Form2()
        {
            InitializeComponent();
           
        }

        int sayac = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (sayac % 2 == 0)
            {
                button2.Location = new Point(button2.Location.X, button2.Location.Y+33);
                textBox3.Visible = true;
                label3.Visible = true;
                button2.Text = "Register";
                textBox2.PasswordChar = '\0';
                this.Height = this.Height + 30;
            }
            else
            {
                button2.Location = new Point(button2.Location.X, button2.Location.Y - 33);
                textBox3.Visible = false;
                label3.Visible = false;
                textBox2.PasswordChar = '*';
                button2.Text = "Login";
                this.Height = this.Height - 30;
            }
            sayac++;
        }
        private void ChangeProp(string file, string author)
        {
            DSOFile.OleDocumentPropertiesClass documentProperties = new DSOFile.OleDocumentPropertiesClass();
            DSOFile.SummaryProperties summaryProperties;
            

            try
            {
                documentProperties.Open(file, false, DSOFile.dsoFileOpenOptions.dsoOptionOpenReadOnlyIfNoWriteAccess);
                documentProperties.Save();

                
            }
            finally
            {
                summaryProperties = null;
                documentProperties = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeProp(@"C:\users\Emin\desktop\text.txt","Emina");
            if(sayac%2==0)
            {
                using (SQLiteConnection conn = new SQLiteConnection("data source=database.db"))
                {
                    using (SQLiteCommand sqliteCommand = new SQLiteCommand(conn))
                    {
                        conn.Open();
                        sqliteCommand.CommandText = "SELECT name from Users where name='"+textBox1.Text+"' and password='"+textBox2.Text + "'";
                        using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                        {
                            int count = 0;
                            while(sqliteReader.Read())
                            {
                                count++;
                            }
                            if(count==1)
                            {
                                Form1 form1 = new Form1(textBox1.Text,level);
                                form1.Show();
                                this.Hide();

                            }
                            if(count ==0)
                            {
                                MessageBox.Show("Chech your information !");
                            }
                        }
                    }
                }






               // Form1 form1 = new Form1();
                //this.Hide();
                //form1.Show();
            } else if(sayac%2==1)
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox2.Text == textBox3.Text)
                {
                    using (SQLiteConnection conn = new SQLiteConnection("data source=database.db"))
                    {
                        using (SQLiteCommand sqliteCommand = new SQLiteCommand(conn))
                        {
                            conn.Open();
                            sqliteCommand.CommandText = "INSERT INTO Users(name,password) values('" + textBox1.Text + "','" + textBox2.Text + "')";
                            sqliteCommand.ExecuteNonQuery();
                            sqliteCommand.CommandText = "SELECT * from Users";
                            using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                            {
                                while (sqliteReader.Read())
                                {
                                    
                                }
                            }
                        }
                    }
                    Form3 form3 = new Form3(textBox1.Text, level);
                    this.Hide();
                    form3.Show();
                }
                else
                    MessageBox.Show("Check your information please.");

            }
            
        }
        public string getName()
        {
            return textBox1.Text;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
    }
}
