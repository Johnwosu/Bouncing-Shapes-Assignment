using System.Drawing;
using System.Drawing.Drawing2D;

namespace BouncingShapesAssignment
{
    public class RectangleShape : Shape
    {
        private bool horizontalGradient = true;

        public RectangleShape(int x, int y)
            : base(x, y, 70, 45)
        {
        }

        public override void Draw(Graphics graphics)
        {
            LinearGradientMode direction;

            if (horizontalGradient)
            {
                direction = LinearGradientMode.Horizontal;
            }
            else
            {
                direction = LinearGradientMode.Vertical;
            }

            using (LinearGradientBrush brush =
                new LinearGradientBrush(
                    Bounds,
                    Color.Yellow,
                    Color.Red,
                    direction))
            {
                graphics.FillRectangle(brush, Bounds);
            }
        }

        // Visual change 2: switch the gradient direction.
        public override void ChangeAppearance()
        {
            horizontalGradient = !horizontalGradient;
        }
    }
}