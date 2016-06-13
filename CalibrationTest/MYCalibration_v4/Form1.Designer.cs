namespace MYCalibration_v4
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.buttonCorrection = new System.Windows.Forms.Button();
            this.textBoxZangle = new System.Windows.Forms.TextBox();
            this.textBoxYangle = new System.Windows.Forms.TextBox();
            this.textBoxXangle = new System.Windows.Forms.TextBox();
            this.buttonReinit = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonSpectatorCam = new System.Windows.Forms.Button();
            this.buttonSurfaceCam = new System.Windows.Forms.Button();
            this.buttonCamCalib = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.glControl1 = new OpenTK.GLControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.textBox8);
            this.panel1.Controls.Add(this.textBox7);
            this.panel1.Controls.Add(this.textBox6);
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.buttonCorrection);
            this.panel1.Controls.Add(this.textBoxZangle);
            this.panel1.Controls.Add(this.textBoxYangle);
            this.panel1.Controls.Add(this.textBoxXangle);
            this.panel1.Controls.Add(this.buttonReinit);
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.buttonSpectatorCam);
            this.panel1.Controls.Add(this.buttonSurfaceCam);
            this.panel1.Controls.Add(this.buttonCamCalib);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 579);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1089, 77);
            this.panel1.TabIndex = 0;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBox7.Location = new System.Drawing.Point(871, 52);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(194, 22);
            this.textBox7.TabIndex = 16;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.PaleGreen;
            this.textBox6.Location = new System.Drawing.Point(659, 54);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(206, 22);
            this.textBox6.TabIndex = 15;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.IndianRed;
            this.textBox5.Location = new System.Drawing.Point(447, 55);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(206, 22);
            this.textBox5.TabIndex = 14;
            // 
            // buttonCorrection
            // 
            this.buttonCorrection.Location = new System.Drawing.Point(84, 53);
            this.buttonCorrection.Name = "buttonCorrection";
            this.buttonCorrection.Size = new System.Drawing.Size(87, 23);
            this.buttonCorrection.TabIndex = 13;
            this.buttonCorrection.Text = "CORRIGER";
            this.buttonCorrection.UseVisualStyleBackColor = true;
            this.buttonCorrection.Click += new System.EventHandler(this.buttonCorrection_Click);
            // 
            // textBoxZangle
            // 
            this.textBoxZangle.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBoxZangle.Location = new System.Drawing.Point(871, 31);
            this.textBoxZangle.Name = "textBoxZangle";
            this.textBoxZangle.Size = new System.Drawing.Size(194, 22);
            this.textBoxZangle.TabIndex = 12;
            // 
            // textBoxYangle
            // 
            this.textBoxYangle.BackColor = System.Drawing.Color.PaleGreen;
            this.textBoxYangle.Location = new System.Drawing.Point(659, 32);
            this.textBoxYangle.Name = "textBoxYangle";
            this.textBoxYangle.Size = new System.Drawing.Size(206, 22);
            this.textBoxYangle.TabIndex = 11;
            // 
            // textBoxXangle
            // 
            this.textBoxXangle.BackColor = System.Drawing.Color.IndianRed;
            this.textBoxXangle.Location = new System.Drawing.Point(447, 32);
            this.textBoxXangle.Name = "textBoxXangle";
            this.textBoxXangle.Size = new System.Drawing.Size(206, 22);
            this.textBoxXangle.TabIndex = 10;
            // 
            // buttonReinit
            // 
            this.buttonReinit.Location = new System.Drawing.Point(349, 25);
            this.buttonReinit.Name = "buttonReinit";
            this.buttonReinit.Size = new System.Drawing.Size(92, 23);
            this.buttonReinit.TabIndex = 9;
            this.buttonReinit.Text = "Reinitialiser";
            this.buttonReinit.UseVisualStyleBackColor = true;
            this.buttonReinit.Click += new System.EventHandler(this.buttonReinit_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(765, 4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 22);
            this.textBox4.TabIndex = 8;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(659, 4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 22);
            this.textBox3.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(553, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 22);
            this.textBox2.TabIndex = 6;
            // 
            // buttonSpectatorCam
            // 
            this.buttonSpectatorCam.Location = new System.Drawing.Point(259, 3);
            this.buttonSpectatorCam.Name = "buttonSpectatorCam";
            this.buttonSpectatorCam.Size = new System.Drawing.Size(84, 48);
            this.buttonSpectatorCam.TabIndex = 5;
            this.buttonSpectatorCam.Text = "Spectator Cam";
            this.buttonSpectatorCam.UseVisualStyleBackColor = true;
            this.buttonSpectatorCam.Click += new System.EventHandler(this.buttonSpectatorCam_Click);
            // 
            // buttonSurfaceCam
            // 
            this.buttonSurfaceCam.Location = new System.Drawing.Point(177, 3);
            this.buttonSurfaceCam.Name = "buttonSurfaceCam";
            this.buttonSurfaceCam.Size = new System.Drawing.Size(75, 48);
            this.buttonSurfaceCam.TabIndex = 4;
            this.buttonSurfaceCam.Text = "Surface Cam";
            this.buttonSurfaceCam.UseVisualStyleBackColor = true;
            this.buttonSurfaceCam.Click += new System.EventHandler(this.buttonSurfaceCam_Click);
            // 
            // buttonCamCalib
            // 
            this.buttonCamCalib.Location = new System.Drawing.Point(84, 3);
            this.buttonCamCalib.Name = "buttonCamCalib";
            this.buttonCamCalib.Size = new System.Drawing.Size(87, 48);
            this.buttonCamCalib.TabIndex = 3;
            this.buttonCamCalib.Text = "Calibrated Cam";
            this.buttonCamCalib.UseVisualStyleBackColor = true;
            this.buttonCamCalib.Click += new System.EventHandler(this.buttonCamCalib_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(349, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "CALIBRER";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(447, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 50);
            this.button1.TabIndex = 0;
            this.button1.Text = "Charger image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.DarkGray;
            this.glControl1.Location = new System.Drawing.Point(-3, 4);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(315, 182);
            this.glControl1.TabIndex = 1;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseClick);
            this.glControl1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.glControl1_PreviewKeyDown);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.glControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1089, 570);
            this.panel2.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.55691F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.4431F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1095, 659);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(919, 4);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(100, 22);
            this.textBox8.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 659);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonSpectatorCam;
        private System.Windows.Forms.Button buttonSurfaceCam;
        private System.Windows.Forms.Button buttonCamCalib;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button buttonReinit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxZangle;
        private System.Windows.Forms.TextBox textBoxYangle;
        private System.Windows.Forms.TextBox textBoxXangle;
        private System.Windows.Forms.Button buttonCorrection;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox8;
    }
}

