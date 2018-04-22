
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WF2
{
    public partial class Form1 : Form
    {
        private List<int> indexfind; //list contains numbers of index of QA in find box
        private List<Label> loadlb; //contain new label will apear when find box change
        //private List<QA> loadfindQA;
        private data dataQ; //contains whole data upload from xml
        private string filePath = "data.xml"; //file xml name
        private List<Label> listLb; //list label appear when open the app
        public data DataQ { get => dataQ; set => dataQ = value; }
        public List<Label> ListLb { get => listLb; set => listLb = value; }
        public List<int> Indexfind { get => indexfind; set => indexfind = value; }
        //public List<QA> LoadfindQA { get => loadfindQA; set => loadfindQA = value; }

        public Form1()
        {
            InitializeComponent();
            normalize();

            try
            {
                dataQ = DeserializeFromXML(filePath) as data;
            }
            catch
            {
                SetDefault();
            }
            setLabel();
        }

        void normalize()
        {
            comboBox2.DataSource = QA.ListTopic2;
            comboBox1.DataSource = QA.ListStatus2;
        }


        //<QA show on label list>
        void setLabel() // label list
        {
            panel4.Controls.Clear();
            listLb = new List<Label>();
            for (int i = 0; i < dataQ.QAs1.Count; i++)
            {
                Label newlb = new Label();
                newlb.Location = new Point(5, i * constant.labelHeight + 8);
                newlb.Text = " " + (i + 1).ToString() + ". " + dataQ.QAs1[i].Question;
                newlb.Click += label_click;
                newlb.MouseHover += label_mouse_hover;
                newlb.MouseLeave += label_mouse_leave;
                newlb.TextAlign = ContentAlignment.MiddleLeft;
                newlb.Width = constant.labelWidth;
                newlb.MaximumSize = new System.Drawing.Size(10000, constant.labelHeight - 1);
                listLb.Add(newlb);
                panel4.Controls.Add(newlb);
            }
        }
        void label_click(object sender, EventArgs e) //click to open form 2. see infor of QA
        {
            if (string.IsNullOrEmpty((sender as Label).Text))
                return;
            //string tempQ = dataQ.QAs1[listLb.IndexOf(sender as Label)].Question;
            Form2 formQA = new Form2(listLb.IndexOf(sender as Label), dataQ, 0);
            formQA.ShowDialog();
            //if (tempQ != dataQ.QAs1[listLb.IndexOf(sender as Label)].Question)
            //{
            //    setLabel();
            //}
            setLabel();
        }
        void label_mouse_hover(object sender, EventArgs e)
        {
            (sender as Label).BackColor = Color.DarkGray;
        }
        void label_mouse_leave(object sender, EventArgs e)
        {
            (sender as Label).BackColor = System.Drawing.Color.White;
        }

        //<end QA shower by label/>

        void SetDefault() //no use
        {
            dataQ = new data();
            dataQ.QAs1 = new List<QA>();
        }



        // <xml part>
        private object DeserializeFromXML(string filePath) //get data from file XML, object:dataQA
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                XmlSerializer sr = new XmlSerializer(typeof(data));

                object result = sr.Deserialize(fs);
                fs.Close();
                return result;
            }
            catch (Exception e)
            {
                fs.Close();
                throw new NotImplementedException();
            }
        }
        private void SerializeToXML(object data, string filePath) // save object dataQA to XML file
        {
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            XmlSerializer sr = new XmlSerializer(typeof(data));

            sr.Serialize(fs, data);

            fs.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //save data
        {
            SerializeToXML(dataQ, filePath);
        }

        // <end xml/>

        //<add QA button>
        private void button1_Click(object sender, EventArgs e) // add new QA
        {
            Label lb = new Label();
            QA qa = new QA();
            if (dataQ == null || dataQ.QAs1 == null) //only appear when database is empty
            {
                dataQ = new data();
                dataQ.QAs1 = new List<QA>();
                qa.Question = "null";
                qa.Answer = "null";
                dataQ.QAs1.Add(qa);
                Form2 formQA = new Form2(0, dataQ);
                formQA.ShowDialog();
            }
            else
            {
                //lb.Location = new Point(0, (dataQ.QAs1.Count) * constant.labelHeight);
                //button1.Text = dataQ.QAs1.Count.ToString();

                Form2 formQA = new Form2((dataQ.QAs1.Count), dataQ);
                formQA.ShowDialog();
                //lb.Text = dataQ.QAs1[dataQ.QAs1.Count-1].Answer;
            }

            //panel4.Controls.Add(lb);
            setLabel(); //reload labels for new QA
        }
        //<end add QA btn>





        // <the function below will activate find QA in textbox feature>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            panel4.Controls.Clear();
            loadlabelbyQ();
        }
        void loadlabelbyQ()
        {
            loadlb = new List<Label>();
            //loadfindQA = new List<QA>();
            indexfind = new List<int>();
            int j = 0;
            for (int i = 0; i < DataQ.QAs1.Count(); i++)
            {
                if (findQA(textBox1.Text, i) == true)
                {
                    Label newlb = new Label();
                    //QA newqa = new QA();
                    //newqa = DataQ.QAs1[i];
                    newlb.Location = new Point(5, j * constant.labelHeight + 8);
                    j++;
                    newlb.Text = " " + (i + 1).ToString() + ". " + dataQ.QAs1[i].Question;
                    newlb.TextAlign = ContentAlignment.MiddleLeft;
                    newlb.Width = constant.labelWidth;
                    newlb.MaximumSize = new System.Drawing.Size(10000, constant.labelHeight - 1);
                    newlb.MouseHover += label_mouse_hover;
                    newlb.MouseLeave += label_mouse_leave;
                    newlb.Click += label_click2;
                    //loadfindQA.Add(newqa);
                    loadlb.Add(newlb);
                    indexfind.Add(i);

                    panel4.Controls.Add(newlb);
                }
            }
        }
        bool findQA(string str, int positison)
        {
            if (dataQ.QAs1[positison].Question.Contains(str) == true)
                return true;
            return false;
        }
        void label_click2(object sender, EventArgs e)
        {
            int m = loadlb.IndexOf(sender as Label);
            int n = indexfind[m];
            Form2 formQA = new Form2(n, dataQ, 0);
            formQA.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setLabel();
        }

        // <end feature/>



        // <filter by topic>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                return;
                //setLabel();
            }
            else
            {
                panel4.Controls.Clear();
                loadLabelByTopic();
            }
        }
        void loadLabelByTopic()
        {
            loadlb = new List<Label>();
            indexfind = new List<int>();
            int j = 0;
            for (int i = 0; i < DataQ.QAs1.Count(); i++)
            {
                if (findQAByTopic(QA.ListTopic2[comboBox2.SelectedIndex], i) == true)
                {
                    Label newlb = new Label();
                    newlb.Location = new Point(5, j * constant.labelHeight + 8);
                    j++;
                    newlb.Text = " " + (i + 1).ToString() + ". " + dataQ.QAs1[i].Question;
                    newlb.TextAlign = ContentAlignment.MiddleLeft;
                    newlb.Width = constant.labelWidth;
                    newlb.MaximumSize = new System.Drawing.Size(10000, constant.labelHeight - 1);
                    newlb.MouseHover += label_mouse_hover;
                    newlb.MouseLeave += label_mouse_leave;
                    newlb.Click += label_click2;
                    loadlb.Add(newlb);
                    indexfind.Add(i);

                    panel4.Controls.Add(newlb);
                }
            }
        }
        bool findQAByTopic(string topic, int index)
        {
            if (dataQ.QAs1[index].Topic == topic)
                return true;
            return false;
        }

        //<end filter by topic>


        //<filter by status>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                return;
                //setLabel();
            }
            else
            {
                panel4.Controls.Clear();
                loadLabelByStatus();
            }
        }
        void loadLabelByStatus()
        {
            loadlb = new List<Label>();
            indexfind = new List<int>();
            int j = 0;
            for (int i = 0; i < DataQ.QAs1.Count(); i++)
            {
                if (findQAByStatus(QA.ListStatus2[comboBox1.SelectedIndex], i) == true)
                {
                    Label newlb = new Label();
                    newlb.Location = new Point(5, j * constant.labelHeight + 8);
                    j++;
                    newlb.Text = " " + (i + 1).ToString() + ". " + dataQ.QAs1[i].Question;
                    newlb.TextAlign = ContentAlignment.MiddleLeft;
                    newlb.Width = constant.labelWidth;
                    newlb.MaximumSize = new System.Drawing.Size(10000, constant.labelHeight - 1);
                    newlb.MouseHover += label_mouse_hover;
                    newlb.MouseLeave += label_mouse_leave;
                    newlb.Click += label_click2;
                    loadlb.Add(newlb);
                    indexfind.Add(i);

                    panel4.Controls.Add(newlb);
                }
            }
        }
        bool findQAByStatus(string status, int index)
        {
            if (dataQ.QAs1[index].Status == status)
                return true;
            return false;
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            (sender as Label).ForeColor = System.Drawing.Color.DarkBlue;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            (sender as Label).ForeColor = System.Drawing.Color.DarkCyan;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("if you are confuse about using, or see anything wrong when using this app, please email me at quanghuy1998kh@gmail.com, or contact me by facebook if you know mine", "help");

        }

        // <end filter by status>









    }
}
