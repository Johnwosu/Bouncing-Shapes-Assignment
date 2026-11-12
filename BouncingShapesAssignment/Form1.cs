using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace BouncingShapesAssignment
{
    public partial class Form1 : Form
    {
        private DrawingPanel drawingPanel;
        private FlowLayoutPanel buttonPanel;

        private ComboBox shapeSelector;
        private Button addButton;
        private Button pauseButton;
        private Button clearButton;
        private Button exitButton;

        private Timer animationTimer;

        // The rubric explicitly requires an ArrayList.
        private ArrayList shapes = new ArrayList();

        private Random random = new Random();
        private int appearanceTicks = 0;

        public Form1()
        {
            InitializeComponent();

            Text = "Bouncing Shapes Assignment";
            ClientSize = new Size(1000, 650);
            MinimumSize = new Size(650, 450);
            StartPosition = FormStartPosition.CenterScreen;

            CreateControls();

            // Put the timer in the form's component container.
            if (components == null)
            {
                components =
                    new System.ComponentModel.Container();
            }

            animationTimer = new Timer(components);
            animationTimer.Interval = 30;
            animationTimer.Tick += AnimationTimer_Tick;

            Shown += Form1_Shown;
            FormClosed += Form1_FormClosed;
        }

        private void CreateControls()
        {
            drawingPanel = new DrawingPanel();
            drawingPanel.Dock = DockStyle.Fill;
            drawingPanel.BackColor = Color.FromArgb(25, 25, 40);

            drawingPanel.Paint += DrawingPanel_Paint;
            drawingPanel.MouseClick += DrawingPanel_MouseClick;
            drawingPanel.Resize += DrawingPanel_Resize;

            buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Top;
            buttonPanel.Height = 50;
            buttonPanel.Padding = new Padding(8);
            buttonPanel.WrapContents = false;

            shapeSelector = new ComboBox();
            shapeSelector.DropDownStyle =
                ComboBoxStyle.DropDownList;
            shapeSelector.Width = 140;

            shapeSelector.Items.Add("Circle");
            shapeSelector.Items.Add("Rectangle");
            shapeSelector.Items.Add("Polygon");
            shapeSelector.Items.Add("PictureBox");
            shapeSelector.SelectedIndex = 0;

            addButton = new Button();
            addButton.Text = "Add Shape";
            addButton.Width = 100;
            addButton.Click += AddButton_Click;

            pauseButton = new Button();
            pauseButton.Text = "Pause";
            pauseButton.Width = 90;
            pauseButton.Click += PauseButton_Click;

            clearButton = new Button();
            clearButton.Text = "Clear";
            clearButton.Width = 90;
            clearButton.Click += ClearButton_Click;

            exitButton = new Button();
            exitButton.Text = "Exit";
            exitButton.Width = 90;
            exitButton.Click += ExitButton_Click;

            buttonPanel.Controls.Add(shapeSelector);
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(pauseButton);
            buttonPanel.Controls.Add(clearButton);
            buttonPanel.Controls.Add(exitButton);

            Controls.Add(drawingPanel);
            Controls.Add(buttonPanel);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // Begin with one example of each shape.
            AddShape(0, 40, 70);
            AddShape(1, 190, 150);
            AddShape(2, 340, 230);
            AddShape(3, 490, 310);

            animationTimer.Start();
        }

        private void AddShape(int shapeType, int x, int y)
        {
            Shape newShape;

            // Explicit upcasting to the shared base class.
            switch (shapeType)
            {
                case 0:
                    newShape = (Shape)new CircleShape(x, y);
                    break;

                case 1:
                    newShape = (Shape)new RectangleShape(x, y);
                    break;

                case 2:
                    newShape = (Shape)new PolygonShape(x, y);
                    break;

                case 3:
                    newShape = (Shape)new PictureShape(
                        x, y, drawingPanel);
                    break;

                default:
                    return;
            }

            newShape.CheckWalls(
                drawingPanel.ClientSize.Width,
                drawingPanel.ClientSize.Height);

            // Try other positions if the new shape overlaps.
            int attempts = 0;

            while (OverlapsExistingShape(newShape) &&
                   attempts < 100)
            {
                newShape.X = random.Next(
                    Math.Max(
                        1,
                        drawingPanel.ClientSize.Width
                        - newShape.Width + 1));

                newShape.Y = random.Next(
                    Math.Max(
                        1,
                        drawingPanel.ClientSize.Height
                        - newShape.Height + 1));

                attempts++;
            }

            if (OverlapsExistingShape(newShape))
            {
                newShape.Remove();

                MessageBox.Show(
                    "There is not enough free space. " +
                    "Clear some shapes or enlarge the window.");

                return;
            }

            // ArrayList stores objects, so this cast is explicit.
            shapes.Add((object)newShape);

            newShape.UpdateControl();
            drawingPanel.Invalidate();
        }

        private bool OverlapsExistingShape(Shape newShape)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape existingShape =
                    (Shape)shapes[i];

                if (newShape.Bounds.IntersectsWith(
                    existingShape.Bounds))
                {
                    return true;
                }
            }

            return false;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            int x = random.Next(
                Math.Max(1, drawingPanel.ClientSize.Width - 80));

            int y = random.Next(
                Math.Max(1, drawingPanel.ClientSize.Height - 80));

            AddShape(shapeSelector.SelectedIndex, x, y);
        }

        private void DrawingPanel_MouseClick(
            object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Mouse coordinates are relative to this panel.
                AddShape(shapeSelector.SelectedIndex, e.X, e.Y);
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            appearanceTicks++;

            // Move all shapes before checking their collisions.
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = (Shape)shapes[i];

                shape.Move();

                shape.CheckWalls(
                    drawingPanel.ClientSize.Width,
                    drawingPanel.ClientSize.Height);

                // Change appearance roughly every 750 ms.
                if (appearanceTicks >= 25)
                {
                    // Late binding calls the derived version.
                    shape.ChangeAppearance();
                }
            }

            // Compare each pair once.
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape firstShape = (Shape)shapes[i];

                for (int j = i + 1; j < shapes.Count; j++)
                {
                    Shape secondShape =
                        (Shape)shapes[j];

                    firstShape.CheckCollision(secondShape);
                }
            }

            // Collision separation may push a shape toward a wall.
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = (Shape)shapes[i];

                shape.CheckWalls(
                    drawingPanel.ClientSize.Width,
                    drawingPanel.ClientSize.Height);

                shape.UpdateControl();
            }

            if (appearanceTicks >= 25)
            {
                appearanceTicks = 0;
            }

            drawingPanel.Invalidate();
        }

        private void DrawingPanel_Paint(
            object sender, PaintEventArgs e)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = (Shape)shapes[i];

                // Late binding chooses the correct Draw method.
                shape.Draw(e.Graphics);
            }
        }

        private void DrawingPanel_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = (Shape)shapes[i];

                shape.CheckWalls(
                    drawingPanel.ClientSize.Width,
                    drawingPanel.ClientSize.Height);

                shape.UpdateControl();
            }

            drawingPanel.Invalidate();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (animationTimer.Enabled)
            {
                animationTimer.Stop();
                pauseButton.Text = "Resume";
            }
            else
            {
                animationTimer.Start();
                pauseButton.Text = "Pause";
            }
        }

        private void ClearShapes()
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = (Shape)shapes[i];
                shape.Remove();
            }

            shapes.Clear();
            appearanceTicks = 0;

            drawingPanel.Invalidate();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearShapes();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosed(
            object sender, FormClosedEventArgs e)
        {
            animationTimer.Stop();
            ClearShapes();
        }
    }
}