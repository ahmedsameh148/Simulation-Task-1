namespace MultiQueueSimulation
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.readFromFile = new System.Windows.Forms.Button();
            this.runTestCase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(882, 267);
            this.dataGridView1.TabIndex = 0;
            // 
            // readFromFile
            // 
            this.readFromFile.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.readFromFile.Location = new System.Drawing.Point(313, 285);
            this.readFromFile.Name = "readFromFile";
            this.readFromFile.Size = new System.Drawing.Size(99, 35);
            this.readFromFile.TabIndex = 1;
            this.readFromFile.Text = "Read From File";
            this.readFromFile.UseVisualStyleBackColor = true;
            // 
            // runTestCase
            // 
            this.runTestCase.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.runTestCase.Location = new System.Drawing.Point(498, 285);
            this.runTestCase.Name = "runTestCase";
            this.runTestCase.Size = new System.Drawing.Size(99, 35);
            this.runTestCase.TabIndex = 2;
            this.runTestCase.Text = "Run Test Case";
            this.runTestCase.UseVisualStyleBackColor = true;
            this.runTestCase.Click += new System.EventHandler(this.runTestCase_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 327);
            this.Controls.Add(this.runTestCase);
            this.Controls.Add(this.readFromFile);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button readFromFile;
        private System.Windows.Forms.Button runTestCase;
    }
}

