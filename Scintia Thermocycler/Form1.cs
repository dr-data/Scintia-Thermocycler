using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Scintia_Thermocycler
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// Background worker to do run cycle.
        /// </summary>
        BackgroundWorker bWorker;

        public Form1()
        {
            InitializeComponent();
            bWorker = new BackgroundWorker();
            bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
            bWorker.WorkerSupportsCancellation = true;
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Open();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Add Step Button Function.
        /// </summary>
        private void addSBtn_Click(object sender, EventArgs e)
        {
            if (!Program.running)
            {
                if (stepsList.SelectedNode == null)
                {
                    stepsList.SelectedNode = stepsList.Nodes["Root"];
                }
                addStepDlg addstep = new addStepDlg();
                addstep.ShowDialog();
                if (Program.OKbtn)
                {
                    float temperature, duration;
                    float.TryParse(Program.stepTemperature, out temperature);
                    float.TryParse(Program.stepDuration, out duration);

                    TreeNode newStep = new TreeNode("Temperature: " + Program.stepTemperature + " °C, Duration: " + Program.stepDuration + " s");
                    newStep.Tag = new List<float> { temperature, duration };

                    if (stepsList.SelectedNode.Name == "Root")
                    {
                        newStep.Name = "step-" + stepsList.SelectedNode.GetNodeCount(false);
                        stepsList.SelectedNode.Nodes.Add(newStep);
                        stepsList.SelectedNode.Expand();
                    }
                    else if (stepsList.SelectedNode.Text.Contains("Cycle Name"))
                    {
                        newStep.Name = stepsList.SelectedNode.Name + "-step-" + stepsList.SelectedNode.GetNodeCount(false);
                        stepsList.SelectedNode.Nodes.Add(newStep);
                        stepsList.SelectedNode.Expand();
                    }
                    else
                    {
                        stepsList.SelectedNode.Parent.Nodes.Insert(stepsList.SelectedNode.Index + 1, newStep);
                        stepsList.SelectedNode.Parent.Expand();
                    }
                }
            }
            else
            {
                MessageBox.Show("Can't add steps while program is running.");
            }
        }

        /// <summary>
        /// Add Cycle Button Function.
        /// </summary>
        private void addCBtn_Click(object sender, EventArgs e)
        {
            if (!Program.running)
            {
                if (stepsList.SelectedNode == null)
                {
                    stepsList.SelectedNode = stepsList.Nodes["Root"];
                }
                addCycleDlg addCycle = new addCycleDlg();
                addCycle.ShowDialog();
                if (Program.OKbtn)
                {
                    int repetitions;
                    int.TryParse(Program.cycleReps, out repetitions);

                    TreeNode newCycle = new TreeNode("Cycle Name: " + Program.cycleName + ", Repetitions: " + Program.cycleReps);
                    newCycle.Tag = repetitions;

                    if (stepsList.SelectedNode.Name == "Root")
                    {
                        stepsList.SelectedNode.Nodes.Add(newCycle);
                        stepsList.SelectedNode.Expand();
                    }
                    else if (stepsList.SelectedNode.Text.Contains("Temperature"))
                    {
                        stepsList.SelectedNode.Parent.Nodes.Insert(stepsList.SelectedNode.Index + 1, newCycle);
                    }
                }
            }
            else
            {
                MessageBox.Show("Can't add cycle while program is running");
            }
        }

        /// <summary>
        /// Remove Selected Element Button Function.
        /// </summary>
        private void remSelctedBtn_Click(object sender, EventArgs e)
        {
            if (!Program.running)
            {
                if (stepsList.SelectedNode != null)
                {
                    if (stepsList.SelectedNode.Name != "Root")
                    {
                        stepsList.SelectedNode.Remove();
                    }
                }
            }
            else
            {
                MessageBox.Show("Can't remove steps while program is running");
            }
        }

        /// <summary>
        /// Edit Selected Element Button Function.
        /// </summary>
        private void editSelectedBtn_Click(object sender, EventArgs e)
        {
            if (!Program.running)
            {
            }
            else
            {
                MessageBox.Show("Can't edit step while program is running");
            }
        }

        /// <summary>
        /// Run Button Function
        /// </summary>
        private void runBtn_Click(object sender, EventArgs e)
        {
            Program.running = true;
            addSBtn.Enabled = false;
            addCBtn.Enabled = false;
            editSelectedBtn.Enabled = false;
            remSelctedBtn.Enabled = false;
            runBtn.Enabled = false;
            Program.cycleToPerform = Program.treeToList(stepsList);
            bWorker.RunWorkerAsync(Program.cycleToPerform);
        }

        /// <summary>
        /// Stop Button Function
        /// </summary>
        private void stopBtn_Click(object sender, EventArgs e)
        {
            bWorker.CancelAsync();
            Program.running = false;
            addSBtn.Enabled = true;
            addCBtn.Enabled = true;
            editSelectedBtn.Enabled = true;
            remSelctedBtn.Enabled = true;
            runBtn.Enabled = true;
        }

        /// <summary>
        /// Port Data Received
        /// </summary>
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Program.aux += serialPort1.ReadExisting();
            if (Program.aux[Program.aux.Length - 1] == '\n')
            {
                Program.nuevo = Program.aux.Clone() as String;
                if (Program.readingBottom)
                {
                    Program.botTemp = (float) Program.inDataToTemp(Program.nuevo);
                }
                else
                {
                    Program.topTemp = (float)Program.inDataToTemp(Program.nuevo);
                }
                Program.tempRead = true;                
            }
        }

        /// <summary>
        /// DoWork Background Thread Function.
        /// </summary>
        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Declare internal variables.
            Stopwatch counter;
            List<List<float>> stepsToDo = (List<List<float>>) e.Argument;
            float tempObj;
            int curDur;
            long curTime = 0L;

            while (stepsToDo.Count() > 0)
            {
                tempObj = stepsToDo[0][0];
                curDur = (int) stepsToDo[0][1] * 1000;
                while(curDur > 0){
                    // Start timer
                    counter = Stopwatch.StartNew();

                    // Stop if Cancel was clicked
                    if (this.bWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    // Get Current Top Temperature
                    Program.readingBottom = false;
                    serialPort1.Write("A");
                    while (!Program.tempRead)
                    {
                    }
                    Program.tempRead = false;

                    //Get Current Bottom Temperature
                    Program.readingBottom = true;
                    serialPort1.Write("B");
                    while (!Program.tempRead)
                    {
                    }
                    Program.tempRead = false;

                    // Check and Control Top Temp
                    if (Program.topTemp > 92)
                    {
                        serialPort1.Write("3");
                    }
                    else if (Program.topTemp < 92)
                    {
                        serialPort1.Write("8");
                    }
 
                    // Check and Control Bottom Temp
                    if (Program.botTemp > tempObj)
                    {
                        serialPort1.Write("4");
                        if (Program.botTemp > tempObj + Program.tempDropConstant)
                        {
                            serialPort1.Write("5");
                            serialPort1.Write("6");
                            serialPort1.Write("7");
                        }
                    }
                    else if (Program.botTemp < tempObj)
                    {
                        serialPort1.Write("9");
                        if (Program.botTemp < tempObj + Program.tempRaiseConstant)
                        {
                            serialPort1.Write("0");
                            serialPort1.Write("1");
                            serialPort1.Write("2");
                        }
                    }

                    // Graph both temps
                    ttChart.BeginInvoke((Action)(() =>
                        ttChart.Series[2].Points.AddXY((double)curTime/1000,(double)Program.topTemp)
                        )
                    );
                    ttChart.BeginInvoke((Action)(() =>
                        ttChart.Series[3].Points.AddXY((double)curTime/1000, (double)Program.botTemp)
                        )
                    );
                    ttChart.BeginInvoke((Action)(() => 
                            ttChart.Refresh()
                        )
                    );

                    // Stop Timer
                    counter.Stop();

                    // Update Duration and Time elapsed
                    curTime += counter.ElapsedMilliseconds;
                    curDur -= (int)counter.ElapsedMilliseconds;
                }

                stepsToDo.RemoveAt(0);
            }
        }
    }
}
