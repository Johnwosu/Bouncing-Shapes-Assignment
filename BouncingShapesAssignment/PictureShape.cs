using System.Drawing;
using System.Windows.Forms;

namespace BouncingShapesAssignment
{
    public class PictureShape : Shape
    {
        private PictureBox pictureBox;
        private Bitmap picture;

        public PictureShape(int x, int y, Panel panel)
            : base(x, y, 55, 55)
        {
            // Create a simple picture without an external image file.
            picture = new Bitmap(55, 55);

            using (Graphics graphics = Graphics.FromImage(picture))
            {
                graphics.Clear(Color.MidnightBlue);

                graphics.FillEllipse(
                    Brushes.Gold, 3, 3, 49, 49);

                graphics.FillEllipse(
                    Brushes.Black, 15, 17, 6, 8);

                graphics.FillEllipse(
                    Brushes.Black, 34, 17, 6, 8);

                using (Pen pen = new Pen(Color.Black, 3))
                {
                    graphics.DrawArc(
                        pen, 15, 25, 25, 16, 0, 180);
                }
            }

            pictureBox = new PictureBox();
            pictureBox.Size = new Size(Width, Height);
            pictureBox.Location = new Point(X, Y);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = picture;
            pictureBox.Enabled = false;

            panel.Controls.Add(pictureBox);
        }

        public override void Draw(Graphics graphics)
        {
            // A real PictureBox draws its own image.
        }

        public override void UpdateControl()
        {
            pictureBox.Location = new Point(X, Y);
        }

        // Visual change 4: rotate the displayed image.
        public override void ChangeAppearance()
        {
            picture.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox.Invalidate();
        }

        public override void Remove()
        {
            pictureBox.Image = null;
            pictureBox.Dispose();
            picture.Dispose();
        }
    }
}