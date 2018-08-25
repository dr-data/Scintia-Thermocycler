using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scintia_Thermocycler
{
    public partial class addCycleDlg : Form
    {
        public addCycleDlg()
        {
            InitializeComponent();
        }

        private void okAddStepBtn_Click(object sender, EventArgs e)
        {
            int reps;
            if (int.TryParse(addCycleDlgR.Text, out reps))
            {
                Program.cycleReps = addCycleDlgR.Text;
                Program.cycleName = addCycleDlgN.Text;
                Program.OKbtn = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Repetitions must be an integer");
            }
        }

        private void cnclStepBtn_Click(object sender, EventArgs e)
        {
            Program.OKbtn = false;
            this.Close();
        }
    }
}
