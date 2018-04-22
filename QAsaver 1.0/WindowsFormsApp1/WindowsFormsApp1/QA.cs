using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF2
{
    public class QA
    {
        private string question;
        private string answer;
        private DateTime date;
        private string status;
        private DateTime dateChange;
        private string topic;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public string Answer { get => answer; set => answer = value; }
        public string Question { get => question; set => question = value; }
        public DateTime DateChange { get => dateChange; set => dateChange = value; }
        public string Topic { get => topic; set => topic = value; }

        public static List<string> ListStatus = new List<string>() { "SOLVED", "DOING", "UNSOLVED"};
        public static List<string> ListStatus2 = new List<string>() { "","SOLVED", "DOING", "UNSOLVED" };
        public static List<string> ListTopic = new List<string>() { "<none>", "english", "food", "business", "books", "science", "developer", "technology", "affection", "how to ...", "sexual" };
        public static List<string> ListTopic2 = new List<string>() { "","<none>", "english", "food", "business", "books", "science", "developer", "technology", "affection", "how to ...", "sexual" };

    }
    public enum Estatus
    {
        SOLVED,
        DOING,
        UNSOLVED,
    }

}

