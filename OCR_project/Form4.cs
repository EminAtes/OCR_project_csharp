using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OCR_project
{
    public partial class Form4 : Form
    {
        public string name1;
        public Form4(int a,int b, int i, string name)
        {
            InitializeComponent();
            this.Height = a;
            this.Width = b;
            pictureBox1.Height = a;
            pictureBox1.Width = b;
            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\Screenshot" + (i - 1) + ".png");
            name1 = name;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 form1 = new Form1(name1,"A1");
            form1.checkBox2.CheckState = CheckState.Unchecked;
        }
    }
}
