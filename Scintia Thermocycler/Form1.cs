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
        Stopwatch counter;

        public Form1()
        {
            InitializeComponent();
            counter = new Stopwatch();
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
                if (stepsList.SelectedNode == null)
                {
                    MessageBox.Show("Select an element to edit");
                }
                else if (stepsList.SelectedNode.Text.Contains("Temperature"))
                {
                    addStepDlg addstep = new addStepDlg();
                    addstep.Text = "Editing Step";
                    addstep.ShowDialog();
                    if (Program.OKbtn)
                    {
                        float temperature, duration;
                        float.TryParse(Program.stepTemperature, out temperature);
                        float.TryParse(Program.stepDuration, out duration);
                        stepsList.SelectedNode.Text = "Temperature: " + Program.stepTemperature + " °C, Duration: " + Program.stepDuration + " s";
                        stepsList.Tag = new List<float> { temperature, duration };
                    }
                }
                else
                {
                    addCycleDlg addCycle = new addCycleDlg();
                    addCycle.Text = "Editing Cycle";
                    addCycle.ShowDialog();
                    if (Program.OKbtn)
                    {
                        int repetitions;
                        int.TryParse(Program.cycleReps, out repetitions);
                        stepsList.SelectedNode.Text = "Cycle Name: " + Program.cycleName + ", Repetitions: " + Program.cycleReps;
                        stepsList.Tag = repetitions;
                    }
                }
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
            checkIfPortIsOpen();
            turnAllFansOff();
            turnTopROff();
            turnBottomROff();
            disableAllButtons();
            foreach(var series in ttChart.Series) {
                series.Points.Clear();
            }
            ttChart.Refresh();
            Program.timestamp = 0;
            Program.topTemp = 0;
            Program.botTemp = 0;
            if (Program.cycleToPerform.Count > 0)
            {
                Program.cycleToPerform.RemoveRange(0, Program.cycleToPerform.Count - 1);
            }
            Program.TraverseTree(stepsList.Nodes);
            bgw.RunWorkerAsync();
        }

        /// <summary>
        /// Stop Button Function
        /// </summary>
        private void stopBtn_Click(object sender, EventArgs e)
        {
            bgw.CancelAsync();
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
            if (Program.aux[Program.aux.Length-1]=='\n')
            {
                if (Program.turn != true)
                {
                    Program.botTemp = Program.inDataToTemp();
                }
                else
                {
                    Program.topTemp = Program.inDataToTemp();
                }
                Program.aux = "";
                Program.readingPort = false;
            }
        }

        /// <summary>
        /// DoWork Background Thread Function.
        /// </summary>
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            // Predict results
            // predictGraph(Program.cycleToPerform);
            // The first time this is invoked preheat the top resistor
            Program.preheat = true;
            // If the program is running, do the following
            while (Program.running)
            {
                // If cancelation was called
                if (bgw.CancellationPending)
                {
                    // Program is no longer running
                    Program.running = false;
                    e.Cancel = true;
                }
                // If there are no more steps to go through and the duration of the last step is over
                if (Program.cycleToPerform.Count == 0 && Program.currentStepDuration <= 0)
                {
                    // Program is no longer running
                    Program.running = false;
                    return;
                }
                // Otherwise, if there are steps yet to do
                else if (Program.cycleToPerform.Count >= 0)
                {
                    // Start counting process time
                    counter.Start();
                    if (counter.ElapsedMilliseconds != 0) { counter.Reset(); }
                    // Preheat the top resistance
                    if (Program.preheat)
                    {                        
                        if (!Program.turn)
                        {
                            Program.turn = true;
                        }
                        updateTopTemp();
                        if (Program.fansOn)
                        {
                            turnAllFansOff();
                        }
                        if (Program.botROn)
                        {
                            turnBottomROff();
                        }
                        if (!Program.topROn)
                        {
                            turnTopROn();
                        }
                        if (Program.topTemp >= ((Program.topTempUpperLimit + Program.topTempLowerLimit) / 2))
                        {
                            turnTopROff();
                            Program.preheat = false;
                            Program.turn = false;
                        }
                    }
                    // Enter the main steps list
                    else
                    {
                        // If this isn't the last step and duration is over
                        if (Program.cycleToPerform.Count > 0 && Program.currentStepDuration <= 0)
                        {
                            // Reset reachedTargetTempFirstTime flag
                            Program.reachedTargetTempFirstTime = false;
                            // Load the next target temperature
                            Program.currentTargetTemperature = Program.cycleToPerform[0][0];
                            // Load the next step duration in miliseconds
                            Program.currentStepDuration = (int)Program.cycleToPerform[0][1] * 1000;
                            // Take out step at index 0
                            Program.cycleToPerform.RemoveAt(0);
                        }
                        // Check if temperature is in range
                        if (Program.tempInRange() && Program.reachedTargetTempFirstTime != true)
                        {
                            // We've reached it for the first time so we can start counting duration
                            Program.reachedTargetTempFirstTime = true;
                        }
                        // Let's take turns on temperatures, true for top, false for bottom
                        if (Program.turn == true)
                        {
                            // Get top temperature updated
                            updateTopTemp();
                            // Make decisions on the updated temperature
                            if ((Program.topTemp >= Program.topTempUpperLimit) && (Program.topROn == true))
                            {
                                turnTopROff();
                            }
                            else if ((Program.topTemp <= Program.topTempLowerLimit) && (Program.topROn == false))
                            {
                                turnTopROn();
                            }
                            // Change turns
                            Program.turn = false;
                        }
                        else if (Program.turn == false)
                        {
                            // Update bottom temperature
                            updateBottomTemp();
                            // Wait for port to be read
                            Thread.Sleep(100);
                            while (Program.readingPort)
                            {
                            }
                            // Make decisions on the updated temperature
                            if (Program.botTemp >= Program.currentTargetTemperature)
                            {
                                if (Program.botROn == true)
                                {
                                    turnBottomROff();
                                }
                                if (Program.botTemp > (Program.currentTargetTemperature + Program.tempDropConstant))
                                {
                                    turnAllFansOn();
                                }
                            }
                            else
                            {
                                turnAllFansOff();
                                if (Program.botTemp < (Program.currentTargetTemperature - Program.tempRaiseConstant))
                                {
                                    if (Program.botROn == false)
                                    {
                                        turnBottomROn();
                                    }
                                }
                            }
                            // Change turns
                            Program.turn = true;
                        }
                    }
                    // Stop counter and report Progress
                    counter.Stop();
                    bgw.ReportProgress(0);
                }
            }
        }
        
        /// <summary>
        /// ReportProgress Background Thread Function
        /// </summary>
        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Update timestamp with the time the step took
            Program.residualMilliseconds += counter.ElapsedMilliseconds;
            Program.timestamp += (int) Program.residualMilliseconds;
            Program.timestamp += 100;
            // Update currentStepDuration only if we have reached the target temp before
            if (Program.reachedTargetTempFirstTime)
            {
                Program.currentStepDuration -= 100;
                Program.currentStepDuration -= (int) Program.residualMilliseconds;
            }
            Debug.WriteLine(Program.currentStepDuration.ToString() + ", " + counter.ElapsedMilliseconds.ToString());
            Program.residualMilliseconds = 0;
            // Update graph with the current timestamp
            updateGraph(Program.timestamp);
        }

        /// <summary>
        /// Background Thread Completed Function
        /// </summary>
        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Program.running = false;
                turnAllFansOff();
                turnBottomROff();
                turnTopROff();
                enableAllButtons();
                MessageBox.Show("Stopped the cycle.");
            }
            else if (e.Error != null)
            {
                Program.running = false;
                turnAllFansOff();
                turnBottomROff();
                turnTopROff();
                enableAllButtons();
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
                Program.running = false;
                turnAllFansOff();
                turnBottomROff();
                turnTopROff();
                enableAllButtons();
                MessageBox.Show("Background work finished successfully");
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

            ttChart.Invoke((Action)(() => ttChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true));
            ttChart.Invoke((Action)(() => ttChart.ChartAreas[0].AxisY.ScaleView.Zoom(0, 120)));

            updateTopTemp();
            while (Program.readingPort)
            { }
            updateBottomTemp();
            while (Program.readingPort)
            { }
            double time = 0;

            ttChart.Invoke((Action)(() => ttChart.Series["Estimated Top Temp"].Points.AddXY(time, Program.topTemp)));
            ttChart.Invoke((Action)(() => ttChart.Series["Estimated Bottom Temp"].Points.AddXY(time, Program.botTemp)));

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

                    ttChart.Invoke((Action)(() => ttChart.Series["Estimated Top Temp"].Points.AddXY(time / 1000, expectedTopTemp)));
                    ttChart.Invoke((Action)(() => ttChart.Series["Estimated Top Temp"].Points.AddXY(time / 1000, expectedBotTemp)));
                    ttChart.Invoke((Action)(() => ttChart.Refresh()));
                    time += 100;
                    if (innerReachTargetTemp)
                    {
                        expectedDur -= 100;
                    }
                }
            }
            ttChart.Invoke((Action)(() => ttChart.ChartAreas[0].AxisX.ScaleView.Zoom(0, time/1000)));            
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
            serialPort1.Write("A");
            Thread.Sleep(100);
        }

        private void updateBottomTemp()
        {
            serialPort1.Write("B");
            Thread.Sleep(100);
        }

        private void turnTopROn()
        {
                Program.topROn = true;
                serialPort1.Write("8");
        }

        private void turnTopROff()
        {
            Program.topROn = false;
            serialPort1.Write("3");
        }

        private void turnBottomROn()
        {
            Program.botROn = true;
            serialPort1.Write("9");
        }

        private void turnBottomROff()
        {
            Program.botROn = false;
            serialPort1.Write("4");
        }

        private void turnAllFansOn()
        {
            Program.fansOn = true;
            serialPort1.Write("5");
            serialPort1.Write("6");
            serialPort1.Write("7");
        }

        private void turnAllFansOff()
        {
            Program.fansOn = false;
            serialPort1.Write("0");
            serialPort1.Write("1");
            serialPort1.Write("2");
        }
    }
}
