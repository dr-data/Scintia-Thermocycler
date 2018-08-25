namespace Scintia_Thermocycler
{
    partial class addStepDlg
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
            this.addStepDlgT = new System.Windows.Forms.TextBox();
            this.addStepDlgD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.okAddStepBtn = new System.Windows.Forms.Button();
            this.cnclStepBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addStepDlgT
            // 
            this.addStepDlgT.Location = new System.Drawing.Point(12, 49);
            this.addStepDlgT.Name = "addStepDlgT";
            this.addStepDlgT.Size = new System.Drawing.Size(415, 22);
            this.addStepDlgT.TabIndex = 0;
            // 
            // addStepDlgD
            // 
            this.addStepDlgD.Location = new System.Drawing.Point(12, 95);
            this.addStepDlgD.Name = "addStepDlgD";
            this.addStepDlgD.Size = new System.Drawing.Size(415, 22);
            this.addStepDlgD.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Temperature";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Duration";
            // 
            // okAddStepBtn
            // 
            this.okAddStepBtn.Location = new System.Drawing.Point(252, 218);
            this.okAddStepBtn.Name = "okAddStepBtn";
            this.okAddStepBtn.Size = new System.Drawing.Size(75, 23);
            this.okAddStepBtn.TabIndex = 4;
            this.okAddStepBtn.Text = "OK";
            this.okAddStepBtn.UseVisualStyleBackColor = true;
            this.okAddStepBtn.Click += new System.EventHandler(this.okAddStepBtn_Click);
            // 
            // cnclStepBtn
            // 
            this.cnclStepBtn.Location = new System.Drawing.Point(342, 218);
            this.cnclStepBtn.Name = "cnclStepBtn";
            this.cnclStepBtn.Size = new System.Drawing.Size(75, 23);
            this.cnclStepBtn.TabIndex = 5;
            this.cnclStepBtn.Text = "Cancel";
            this.cnclStepBtn.UseVisualStyleBackColor = true;
            this.cnclStepBtn.Click += new System.EventHandler(this.cnclStepBtn_Click);
            // 
            // addStepDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 253);
            this.Controls.Add(this.cnclStepBtn);
            this.Controls.Add(this.okAddStepBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addStepDlgD);
            this.Controls.Add(this.addStepDlgT);
            this.Name = "addStepDlg";
            this.Text = "Add Step";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox addStepDlgT;
        private System.Windows.Forms.TextBox addStepDlgD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button okAddStepBtn;
        private System.Windows.Forms.Button cnclStepBtn;
    }
}