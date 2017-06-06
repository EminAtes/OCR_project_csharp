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
    public partial class Form5 : Form
    {
        string name;
        public Form5(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=database.db"))
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(conn))
                {
                    conn.Open();
                    
                    sqliteCommand.CommandText = "SELECT * from "+name;
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {

                        }
                    }
                    SQLiteDataAdapter dataadapter = new SQLiteDataAdapter("SELECT * from "+name,conn);
                    DataSet ds = new System.Data.DataSet();
                    dataadapter.Fill(ds,"Info");
                    dataGridView1.DataSource = ds.Tables[0];
                    conn.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=database.db"))
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(conn))
                {
                    conn.Open();

                    sqliteCommand.CommandText = "SELECT * from " + name;
                    using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        while (sqliteReader.Read())
                        {

                        }
                    }
                    SQLiteDataAdapter dataadapter = new SQLiteDataAdapter("SELECT * from " + name, conn);
                    DataSet ds = new System.Data.DataSet();
                    dataadapter.Fill(ds, "Info");
                    dataGridView1.DataSource = ds.Tables[0];
                    conn.Close();
                }
            }
        }
    }
}
