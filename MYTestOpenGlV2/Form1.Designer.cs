namespace MYTestOpenGlV2
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
            this.up = new System.Windows.Forms.Label();
            this.target = new System.Windows.Forms.Label();
            this.eye = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxTarget = new System.Windows.Forms.TextBox();
            this.textBoxUp = new System.Windows.Forms.TextBox();
            this.textBoxEye = new System.Windows.Forms.TextBox();
            this.labelMat = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxM11 = new System.Windows.Forms.TextBox();
            this.textBoxM12 = new System.Windows.Forms.TextBox();
            this.textBoxM13 = new System.Windows.Forms.TextBox();
            this.textBoxM21 = new System.Windows.Forms.TextBox();
            this.textBoxM22 = new System.Windows.Forms.TextBox();
            this.textBoxM23 = new System.Windows.Forms.TextBox();
            this.textBoxM31 = new System.Windows.Forms.TextBox();
            this.textBoxM32 = new System.Windows.Forms.TextBox();
            this.textBoxM33 = new System.Windows.Forms.TextBox();
            this.simpleOpenGlControl1 = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.up);
            this.panel1.Controls.Add(this.target);
            this.panel1.Controls.Add(this.eye);
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.labelMat);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 474);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1104, 108);
            this.panel1.TabIndex = 0;
            // 
            // up
            // 
            this.up.AutoSize = true;
            this.up.Location = new System.Drawing.Point(663, 71);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(24, 17);
            this.up.TabIndex = 5;
            this.up.Text = "up";
            // 
            // target
            // 
            this.target.AutoSize = true;
            this.target.Location = new System.Drawing.Point(663, 40);
            this.target.Name = "target";
            this.target.Size = new System.Drawing.Size(45, 17);
            this.target.TabIndex = 4;
            this.target.Text = "target";
            // 
            // eye
            // 
            this.eye.AutoSize = true;
            this.eye.Location = new System.Drawing.Point(660, 13);
            this.eye.Name = "eye";
            this.eye.Size = new System.Drawing.Size(31, 17);
            this.eye.TabIndex = 3;
            this.eye.Text = "eye";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxTarget, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxUp, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxEye, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(767, 13);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.66667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(299, 83);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // textBoxTarget
            // 
            this.textBoxTarget.Location = new System.Drawing.Point(3, 29);
            this.textBoxTarget.Name = "textBoxTarget";
            this.textBoxTarget.Size = new System.Drawing.Size(293, 22);
            this.textBoxTarget.TabIndex = 1;
            // 
            // textBoxUp
            // 
            this.textBoxUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUp.Location = new System.Drawing.Point(3, 58);
            this.textBoxUp.Name = "textBoxUp";
            this.textBoxUp.Size = new System.Drawing.Size(293, 22);
            this.textBoxUp.TabIndex = 2;
            // 
            // textBoxEye
            // 
            this.textBoxEye.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEye.Location = new System.Drawing.Point(3, 3);
            this.textBoxEye.Name = "textBoxEye";
            this.textBoxEye.Size = new System.Drawing.Size(293, 22);
            this.textBoxEye.TabIndex = 0;
            // 
            // labelMat
            // 
            this.labelMat.AutoSize = true;
            this.labelMat.Location = new System.Drawing.Point(38, 36);
            this.labelMat.Name = "labelMat";
            this.labelMat.Size = new System.Drawing.Size(134, 17);
            this.labelMat.TabIndex = 1;
            this.labelMat.Text = "Matrice de rotation :";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.88889F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxM11, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM12, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM13, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM21, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM22, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM23, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM31, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM32, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxM33, 2, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(197, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(195, 90);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBoxM11
            // 
            this.textBoxM11.Location = new System.Drawing.Point(3, 3);
            this.textBoxM11.Name = "textBoxM11";
            this.textBoxM11.Size = new System.Drawing.Size(54, 22);
            this.textBoxM11.TabIndex = 9;
            // 
            // textBoxM12
            // 
            this.textBoxM12.Location = new System.Drawing.Point(63, 3);
            this.textBoxM12.Name = "textBoxM12";
            this.textBoxM12.Size = new System.Drawing.Size(57, 22);
            this.textBoxM12.TabIndex = 10;
            // 
            // textBoxM13
            // 
            this.textBoxM13.Location = new System.Drawing.Point(126, 3);
            this.textBoxM13.Name = "textBoxM13";
            this.textBoxM13.Size = new System.Drawing.Size(58, 22);
            this.textBoxM13.TabIndex = 11;
            // 
            // textBoxM21
            // 
            this.textBoxM21.Location = new System.Drawing.Point(3, 34);
            this.textBoxM21.Name = "textBoxM21";
            this.textBoxM21.Size = new System.Drawing.Size(54, 22);
            this.textBoxM21.TabIndex = 12;
            // 
            // textBoxM22
            // 
            this.textBoxM22.Location = new System.Drawing.Point(63, 34);
            this.textBoxM22.Name = "textBoxM22";
            this.textBoxM22.Size = new System.Drawing.Size(57, 22);
            this.textBoxM22.TabIndex = 13;
            // 
            // textBoxM23
            // 
            this.textBoxM23.Location = new System.Drawing.Point(126, 34);
            this.textBoxM23.Name = "textBoxM23";
            this.textBoxM23.Size = new System.Drawing.Size(58, 22);
            this.textBoxM23.TabIndex = 14;
            // 
            // textBoxM31
            // 
            this.textBoxM31.Location = new System.Drawing.Point(3, 65);
            this.textBoxM31.Name = "textBoxM31";
            this.textBoxM31.Size = new System.Drawing.Size(54, 22);
            this.textBoxM31.TabIndex = 15;
            // 
            // textBoxM32
            // 
            this.textBoxM32.Location = new System.Drawing.Point(63, 65);
            this.textBoxM32.Name = "textBoxM32";
            this.textBoxM32.Size = new System.Drawing.Size(57, 22);
            this.textBoxM32.TabIndex = 16;
            // 
            // textBoxM33
            // 
            this.textBoxM33.Location = new System.Drawing.Point(126, 65);
            this.textBoxM33.Name = "textBoxM33";
            this.textBoxM33.Size = new System.Drawing.Size(58, 22);
            this.textBoxM33.TabIndex = 17;
            // 
            // simpleOpenGlControl1
            // 
            this.simpleOpenGlControl1.AccumBits = ((byte)(0));
            this.simpleOpenGlControl1.AutoCheckErrors = false;
            this.simpleOpenGlControl1.AutoFinish = false;
            this.simpleOpenGlControl1.AutoMakeCurrent = true;
            this.simpleOpenGlControl1.AutoSwapBuffers = true;
            this.simpleOpenGlControl1.BackColor = System.Drawing.Color.Black;
            this.simpleOpenGlControl1.ColorBits = ((byte)(32));
            this.simpleOpenGlControl1.DepthBits = ((byte)(16));
            this.simpleOpenGlControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleOpenGlControl1.Location = new System.Drawing.Point(0, 0);
            this.simpleOpenGlControl1.Name = "simpleOpenGlControl1";
            this.simpleOpenGlControl1.Size = new System.Drawing.Size(1104, 474);
            this.simpleOpenGlControl1.StencilBits = ((byte)(0));
            this.simpleOpenGlControl1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 582);
            this.Controls.Add(this.simpleOpenGlControl1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Tao.Platform.Windows.SimpleOpenGlControl simpleOpenGlControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelMat;
        private System.Windows.Forms.TextBox textBoxM11;
        private System.Windows.Forms.TextBox textBoxM12;
        private System.Windows.Forms.TextBox textBoxM13;
        private System.Windows.Forms.TextBox textBoxM21;
        private System.Windows.Forms.TextBox textBoxM22;
        private System.Windows.Forms.TextBox textBoxM23;
        private System.Windows.Forms.TextBox textBoxM31;
        private System.Windows.Forms.TextBox textBoxM32;
        private System.Windows.Forms.TextBox textBoxM33;
        private System.Windows.Forms.Label up;
        private System.Windows.Forms.Label target;
        private System.Windows.Forms.Label eye;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxEye;
        private System.Windows.Forms.TextBox textBoxTarget;
        private System.Windows.Forms.TextBox textBoxUp;
    }
}

