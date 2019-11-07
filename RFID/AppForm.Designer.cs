namespace RFID
{
    partial class AppForm
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
            this.inventoryList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.readBtn = new System.Windows.Forms.Button();
            this.functionCallStatusLabel = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionBtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.functionCallStatusLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.inventoryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.inventoryList.GridLines = true;
            this.inventoryList.Location = new System.Drawing.Point(23, 59);
            this.inventoryList.Name = "listView1";
            this.inventoryList.Size = new System.Drawing.Size(306, 189);
            this.inventoryList.TabIndex = 0;
            this.inventoryList.UseCompatibleStateImageBehavior = false;
            this.inventoryList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "EPC ID";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Time ";
            this.columnHeader2.Width = 100;
            // 
            // readBtn
            // 
            this.readBtn.Location = new System.Drawing.Point(198, 277);
            this.readBtn.Name = "readBtn";
            this.readBtn.Size = new System.Drawing.Size(131, 54);
            this.readBtn.TabIndex = 2;
            this.readBtn.Text = "Read";
            this.readBtn.UseVisualStyleBackColor = true;
            // 
            // functionCallStatusLabel
            // 
            this.functionCallStatusLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.functionCallStatusLabel.Location = new System.Drawing.Point(0, 404);
            this.functionCallStatusLabel.Name = "functionCallStatusLabel";
            this.functionCallStatusLabel.Size = new System.Drawing.Size(358, 22);
            this.functionCallStatusLabel.TabIndex = 3;
            this.functionCallStatusLabel.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // connectionBtn
            // 
            this.connectionBtn.Location = new System.Drawing.Point(23, 277);
            this.connectionBtn.Name = "connectionBtn";
            this.connectionBtn.Size = new System.Drawing.Size(126, 54);
            this.connectionBtn.TabIndex = 4;
            this.connectionBtn.Text = "Connect";
            this.connectionBtn.UseVisualStyleBackColor = true;
            this.connectionBtn.Click += new System.EventHandler(this.Button1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // AppForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 426);
            this.Controls.Add(this.connectionBtn);
            this.Controls.Add(this.functionCallStatusLabel);
            this.Controls.Add(this.readBtn);
            this.Controls.Add(this.inventoryList);
            this.Name = "AppForm";
            this.Text = "AppForm";
            this.functionCallStatusLabel.ResumeLayout(false);
            this.functionCallStatusLabel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ListView inventoryList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button readBtn;
        private System.Windows.Forms.StatusStrip functionCallStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button connectionBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}