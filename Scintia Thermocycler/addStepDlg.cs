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
    public partial class addStepDlg : Form
    {
        public addStepDlg()
        {
            InitializeComponent();
        }

        private void okAddStepBtn_Click(object sender, EventArgs e)
        {
            if (addStepDlgT.Text != "" && addStepDlgD.Text != "")
            {
                float temperature = 0.0F;
                int duration = 0;

                if (float.TryParse(addStepDlgT.Text, out temperature))
                {
                    if (int.TryParse(addStepDlgD.Text, out duration))
                    {
                        Program.stepTemperature = addStepDlgT.Text;
                        Program.stepDuration = addStepDlgD.Text;
                        Program.OKbtn = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Duration must be an integer number of seconds.");
                    }
                }
                else 
                {
                    MessageBox.Show("Temperature must be a floating point number of degree celcius");
                }
            }
            else
            {
                MessageBox.Show("Temperature and Duration must be filled out.");
            }
        }

        private void cnclStepBtn_Click(object sender, EventArgs e)
        {
            Program.OKbtn = false;
            this.Close();
        }
    }
}
