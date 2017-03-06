namespace OSERV_BASE
{
    partial class EndDebug
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EndDebug));
            this.endDebugBtn = new System.Windows.Forms.Button();
            this.ics_setup = new System.Windows.Forms.Button();
            this.startGLOWscriptBtn = new System.Windows.Forms.Button();
            this.startGLOWscript = new System.Windows.Forms.TextBox();
            this.runEvent = new System.Windows.Forms.ComboBox();
            this.runEventBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ddwrt_radio_on = new System.Windows.Forms.Button();
            this.ddwrt_radio_off = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // endDebugBtn
            // 
            this.endDebugBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endDebugBtn.ForeColor = System.Drawing.Color.Maroon;
            this.endDebugBtn.Location = new System.Drawing.Point(12, 8);
            this.endDebugBtn.Name = "endDebugBtn";
            this.endDebugBtn.Size = new System.Drawing.Size(148, 32);
            this.endDebugBtn.TabIndex = 0;
            this.endDebugBtn.Text = "End Debug";
            this.endDebugBtn.UseVisualStyleBackColor = true;
            this.endDebugBtn.Click += new System.EventHandler(this.endDebugBtn_Click);
            // 
            // ics_setup
            // 
            this.ics_setup.Location = new System.Drawing.Point(12, 115);
            this.ics_setup.Name = "ics_setup";
            this.ics_setup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ics_setup.Size = new System.Drawing.Size(148, 23);
            this.ics_setup.TabIndex = 2;
            this.ics_setup.Text = "Setup ICS";
            this.ics_setup.UseVisualStyleBackColor = true;
            this.ics_setup.Click += new System.EventHandler(this.ics_setup_Click);
            // 
            // startGLOWscriptBtn
            // 
            this.startGLOWscriptBtn.Location = new System.Drawing.Point(99, 86);
            this.startGLOWscriptBtn.Name = "startGLOWscriptBtn";
            this.startGLOWscriptBtn.Size = new System.Drawing.Size(61, 23);
            this.startGLOWscriptBtn.TabIndex = 7;
            this.startGLOWscriptBtn.Text = "GLOW-IT";
            this.startGLOWscriptBtn.UseVisualStyleBackColor = true;
            this.startGLOWscriptBtn.Click += new System.EventHandler(this.startGLOWscriptBtn_Click);
            // 
            // startGLOWscript
            // 
            this.startGLOWscript.Location = new System.Drawing.Point(13, 88);
            this.startGLOWscript.Name = "startGLOWscript";
            this.startGLOWscript.Size = new System.Drawing.Size(87, 20);
            this.startGLOWscript.TabIndex = 8;
            // 
            // runEvent
            // 
            this.runEvent.FormattingEnabled = true;
            this.runEvent.Items.AddRange(new object[] {
            "logon",
            "logoff",
            "lock",
            "unlock",
            "networkchange",
            "suspend",
            "resume"});
            this.runEvent.Location = new System.Drawing.Point(13, 61);
            this.runEvent.Name = "runEvent";
            this.runEvent.Size = new System.Drawing.Size(95, 21);
            this.runEvent.TabIndex = 9;
            // 
            // runEventBtn
            // 
            this.runEventBtn.Location = new System.Drawing.Point(107, 59);
            this.runEventBtn.Name = "runEventBtn";
            this.runEventBtn.Size = new System.Drawing.Size(53, 23);
            this.runEventBtn.TabIndex = 10;
            this.runEventBtn.Text = "EVNT";
            this.runEventBtn.UseVisualStyleBackColor = true;
            this.runEventBtn.Click += new System.EventHandler(this.runEventBtn_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 2);
            this.label1.TabIndex = 11;
            this.label1.Text = " ";
            // 
            // ddwrt_radio_on
            // 
            this.ddwrt_radio_on.Location = new System.Drawing.Point(13, 144);
            this.ddwrt_radio_on.Name = "ddwrt_radio_on";
            this.ddwrt_radio_on.Size = new System.Drawing.Size(148, 23);
            this.ddwrt_radio_on.TabIndex = 12;
            this.ddwrt_radio_on.Text = "DD-WRT WIFI RADIO ON";
            this.ddwrt_radio_on.UseVisualStyleBackColor = true;
            this.ddwrt_radio_on.Click += new System.EventHandler(this.ddwrt_radio_on_Click);
            // 
            // ddwrt_radio_off
            // 
            this.ddwrt_radio_off.Location = new System.Drawing.Point(13, 173);
            this.ddwrt_radio_off.Name = "ddwrt_radio_off";
            this.ddwrt_radio_off.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ddwrt_radio_off.Size = new System.Drawing.Size(148, 23);
            this.ddwrt_radio_off.TabIndex = 13;
            this.ddwrt_radio_off.Text = "DD-WRT WIFI RADIO OFF";
            this.ddwrt_radio_off.UseVisualStyleBackColor = true;
            this.ddwrt_radio_off.Click += new System.EventHandler(this.ddwrt_radio_off_Click);
            // 
            // EndDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(172, 205);
            this.ControlBox = false;
            this.Controls.Add(this.ddwrt_radio_off);
            this.Controls.Add(this.ddwrt_radio_on);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.runEventBtn);
            this.Controls.Add(this.runEvent);
            this.Controls.Add(this.startGLOWscript);
            this.Controls.Add(this.startGLOWscriptBtn);
            this.Controls.Add(this.ics_setup);
            this.Controls.Add(this.endDebugBtn);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EndDebug";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IRIS-Service Debug";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EndDebug_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button endDebugBtn;
        private System.Windows.Forms.Button ics_setup;
        private System.Windows.Forms.Button startGLOWscriptBtn;
        private System.Windows.Forms.TextBox startGLOWscript;
        private System.Windows.Forms.ComboBox runEvent;
        private System.Windows.Forms.Button runEventBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ddwrt_radio_on;
        private System.Windows.Forms.Button ddwrt_radio_off;
    }
}