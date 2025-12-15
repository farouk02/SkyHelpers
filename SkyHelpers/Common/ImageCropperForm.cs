using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkyHelpers.Common
{
    public partial class ImageCropperForm : Form
    {
        private Image _originalImage;
        private float _zoomFactor = 1.0f;
        private Point _imageOffset = Point.Empty;
        private Point _dragStart;
        private bool _isDragging = false;
        private int _cropSize = 400;
        private Rectangle _cropRect;

        public Image CroppedImage { get; private set; }

        public ImageCropperForm(Image image)
        {
            InitializeComponent();
            _originalImage = image;
        }

        private void ImageCropperForm_Load(object sender, EventArgs e)
        {
            // Center the crop area
            int centerX = (panelImage.Width - _cropSize) / 2;
            int centerY = (panelImage.Height - _cropSize) / 2;
            _cropRect = new Rectangle(centerX, centerY, _cropSize, _cropSize);

            // Initialize image position
            UpdateImageDisplay();
        }

        private void UpdateImageDisplay()
        {
            if (_originalImage == null) return;

            int scaledWidth = (int)(_originalImage.Width * _zoomFactor);
            int scaledHeight = (int)(_originalImage.Height * _zoomFactor);

            picImage.Size = new Size(scaledWidth, scaledHeight);
            picImage.Image = _originalImage;
            picImage.SizeMode = PictureBoxSizeMode.StretchImage;

            // Center in panel if no offset set
            if (_imageOffset == Point.Empty)
            {
                _imageOffset = new Point(
                    (panelImage.Width - scaledWidth) / 2,
                    (panelImage.Height - scaledHeight) / 2
                );
            }

            picImage.Location = _imageOffset;
            panelOverlay.Invalidate(); // Redraw overlay
        }

        private void TrackZoom_Scroll(object sender, EventArgs e)
        {
            float oldZoom = _zoomFactor;
            _zoomFactor = trackZoom.Value / 100f;

            // Adjust offset to keep image centered on crop area
            int cropCenterX = _cropRect.Left + _cropSize / 2;
            int cropCenterY = _cropRect.Top + _cropSize / 2;

            float zoomRatio = _zoomFactor / oldZoom;

            _imageOffset = new Point(
                cropCenterX - (int)((cropCenterX - picImage.Left) * zoomRatio),
                cropCenterY - (int)((cropCenterY - picImage.Top) * zoomRatio)
            );

            UpdateImageDisplay();
        }

        private void PanelOverlay_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragStart = e.Location;
                panelOverlay.Cursor = Cursors.SizeAll;
            }
        }

        private void PanelOverlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                int dx = e.X - _dragStart.X;
                int dy = e.Y - _dragStart.Y;

                _imageOffset = new Point(
                    picImage.Left + dx,
                    picImage.Top + dy
                );

                picImage.Location = _imageOffset;
                _dragStart = e.Location;
                panelOverlay.Invalidate();
            }
        }

        private void PanelOverlay_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            panelOverlay.Cursor = Cursors.Default;
        }

        private void PanelOverlay_Paint(object sender, PaintEventArgs e)
        {
            // Create a dark overlay with a transparent square in the center
            using var overlayPath = new GraphicsPath();
            
            // Add the entire panel as the region
            overlayPath.AddRectangle(new Rectangle(0, 0, panelOverlay.Width, panelOverlay.Height));
            
            // Cut out the square (this creates the transparent hole)
            overlayPath.AddRectangle(_cropRect);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Fill the overlay (dark with transparency)
            using var overlayBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
            e.Graphics.FillPath(overlayBrush, overlayPath);
            
            // Draw square border
            using var pen = new Pen(Color.White, 3);
            e.Graphics.DrawRectangle(pen, _cropRect);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            CroppedImage = CropImage();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private Image CropImage()
        {
            if (_originalImage == null) return null;

            // Calculate the crop rectangle relative to the original image
            float scaleX = (float)_originalImage.Width / picImage.Width;
            float scaleY = (float)_originalImage.Height / picImage.Height;

            int cropX = (int)((_cropRect.Left - picImage.Left) * scaleX);
            int cropY = (int)((_cropRect.Top - picImage.Top) * scaleY);
            int cropW = (int)(_cropSize * scaleX);
            int cropH = (int)(_cropSize * scaleY);

            // Clamp to image bounds
            cropX = Math.Max(0, cropX);
            cropY = Math.Max(0, cropY);
            cropW = Math.Min(cropW, _originalImage.Width - cropX);
            cropH = Math.Min(cropH, _originalImage.Height - cropY);

            // Ensure square
            int size = Math.Min(cropW, cropH);

            Rectangle cropRect = new Rectangle(cropX, cropY, size, size);

            Bitmap croppedBitmap = new Bitmap(_cropSize, _cropSize);
            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(_originalImage, new Rectangle(0, 0, _cropSize, _cropSize), cropRect, GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }
    }

    // Custom transparent panel that can draw on top of other controls
    public class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background
        }
    }
}
