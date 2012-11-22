namespace LocalMapper
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
            this.lstTasks = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstApps = new System.Windows.Forms.ComboBox();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.btnLog = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddActProf = new System.Windows.Forms.Button();
            this.lstMappedApps = new System.Windows.Forms.ListBox();
            this.btnRemActProf = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstTasks
            // 
            this.lstTasks.FormattingEnabled = true;
            this.lstTasks.Location = new System.Drawing.Point(12, 49);
            this.lstTasks.Name = "lstTasks";
            this.lstTasks.Size = new System.Drawing.Size(334, 69);
            this.lstTasks.TabIndex = 0;
            this.lstTasks.SelectedIndexChanged += new System.EventHandler(this.lstTasks_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tareas";
            // 
            // lstApps
            // 
            this.lstApps.FormattingEnabled = true;
            this.lstApps.Location = new System.Drawing.Point(73, 224);
            this.lstApps.Name = "lstApps";
            this.lstApps.Size = new System.Drawing.Size(273, 21);
            this.lstApps.TabIndex = 2;
            this.lstApps.SelectedIndexChanged += new System.EventHandler(this.lstApps_SelectedIndexChanged);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(73, 257);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(239, 20);
            this.txtKeyword.TabIndex = 3;
            this.txtKeyword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtKeyword_KeyUp);
            // 
            // btnLog
            // 
            this.btnLog.FlatAppearance.BorderSize = 0;
            this.btnLog.Location = new System.Drawing.Point(245, 4);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(75, 23);
            this.btnLog.TabIndex = 4;
            this.btnLog.Text = "registra";
            this.btnLog.UseVisualStyleBackColor = true;
            this.btnLog.Visible = false;
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "autenticado";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAddActProf
            // 
            this.btnAddActProf.Location = new System.Drawing.Point(325, 258);
            this.btnAddActProf.Name = "btnAddActProf";
            this.btnAddActProf.Size = new System.Drawing.Size(21, 23);
            this.btnAddActProf.TabIndex = 6;
            this.btnAddActProf.Text = "+";
            this.btnAddActProf.UseVisualStyleBackColor = true;
            this.btnAddActProf.Visible = false;
            this.btnAddActProf.Click += new System.EventHandler(this.btnAddActProf_Click);
            // 
            // lstMappedApps
            // 
            this.lstMappedApps.FormattingEnabled = true;
            this.lstMappedApps.Location = new System.Drawing.Point(12, 136);
            this.lstMappedApps.Name = "lstMappedApps";
            this.lstMappedApps.Size = new System.Drawing.Size(309, 82);
            this.lstMappedApps.TabIndex = 7;
            this.lstMappedApps.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstMappedApps_KeyUp);
            // 
            // btnRemActProf
            // 
            this.btnRemActProf.Location = new System.Drawing.Point(327, 136);
            this.btnRemActProf.Name = "btnRemActProf";
            this.btnRemActProf.Size = new System.Drawing.Size(19, 23);
            this.btnRemActProf.TabIndex = 8;
            this.btnRemActProf.Text = "-";
            this.btnRemActProf.UseVisualStyleBackColor = true;
            this.btnRemActProf.Visible = false;
            this.btnRemActProf.Click += new System.EventHandler(this.btnRemActProf_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Programa";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Expresion";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(358, 291);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRemActProf);
            this.Controls.Add(this.lstMappedApps);
            this.Controls.Add(this.btnAddActProf);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.txtKeyword);
            this.Controls.Add(this.lstApps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstTasks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[ nose ]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstTasks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstApps;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAddActProf;
        private System.Windows.Forms.ListBox lstMappedApps;
        private System.Windows.Forms.Button btnRemActProf;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

