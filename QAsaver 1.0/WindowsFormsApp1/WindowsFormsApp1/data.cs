using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF2
{
    [Serializable]
    public class data
    {
        private List<QA> QAs;

        public List<QA> QAs1 { get => QAs; set => QAs = value; }
    }
}
