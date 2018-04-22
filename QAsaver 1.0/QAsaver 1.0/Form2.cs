using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//question automatically save when table closed. last updated: 1/1/2001 12:31
namespace WF2
{
    public partial class Form2 : Form
    {
        private bool isdeleteclick=false;
        private data dataQA;
        private int indexed;
        public Form2(int index,data QaA) // new form in addnewQA_click
        {
            this.indexed = index;
            this.dataQA = QaA;
            InitializeComponent();
            showQAByIndex();
        }

        public Form2(int index,data QaA, int n) //new form in see QA_label_click
        {
            this.dataQA = QaA;
            this.indexed = index;
            InitializeComponent();
            showQAbyIndex(index);
        }

        public data QA1 { get => dataQA; set => dataQA = value; }
        public int Indexed { get => indexed; set => indexed = value; }
        public bool Isdeleteclick { get => isdeleteclick; set => isdeleteclick = value; }

        private void showQAByIndex() //addnewQA_click
        {
            numericUpDown1.Value = indexed+1;

            textBox1.Controls.Clear();
            textBox2.Controls.Clear();

            QA.ListTopic.Sort();
            QA.ListStatus.Sort();
            comboBox1.DataSource = QA.ListTopic;
            comboBox2.DataSource = QA.ListStatus;
            
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void showQAbyIndex(int index) //see QA_label_click
        {
            numericUpDown1.Value = index + 1;

            textBox1.Controls.Clear();
            textBox2.Controls.Clear();

            QA.ListTopic.Sort();
            QA.ListStatus.Sort();

            comboBox1.DataSource = QA.ListTopic;
            comboBox2.DataSource = QA.ListStatus;

            databaseToQA(index);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isdeleteclick == false)
            {
                if (indexed < dataQA.QAs1.Count) //case of label_click
                {
                    updateChangeTime(indexed);

                    return;
                }
                else //case of addQA_click
                {
                    if (textBox1.Text != "" && textBox1.Text != null)
                    {
                        QA newqa = new QA();
                        newqa.Question = textBox1.Text;
                        newqa.Answer = textBox2.Text;
                        newqa.DateChange = DateTime.Now.Date;
                        if (comboBox1.SelectedItem != null)
                            newqa.Topic = QA.ListTopic[comboBox1.SelectedIndex];
                        if (comboBox2.SelectedItem != null)
                            newqa.Status = QA.ListStatus[comboBox2.SelectedIndex];
                        if (newqa.Answer == "")
                            newqa.Status = Estatus.UNSOLVED.ToString();
                        dataQA.QAs1.Add(newqa);
                        Label newlb = new Label();
                        newlb.Text = textBox1.Text;
                    }
                }
            }
        }

        void databaseToQA(int index) //get QA from XML file
        {
            if (dataQA.QAs1[index].Answer != null)
            {
                textBox2.Text = dataQA.QAs1[index].Answer;
            }
            if (dataQA.QAs1[index].Question != null)
            {
                textBox1.Text = dataQA.QAs1[index].Question;
            }
            if (comboBox1.SelectedItem != null)
            {
                comboBox1.SelectedIndex = QA.ListTopic.IndexOf(dataQA.QAs1[index].Topic);
            }
            if (dataQA.QAs1[index].Status != null)
            {
                comboBox2.SelectedIndex = QA.ListStatus.IndexOf(dataQA.QAs1[index].Status);
            }
            if (dataQA.QAs1[index].DateChange != null)
            {
                label4.Text = label4.Text + ". last updated:" + dataQA.QAs1[index].DateChange.Day + "/" + dataQA.QAs1[index].DateChange.Month + "/" + dataQA.QAs1[index].DateChange.Year + " " + dataQA.QAs1[index].DateChange.Hour + ":" + dataQA.QAs1[index].DateChange.Minute;   //1/1/2001 12:31
            }
        }

        void QAtoDatabase(int index) //save QA to XML file
        {
            if (textBox2.Text != null)
            {
                dataQA.QAs1[index].Answer = textBox2.Text;
            }
            if (textBox1.Text != null)
            {
                dataQA.QAs1[index].Question = textBox1.Text;
            }
            if (comboBox1.SelectedIndex != -1)
            {
                dataQA.QAs1[index].Topic = QA.ListTopic[comboBox1.SelectedIndex];
            }
            if (comboBox2.SelectedIndex != -1)
            {
                dataQA.QAs1[index].Status = QA.ListStatus[comboBox2.SelectedIndex];
            }
            if (textBox2.Text == null || textBox2.Text == "")
            {
                dataQA.QAs1[index].Status = QA.ListStatus[QA.ListStatus.IndexOf(Estatus.UNSOLVED.ToString())];
            } // dong code nay co tac dung neu nguoi dung de answer trống thi status tu dong = unsolved
        }

        void updateChangeTime(int index) //get QA infor to xml
        {
            string temptopic = dataQA.QAs1[index].Topic;
            string tempQ = dataQA.QAs1[index].Question;
            string tempA = dataQA.QAs1[index].Answer; 
            QAtoDatabase(index);
            if (tempA != dataQA.QAs1[index].Answer || tempQ != dataQA.QAs1[index].Question || temptopic != dataQA.QAs1[index].Topic)
            {
                dataQA.QAs1[index].DateChange = DateTime.Now.Date;
            }
        }



        void deleteQA(int index)
        {
            dataQA.QAs1.RemoveAt(index);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult yesOrno = MessageBox.Show("are you sure want to delete?", "QA saver", MessageBoxButtons.YesNo);
            if (yesOrno == System.Windows.Forms.DialogResult.Yes && indexed < dataQA.QAs1.Count)
            {
                isdeleteclick = true;
                deleteQA(indexed);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("oops! this version do not support for add new topic. by the time this version released the creater to lazy to do it, please wait for the next version", "add topic");
        }
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    string newtopic = "";
        //    Form3 form3 = new Form3(ref newtopic);
        //    form3.ShowDialog();
        //    if (newtopic == "") return;
        //    else
        //    {
        //        QA.ListTopic.Add(newtopic);
        //        if (indexed < dataQA.QAs1.Count)
        //            showQAbyIndex(indexed);
        //        else
        //            showQAByIndex();
        //    }
        //}
    }
}
