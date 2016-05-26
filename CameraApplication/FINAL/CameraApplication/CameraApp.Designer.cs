namespace CameraApplication
{
    partial class CameraApp
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.shootButton = new System.Windows.Forms.Button();
            this.imageVideo = new Emgu.CV.UI.ImageBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.shootButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.imageVideo, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.82171F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.17829F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(978, 532);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // shootButton
            // 
            this.shootButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shootButton.Location = new System.Drawing.Point(3, 464);
            this.shootButton.Name = "shootButton";
            this.shootButton.Size = new System.Drawing.Size(972, 65);
            this.shootButton.TabIndex = 3;
            this.shootButton.Text = "Shoot";
            this.shootButton.UseVisualStyleBackColor = true;
            this.shootButton.Click += new System.EventHandler(this.shootButton_Click);
            // 
            // imageVideo
            // 
            this.imageVideo.BackColor = System.Drawing.SystemColors.Desktop;
            this.imageVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageVideo.Location = new System.Drawing.Point(3, 3);
            this.imageVideo.Name = "imageVideo";
            this.imageVideo.Size = new System.Drawing.Size(972, 455);
            this.imageVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.imageVideo.TabIndex = 2;
            this.imageVideo.TabStop = false;
            // 
            // CameraApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 532);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CameraApp";
            this.Text = "MY-Camera-App";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Emgu.CV.UI.ImageBox imageVideo;
        private System.Windows.Forms.Button shootButton;
    }
}

