using System;
using System.Drawing;

namespace BouncingShapesAssignment
{
    // All moving shapes inherit from this class.
    public abstract class Shape
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public int SpeedX { get; set; }
        public int SpeedY { get; set; }

        protected Color shapeColor;

        // One Random object is shared by all shapes.
        protected static Random random = new Random();

        public Shape(int x, int y, int width, int height)
        {
            X = x;
            Y = y;

            Width = width;
            Height = height;

            // Start with a non-zero speed in each direction.
            SpeedX = random.Next(2, 5);
            SpeedY = random.Next(2, 5);

            if (random.Next(2) == 0)
            {
                SpeedX = -SpeedX;
            }

            if (random.Next(2) == 0)
            {
                SpeedY = -SpeedY;
            }

            shapeColor = Color.CornflowerBlue;
        }

        // The rectangle surrounding this shape.
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(X, Y, Width, Height);
            }
        }

        public void Move()
        {
            X += SpeedX;
            Y += SpeedY;
        }

        public void CheckWalls(int panelWidth, int panelHeight)
        {
            // The largest valid top-left coordinates.
            int maximumX = Math.Max(0, panelWidth - Width);
            int maximumY = Math.Max(0, panelHeight - Height);

            // Left and right walls.
            if (X < 0)
            {
                X = 0;
                SpeedX = Math.Abs(SpeedX);
            }
            else if (X > maximumX)
            {
                X = maximumX;
                SpeedX = -Math.Abs(SpeedX);
            }

            // Top and bottom walls.
            if (Y < 0)
            {
                Y = 0;
                SpeedY = Math.Abs(SpeedY);
            }
            else if (Y > maximumY)
            {
                Y = maximumY;
                SpeedY = -Math.Abs(SpeedY);
            }
        }

        public void CheckCollision(Shape other)
        {
            if (!Bounds.IntersectsWith(other.Bounds))
            {
                return;
            }

            Rectangle overlap =
                Rectangle.Intersect(Bounds, other.Bounds);

            // Separate the shapes along the smaller overlap.
            if (overlap.Width < overlap.Height)
            {
                int distance = (overlap.Width + 1) / 2;

                if (X + Width / 2 < other.X + other.Width / 2)
                {
                    X -= distance;
                    other.X += distance;

                    SpeedX = -Math.Abs(SpeedX);
                    other.SpeedX = Math.Abs(other.SpeedX);
                }
                else
                {
                    X += distance;
                    other.X -= distance;

                    SpeedX = Math.Abs(SpeedX);
                    other.SpeedX = -Math.Abs(other.SpeedX);
                }
            }
            else
            {
                int distance = (overlap.Height + 1) / 2;

                if (Y + Height / 2 < other.Y + other.Height / 2)
                {
                    Y -= distance;
                    other.Y += distance;

                    SpeedY = -Math.Abs(SpeedY);
                    other.SpeedY = Math.Abs(other.SpeedY);
                }
                else
                {
                    Y += distance;
                    other.Y -= distance;

                    SpeedY = Math.Abs(SpeedY);
                    other.SpeedY = -Math.Abs(other.SpeedY);
                }
            }
        }

        // Derived classes provide their own implementations.
        public abstract void Draw(Graphics graphics);

        public abstract void ChangeAppearance();

        // PictureShape overrides this to move its control.
        public virtual void UpdateControl()
        {
        }

        // PictureShape overrides this to release its resources.
        public virtual void Remove()
        {
        }
    }
}