using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID
{
    public partial class ConnectForm : Form
    {
        private AppForm appForm;
        public ConnectForm(AppForm appform)
        {
            InitializeComponent();
            appForm = appform;
        }

        public string IpText
        {
            get
            {
                return iptext.Text;
            }
        }

        public string PortText
        {
            get
            {
                return porttext.Text;
            }
        }
    }
}
