namespace Scintia_Thermocycler
{
    partial class addCycleDlg
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.addCycleDlgR = new System.Windows.Forms.TextBox();
            this.addCycleDlgN = new System.Windows.Forms.TextBox();
            this.cnclStepBtn = new System.Windows.Forms.Button();
            this.okAddStepBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Repetitions";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Cycle Name";
            // 
            // addCycleDlgR
            // 
            this.addCycleDlgR.Location = new System.Drawing.Point(12, 86);
            this.addCycleDlgR.Name = "addCycleDlgR";
            this.addCycleDlgR.Size = new System.Drawing.Size(415, 22);
            this.addCycleDlgR.TabIndex = 5;
            // 
            // addCycleDlgN
            // 
            this.addCycleDlgN.Location = new System.Drawing.Point(12, 40);
            this.addCycleDlgN.Name = "addCycleDlgN";
            this.addCycleDlgN.Size = new System.Drawing.Size(415, 22);
            this.addCycleDlgN.TabIndex = 4;
            // 
            // cnclStepBtn
            // 
            this.cnclStepBtn.Location = new System.Drawing.Point(392, 204);
            this.cnclStepBtn.Name = "cnclStepBtn";
            this.cnclStepBtn.Size = new System.Drawing.Size(75, 23);
            this.cnclStepBtn.TabIndex = 9;
            this.cnclStepBtn.Text = "Cancel";
            this.cnclStepBtn.UseVisualStyleBackColor = true;
            this.cnclStepBtn.Click += new System.EventHandler(this.cnclStepBtn_Click);
            // 
            // okAddStepBtn
            // 
            this.okAddStepBtn.Location = new System.Drawing.Point(302, 204);
            this.okAddStepBtn.Name = "okAddStepBtn";
            this.okAddStepBtn.Size = new System.Drawing.Size(75, 23);
            this.okAddStepBtn.TabIndex = 8;
            this.okAddStepBtn.Text = "OK";
            this.okAddStepBtn.UseVisualStyleBackColor = true;
            this.okAddStepBtn.Click += new System.EventHandler(this.okAddStepBtn_Click);
            // 
            // addCycleDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 239);
            this.Controls.Add(this.cnclStepBtn);
            this.Controls.Add(this.okAddStepBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addCycleDlgR);
            this.Controls.Add(this.addCycleDlgN);
            this.Name = "addCycleDlg";
            this.Text = "addCycleDlg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox addCycleDlgR;
        private System.Windows.Forms.TextBox addCycleDlgN;
        private System.Windows.Forms.Button cnclStepBtn;
        private System.Windows.Forms.Button okAddStepBtn;

    }
}