namespace FormSignPad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axHWPenSign1 = new AxHWPenSignLib.AxHWPenSign();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axHWPenSign1)).BeginInit();
            this.SuspendLayout();
            // 
            // axHWPenSign1
            // 
            this.axHWPenSign1.Enabled = true;
            this.axHWPenSign1.Location = new System.Drawing.Point(1, 0);
            this.axHWPenSign1.Name = "axHWPenSign1";
            this.axHWPenSign1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axHWPenSign1.OcxState")));
            this.axHWPenSign1.Size = new System.Drawing.Size(508, 314);
            this.axHWPenSign1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(202, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Hide";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 392);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.axHWPenSign1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axHWPenSign1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxHWPenSignLib.AxHWPenSign axHWPenSign1;
        private System.Windows.Forms.Button button1;
    }
}

