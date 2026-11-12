using System.Drawing;

namespace BouncingShapesAssignment
{
    public class PolygonShape : Shape
    {
        private bool thickOutline = false;

        public PolygonShape(int x, int y)
            : base(x, y, 70, 60)
        {
            shapeColor = Color.MediumPurple;
        }

        public override void Draw(Graphics graphics)
        {
            // Each point is positioned relative to X and Y.
            // This creates an irregular arrow shape.
            Point[] points = new Point[]
            {
                new Point(X + 5, Y + 18),
                new Point(X + 38, Y + 18),
                new Point(X + 38, Y + 5),
                new Point(X + 65, Y + 30),
                new Point(X + 38, Y + 55),
                new Point(X + 38, Y + 42),
                new Point(X + 5, Y + 42)
            };

            using (SolidBrush brush = new SolidBrush(shapeColor))
            {
                graphics.FillPolygon(brush, points);
            }

            int outlineWidth;

            if (thickOutline)
            {
                outlineWidth = 7;
            }
            else
            {
                outlineWidth = 2;
            }

            using (Pen pen = new Pen(Color.White, outlineWidth))
            {
                graphics.DrawPolygon(pen, points);
            }
        }

        // Visual change 3: change the outline thickness.
        public override void ChangeAppearance()
        {
            thickOutline = !thickOutline;
        }
    }
}