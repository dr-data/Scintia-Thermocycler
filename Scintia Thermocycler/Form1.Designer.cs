namespace Scintia_Thermocycler
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Cycle Name: Main, Repetitions: 1. ");
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.addSBtn = new System.Windows.Forms.Button();
            this.addCBtn = new System.Windows.Forms.Button();
            this.remSelctedBtn = new System.Windows.Forms.Button();
            this.editSelectedBtn = new System.Windows.Forms.Button();
            this.stepsList = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.ttChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.stopBtn = new System.Windows.Forms.Button();
            this.runBtn = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.bgw = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ttChart)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(852, 544);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(23, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(806, 498);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.stepsList, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(800, 243);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel6.Controls.Add(this.addSBtn, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.addCBtn, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.remSelctedBtn, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.editSelectedBtn, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(794, 44);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // addSBtn
            // 
            this.addSBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addSBtn.Location = new System.Drawing.Point(3, 3);
            this.addSBtn.Name = "addSBtn";
            this.addSBtn.Size = new System.Drawing.Size(113, 38);
            this.addSBtn.TabIndex = 0;
            this.addSBtn.Text = "Add Step";
            this.addSBtn.UseVisualStyleBackColor = true;
            this.addSBtn.Click += new System.EventHandler(this.addSBtn_Click);
            // 
            // addCBtn
            // 
            this.addCBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addCBtn.Location = new System.Drawing.Point(122, 3);
            this.addCBtn.Name = "addCBtn";
            this.addCBtn.Size = new System.Drawing.Size(113, 38);
            this.addCBtn.TabIndex = 1;
            this.addCBtn.Text = "Add Cycle";
            this.addCBtn.UseVisualStyleBackColor = true;
            this.addCBtn.Click += new System.EventHandler(this.addCBtn_Click);
            // 
            // remSelctedBtn
            // 
            this.remSelctedBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.remSelctedBtn.Location = new System.Drawing.Point(647, 3);
            this.remSelctedBtn.Name = "remSelctedBtn";
            this.remSelctedBtn.Size = new System.Drawing.Size(144, 38);
            this.remSelctedBtn.TabIndex = 2;
            this.remSelctedBtn.Text = "Remove Selected";
            this.remSelctedBtn.UseVisualStyleBackColor = true;
            this.remSelctedBtn.Click += new System.EventHandler(this.remSelctedBtn_Click);
            // 
            // editSelectedBtn
            // 
            this.editSelectedBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editSelectedBtn.Location = new System.Drawing.Point(241, 3);
            this.editSelectedBtn.Name = "editSelectedBtn";
            this.editSelectedBtn.Size = new System.Drawing.Size(113, 38);
            this.editSelectedBtn.TabIndex = 3;
            this.editSelectedBtn.Text = "Edit Selected";
            this.editSelectedBtn.UseVisualStyleBackColor = true;
            this.editSelectedBtn.Click += new System.EventHandler(this.editSelectedBtn_Click);
            // 
            // stepsList
            // 
            this.stepsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepsList.Location = new System.Drawing.Point(3, 53);
            this.stepsList.Name = "stepsList";
            treeNode1.Name = "Root";
            treeNode1.Text = "Cycle Name: Main, Repetitions: 1. ";
            this.stepsList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.stepsList.Size = new System.Drawing.Size(794, 187);
            this.stepsList.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.ttChart, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 252);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(800, 243);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // ttChart
            // 
            chartArea1.Name = "ChartArea1";
            this.ttChart.ChartAreas.Add(chartArea1);
            this.ttChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.ttChart.Legends.Add(legend1);
            this.ttChart.Location = new System.Drawing.Point(3, 3);
            this.ttChart.Name = "ttChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Estimated Top Temp";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "Estimated Bottom Temp";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Legend = "Legend1";
            series3.Name = "Measured Top Temp";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Legend = "Legend1";
            series4.Name = "Measured Bottom Temp";
            this.ttChart.Series.Add(series1);
            this.ttChart.Series.Add(series2);
            this.ttChart.Series.Add(series3);
            this.ttChart.Series.Add(series4);
            this.ttChart.Size = new System.Drawing.Size(794, 187);
            this.ttChart.TabIndex = 0;
            this.ttChart.Text = "chart1";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.stopBtn, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.runBtn, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(497, 196);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(300, 44);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // stopBtn
            // 
            this.stopBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stopBtn.Location = new System.Drawing.Point(153, 3);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(144, 38);
            this.stopBtn.TabIndex = 0;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // runBtn
            // 
            this.runBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runBtn.Location = new System.Drawing.Point(3, 3);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(144, 38);
            this.runBtn.TabIndex = 1;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM4";
            this.serialPort1.StopBits = System.IO.Ports.StopBits.Two;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // bgw
            // 
            this.bgw.WorkerReportsProgress = true;
            this.bgw.WorkerSupportsCancellation = true;
            this.bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_DoWork);
            this.bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgw_ProgressChanged);
            this.bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 544);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Scintia Thermocycler";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ttChart)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.DataVisualization.Charting.Chart ttChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button addSBtn;
        private System.Windows.Forms.Button addCBtn;
        private System.Windows.Forms.TreeView stepsList;
        private System.Windows.Forms.Button remSelctedBtn;
        private System.Windows.Forms.Button editSelectedBtn;
        private System.IO.Ports.SerialPort serialPort1;
        private System.ComponentModel.BackgroundWorker bgw;
    }
}

