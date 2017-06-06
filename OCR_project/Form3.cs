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
    public partial class Form3 : Form
    {
        public string name; 
        public Form3(string value, string level)
        {
            InitializeComponent();
            label1.Text = "Hi, " + value + " help me find out your english level !";
            name = value;
        }
        string level;
        
        private void Form3_Load(object sender, EventArgs e)
        {

           
            
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            level = "A1";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            level = "A2";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            level = "B2";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            level = "C1";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            level = "B1";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            level = "C2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string createQuery = @"CREATE TABLE IF NOT EXISTS ["+ name + "]([id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, [level] NVARCHAR(2048) NULL, [eng] NVARCHAR(2048) NULL, [tur] NVARCHAR(2048) NULL)";
            using (SQLiteConnection conn = new SQLiteConnection("data source=database.db"))
                    {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(conn))
                {
                    conn.Open();
                    sqliteCommand.CommandText = createQuery;
                    sqliteCommand.ExecuteNonQuery();
                    sqliteCommand.CommandText = "INSERT INTO " + name + "(level) values('"+ level + "')";
                    sqliteCommand.ExecuteNonQuery();

                }
                conn.Close();
            }
            Form1 form1 = new Form1(name,level);
            form1.Show();
            this.Hide();
            
        }
    }
}
