using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleDizimistas
{
    public partial class frm_splash : Form
    {
        public frm_splash()
        {
            InitializeComponent();

        }

        private void frm_splash_Load(object sender, EventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i <= progressBar1.Maximum; i++)
            {
                progressBar1.Value = i;
            }

            this.Close();
        }

        private void frm_splash_Load_1(object sender, EventArgs e)
        {

        }
    }
}
