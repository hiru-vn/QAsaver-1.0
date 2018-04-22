using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF2
{
    public partial class Form3 : Form
    {
        private bool isaccept=false;
        private string topic;
        public Form3(ref string newtopic)
        {
            //the topic you add will can not be deleted at this version. please do not add 
            this.topic = newtopic;
            InitializeComponent();
        }

        public string Topic { get => topic; set => topic = value; }
        public bool Isaccept { get => isaccept; set => isaccept = value; }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult yesOrno = MessageBox.Show("this version do not support delete topic. do you still want to add this topic?", "QA saver", MessageBoxButtons.YesNo);
            if (yesOrno== System.Windows.Forms.DialogResult.Yes)
            {
                this.isaccept = true;
            }
            if (isaccept = true)
            {
                this.Topic = textBox1.Text;
            }
            else
            {
                this.Topic = "";
            }
        }
    }
}
