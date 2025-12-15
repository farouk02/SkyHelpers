namespace SkyHelpers.Common
{
    partial class ImageCropperForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelImage = new System.Windows.Forms.Panel();
            panelOverlay = new TransparentPanel();
            picImage = new System.Windows.Forms.PictureBox();
            panelControls = new System.Windows.Forms.Panel();
            lblZoom = new System.Windows.Forms.Label();
            trackZoom = new System.Windows.Forms.TrackBar();
            btnConfirm = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackZoom).BeginInit();
            panelImage.SuspendLayout();
            panelControls.SuspendLayout();
            SuspendLayout();
            // 
            // panelImage
            // 
            panelImage.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            panelImage.Controls.Add(panelOverlay);
            panelImage.Controls.Add(picImage);
            panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            panelImage.Location = new System.Drawing.Point(0, 0);
            panelImage.Name = "panelImage";
            panelImage.Size = new System.Drawing.Size(600, 500);
            panelImage.TabIndex = 0;
            // 
            // panelOverlay
            // 
            panelOverlay.BackColor = System.Drawing.Color.Transparent;
            panelOverlay.Cursor = System.Windows.Forms.Cursors.SizeAll;
            panelOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            panelOverlay.Location = new System.Drawing.Point(0, 0);
            panelOverlay.Name = "panelOverlay";
            panelOverlay.Size = new System.Drawing.Size(600, 500);
            panelOverlay.TabIndex = 1;
            panelOverlay.Paint += PanelOverlay_Paint;
            panelOverlay.MouseDown += PanelOverlay_MouseDown;
            panelOverlay.MouseMove += PanelOverlay_MouseMove;
            panelOverlay.MouseUp += PanelOverlay_MouseUp;
            // 
            // picImage
            // 
            picImage.BackColor = System.Drawing.Color.Transparent;
            picImage.Location = new System.Drawing.Point(50, 50);
            picImage.Name = "picImage";
            picImage.Size = new System.Drawing.Size(500, 400);
            picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picImage.TabIndex = 0;
            picImage.TabStop = false;
            // 
            // panelControls
            // 
            panelControls.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            panelControls.Controls.Add(lblZoom);
            panelControls.Controls.Add(trackZoom);
            panelControls.Controls.Add(btnConfirm);
            panelControls.Controls.Add(btnCancel);
            panelControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelControls.Location = new System.Drawing.Point(0, 500);
            panelControls.Name = "panelControls";
            panelControls.Size = new System.Drawing.Size(600, 60);
            panelControls.TabIndex = 1;
            // 
            // lblZoom
            // 
            lblZoom.AutoSize = true;
            lblZoom.ForeColor = System.Drawing.Color.White;
            lblZoom.Location = new System.Drawing.Point(12, 22);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new System.Drawing.Size(34, 13);
            lblZoom.TabIndex = 0;
            lblZoom.Text = "Zoom";
            // 
            // trackZoom
            // 
            trackZoom.Location = new System.Drawing.Point(52, 12);
            trackZoom.Maximum = 300;
            trackZoom.Minimum = 50;
            trackZoom.Name = "trackZoom";
            trackZoom.Size = new System.Drawing.Size(250, 45);
            trackZoom.TabIndex = 1;
            trackZoom.TickFrequency = 25;
            trackZoom.Value = 100;
            trackZoom.Scroll += TrackZoom_Scroll;
            // 
            // btnConfirm
            // 
            btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Right;
            btnConfirm.BackColor = System.Drawing.Color.FromArgb(66, 133, 244);
            btnConfirm.ForeColor = System.Drawing.Color.White;
            btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Location = new System.Drawing.Point(400, 15);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new System.Drawing.Size(90, 30);
            btnConfirm.TabIndex = 2;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += BtnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(496, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(90, 30);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // ImageCropperForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            ClientSize = new System.Drawing.Size(600, 560);
            Controls.Add(panelImage);
            Controls.Add(panelControls);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImageCropperForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Crop Image";
            Load += ImageCropperForm_Load;
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackZoom).EndInit();
            panelImage.ResumeLayout(false);
            panelControls.ResumeLayout(false);
            panelControls.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelImage;
        private TransparentPanel panelOverlay;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
    }
}
