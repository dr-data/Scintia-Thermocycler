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
        /// Internal Helpers.
        /// </summary>
        BackgroundWorker bWorker;
        Stopwatch counter;

        public Form1()
        {
            InitializeComponent();
            counter = new Stopwatch();
            bWorker = new BackgroundWorker();
            bWorker.DoWork += new DoWorkEventHandler(bWorker_DoWork);
            bWorker.WorkerSupportsCancellation = true;
            bWorker.WorkerReportsProgress = true;
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
            disableAllButtons();
            Program.TraverseTree(stepsList.Nodes);
            // timer1.Enabled = true;
            timer1.Start();
        }

        /// <summary>
        /// Stop Button Function
        /// </summary>
        private void stopBtn_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            bWorker.CancelAsync();
            if (serialPort1.IsOpen)
            {
                serialPort1.Write("0");
                serialPort1.Write("1");
                serialPort1.Write("2");
                serialPort1.Write("3");
                serialPort1.Write("4");
            }
            Program.running = false;
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
                if (Program.readingBottomTemp)
                {
                    Program.botTemp = (float) Program.inDataToTemp(Program.nuevo)[0];
                }
                else
                {
                    Program.topTemp = (float) Program.inDataToTemp(Program.nuevo)[0];
                }                
            }
        }

        /// <summary>
        /// Timer Function
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter.Start();
            if (Program.running)
            {
                if (Program.cycleToPerform.Count == 0 && Program.currentStepDuration <= 0)
                {
                    Program.running = false;
                    enableAllButtons();
                    timer1.Stop();
                }
                else if (Program.cycleToPerform.Count >= 0)
                {
                    if (Program.currentStepDuration <= 0 && Program.cycleToPerform.Count > 0)
                    {
                        Program.currentTargetTemperature = Program.cycleToPerform[0][0];
                        Program.currentStepDuration = (int) Program.cycleToPerform[0][1] * 1000;
                        Program.reachedTargetTempFirstTime = false;
                        Program.cycleToPerform.RemoveAt(0);
                    }
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
                    updateTopTemp();
                    updateBottomTemp();
                    if (bWorker.IsBusy != true)
                    {
                        bWorker.RunWorkerAsync();
                    }
                    updateGraph(Program.timestamp);
                    Program.timestamp += 500;
                    if (Program.reachedTargetTempFirstTime)
                    {
                        Program.currentStepDuration -= 500;
                    }
                    counter.Stop();
                    Program.residualMilliseconds += counter.ElapsedMilliseconds;
                    if (Program.residualMilliseconds >= 1)
                    {
                        Program.timestamp += (int) Program.residualMilliseconds;
                        if (Program.reachedTargetTempFirstTime)
                        {
                            Program.currentStepDuration -= (int) Program.residualMilliseconds;
                        }
                        Program.residualMilliseconds = 0;
                    }
                }
            }
        }

        /// <summary>
        /// DoWork Background Thread Function.
        /// </summary>
        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (Program.running)
            {
                if (bWorker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    int stateBottom = 0;
                    int stateTop = 0;
                    if (!Program.reachedTargetTempFirstTime && Program.tempInRange())
                    {
                        Program.reachedTargetTempFirstTime = true;
                    }
                    if (Program.botTemp > Program.currentTargetTemperature)
                    {
                        stateBottom = 1;
                        if (Program.botTemp > Program.currentTargetTemperature + Program.tempDropConstant)
                        {
                            stateBottom = 2;
                        }
                    }
                    else if (Program.botTemp < Program.currentTargetTemperature)
                    {
                        stateBottom = 3;
                        if (Program.botTemp < Program.currentTargetTemperature - Program.tempRaiseConstant)
                        {
                            stateBottom = 4;
                        }
                    }
                    if (Program.topTemp > Program.topTempUpperLimit)
                    {
                        stateTop = 1;
                    }
                    else if (Program.topTemp < Program.topTempLowerLimit)
                    {
                        stateTop = 2;
                    }
                    bWorker.ReportProgress(stateBottom, stateTop);
                }
            }
        }

        private void bWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
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

            switch (e.ProgressPercentage)
            {
                case 0:    
                break;
                
                case 1:
                turnBottomROff();
                break;

                case 2:
                turnBottomROff();
                turnAllFansOn();
                break;

                case 3:
                turnBottomROn();
                break;

                case 4:
                turnBottomROn();
                turnAllFansOff();
                break;
            }

            switch ((int) e.UserState)
            {
                case 0:
                break;

                case 1:
                turnTopROff();
                break;

                case 2:
                turnTopROn();
                break;
            }
            
        }

        private void updateGraph(double timestamp)
        {
            ttChart.Series[2].Points.AddXY( timestamp / 1000, (double) Program.topTemp);
            ttChart.Series[3].Points.AddXY( timestamp / 1000, (double) Program.botTemp);
            ttChart.Refresh();
        }

        private void enableAllButtons()
        {
            addSBtn.Enabled = true;
            addCBtn.Enabled = true;
            editSelectedBtn.Enabled = true;
            remSelctedBtn.Enabled = true;
            runBtn.Enabled = true;
        }

        private void disableAllButtons()
        {
            addSBtn.Enabled = false;
            addCBtn.Enabled = false;
            editSelectedBtn.Enabled = false;
            remSelctedBtn.Enabled = false;
            runBtn.Enabled = false;
        }

        private void updateTopTemp()
        {
            Program.readingBottomTemp = false;
            serialPort1.Write("A");
        }

        private void updateBottomTemp()
        {
            Program.readingBottomTemp = true;
            serialPort1.Write("B");
        }

        private void turnTopROn()
        {
            serialPort1.Write("8");
        }

        private void turnTopROff()
        {
            serialPort1.Write("3");
        }

        private void turnBottomROn()
        {
            serialPort1.Write("9");
        }

        private void turnBottomROff()
        {
            serialPort1.Write("4");
        }

        private void turnAllFansOn()
        {
            serialPort1.Write("5");
            serialPort1.Write("6");
            serialPort1.Write("7");
        }

        private void turnAllFansOff()
        {
            serialPort1.Write("0");
            serialPort1.Write("1");
            serialPort1.Write("2");
        }
    }
}
