namespace RFID
{
    partial class ConnectForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.iptext = new System.Windows.Forms.TextBox();
            this.porttext = new System.Windows.Forms.TextBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ip Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(248, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // iptext
            // 
            this.iptext.Location = new System.Drawing.Point(76, 10);
            this.iptext.Name = "iptext";
            this.iptext.Size = new System.Drawing.Size(100, 20);
            this.iptext.TabIndex = 2;
            this.iptext.Text = "169.254.10.1";
            // 
            // porttext
            // 
            this.porttext.Location = new System.Drawing.Point(289, 10);
            this.porttext.Name = "porttext";
            this.porttext.Size = new System.Drawing.Size(100, 20);
            this.porttext.TabIndex = 3;
            this.porttext.Text = "5084";
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(122, 58);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(192, 48);
            this.connectBtn.TabIndex = 4;
            this.connectBtn.Text = "Connect!";
            this.connectBtn.UseVisualStyleBackColor = true;
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 150);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.porttext);
            this.Controls.Add(this.iptext);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ConnectForm";
            this.Text = "ConnectForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox iptext;
        private System.Windows.Forms.TextBox porttext;
        private System.Windows.Forms.Button connectBtn;
    }
}