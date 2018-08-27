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
            bWorker.ProgressChanged += new ProgressChangedEventHandler(bWorker_ProgressChanged);
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
            predictGraph(Program.cycleToPerform);
            bWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Stop Button Function
        /// </summary>
        private void stopBtn_Click(object sender, EventArgs e)
        {
            bWorker.CancelAsync();
            checkIfPortIsOpen();
            serialPort1.Write("0");
            serialPort1.Write("1");
            serialPort1.Write("2");
            serialPort1.Write("3");
            serialPort1.Write("4");
            enableAllButtons();
            Program.running = false;
        }

        /// <summary>
        /// Port Data Received
        /// </summary>
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Program.readingPort = true;
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
                Program.aux = "";
                Program.readingPort = false;
            }
        }

        /// <summary>
        /// DoWork Background Thread Function.
        /// </summary>
        private void bWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (Program.running)
            {
                if (Program.cycleToPerform.Count == 0 && Program.currentStepDuration <= 0)
                {
                    bWorker.CancelAsync();
                }
                if (bWorker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else if (Program.cycleToPerform.Count >= 0)
                {
                    counter.Start();
                    if (Program.currentStepDuration <= 0 && Program.cycleToPerform.Count > 0)
                    {
                        Program.currentTargetTemperature = Program.cycleToPerform[0][0];
                        Program.currentStepDuration = (int) Program.cycleToPerform[0][1] * 1000;
                        Program.reachedTargetTempFirstTime = false;
                        Program.cycleToPerform.RemoveAt(0);
                    }

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
                    counter.Stop();
                    if (Program.timestamp % 500 == 0)
                    {
                        updateTopTemp();
                        updateBottomTemp();
                        switch (stateBottom)
                        {
                            case 0:
                                break;

                            case 1:
                                if (!Program.readingPort)
                                {
                                    turnBottomROff();
                                }
                                break;

                            case 2:
                                if (!Program.readingPort)
                                {
                                    turnBottomROff();
                                    turnAllFansOn();
                                }
                                break;

                            case 3:
                                if (!Program.readingPort)
                                {
                                    turnBottomROn();
                                }
                                break;

                            case 4:
                                if (!Program.readingPort)
                                {
                                    turnBottomROn();
                                    turnAllFansOff();
                                }
                                break;
                        }

                        switch (stateTop)
                        {
                            case 0:
                                break;

                            case 1:
                                if (!Program.readingPort)
                                {
                                    turnTopROff();
                                }
                                break;

                            case 2:
                                if (!Program.readingPort)
                                {
                                    turnTopROn();
                                }
                                break;
                        }
                        bWorker.ReportProgress(stateBottom, stateTop);
                    }
                }
            }
        }

        private void bWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MessageBox.Show(Program.timestamp.ToString());
            updateGraph(Program.timestamp);
            Program.timestamp += 500;
            if (Program.reachedTargetTempFirstTime)
            {
                Program.currentStepDuration -= 500;
            }

            Program.residualMilliseconds += counter.ElapsedMilliseconds;
            if (Program.residualMilliseconds >= 1)
            {
                Program.timestamp += (int)Program.residualMilliseconds;
                if (Program.reachedTargetTempFirstTime)
                {
                    Program.currentStepDuration -= (int)Program.residualMilliseconds;
                }
                Program.residualMilliseconds = 0;
            }
        }

        private void predictGraph(List<List<float>> stepList)
        {
            List<List<float>> innerList = new List<List<float>>(stepList);
            double expectedDur = 0;
            double expectedTargetTemp = 0;
            double expectedTopTemp = 0;
            double expectedBotTemp = 0;
            bool innerReachTargetTemp = false;
            updateTopTemp();
            updateBottomTemp();
            double time = 0;

            ttChart.Series["Estimated Top Temp"].Points.AddXY(time, Program.topTemp);
            ttChart.Series["Estimated Bottom Temp"].Points.AddXY(time, Program.botTemp);

            expectedTopTemp = Program.topTemp;
            expectedBotTemp = Program.botTemp;

            while (innerList.Count >= 0)
            {
                if (innerList.Count == 0 && expectedDur <= 0)
                {
                    break;
                }
                else
                {
                    if (expectedDur == 0)
                    {
                        expectedTargetTemp = stepList[0][0];
                        expectedDur = stepList[0][1] * 1000;
                        innerReachTargetTemp = false;
                        innerList.RemoveAt(0);
                    }
                    if (!innerReachTargetTemp && ((expectedBotTemp > (expectedTargetTemp - Program.lowerLimit)) && (expectedBotTemp < (expectedTargetTemp + Program.upperLimit))))
                    {
                        innerReachTargetTemp = true;
                    }

                    if (expectedBotTemp > expectedTargetTemp)
                    {
                        if (expectedBotTemp > expectedTargetTemp + Program.tempDropConstant)
                        {
                            expectedBotTemp -= Program.tempDropConstant / 2;
                            expectedTopTemp -= ((Program.tempOnlyTopRaiseConstant - 0.05) / 2);
                        }
                        else
                        {
                            expectedBotTemp -= Program.tempNoFanDropConstant / 2;
                            expectedTopTemp -= ((Program.tempOnlyTopRaiseConstant - 0.05) / 2);
                        }
                    }
                    else
                    {
                        if (expectedBotTemp < expectedTargetTemp)
                        {
                            if (expectedBotTemp < expectedTargetTemp - Program.tempRaiseConstant)
                            {
                                expectedBotTemp += Program.tempRaiseConstant / 2;
                                expectedTopTemp += ((Program.tempOnlyTopRaiseConstant - 0.05) / 2);
                            }
                            else
                            {
                                expectedBotTemp += Program.tempOnlyTopRaiseConstant / 2;
                                expectedTopTemp += Program.tempOnlyTopRaiseConstant / 2;
                            }
                        }
                    }

                    if(expectedTopTemp > 92)
                    {
                        expectedTopTemp -= Program.tempNoFanDropConstant / 2;
                    }
                    else
                    {
                        expectedTopTemp += Program.tempOnlyTopRaiseConstant / 2;
                    }

                    ttChart.Series["Estimated Top Temp"].Points.AddXY(time / 1000, expectedTopTemp);
                    ttChart.Series["Estimated Bottom Temp"].Points.AddXY(time / 1000, expectedBotTemp);

                    time += 500;
                    if (innerReachTargetTemp)
                    {
                        expectedDur -= 500;
                    }
                }
            }
            ttChart.ChartAreas[0].AxisX.ScaleView.Zoom(0, time/1000);
            ttChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            ttChart.ChartAreas[0].AxisY.ScaleView.Zoom(0, 120);
            ttChart.Refresh();
        }

        private void updateGraph(double timestamp)
        {
            ttChart.Series["Measured Top Temp"].Points.AddXY( timestamp / 1000, (double) Program.topTemp);
            ttChart.Series["Measured Bottom Temp"].Points.AddXY( timestamp / 1000, (double) Program.botTemp);
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

        private void checkIfPortIsOpen()
        {
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Open();
                }
                catch (System.Exception ex)
                {
                    // MessageBox.Show(ex.ToString());
                }
            }
        }

        private void updateTopTemp()
        {
            if (!Program.readingPort)
            {
                checkIfPortIsOpen();
                Program.readingBottomTemp = false;
                serialPort1.Write("A");
            }
        }

        private void updateBottomTemp()
        {
            if (!Program.readingPort)
            {
                checkIfPortIsOpen();
                Program.readingBottomTemp = true;
                serialPort1.Write("B");
            }
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
