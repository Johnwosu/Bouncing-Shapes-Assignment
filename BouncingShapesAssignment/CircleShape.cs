using System.Drawing;

namespace BouncingShapesAssignment
{
    public class CircleShape : Shape
    {
        public CircleShape(int x, int y)
            : base(x, y, 50, 50)
        {
            shapeColor = Color.DeepSkyBlue;
        }

        public override void Draw(Graphics graphics)
        {
            using (SolidBrush brush = new SolidBrush(shapeColor))
            {
                graphics.FillEllipse(brush, X, Y, Width, Height);
            }
        }

        // Visual change 1: alternate the fill colour.
        public override void ChangeAppearance()
        {
            if (shapeColor == Color.DeepSkyBlue)
            {
                shapeColor = Color.Orange;
            }
            else
            {
                shapeColor = Color.DeepSkyBlue;
            }
        }
    }
}