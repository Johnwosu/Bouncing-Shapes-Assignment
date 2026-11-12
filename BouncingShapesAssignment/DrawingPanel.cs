using System.Windows.Forms;

namespace BouncingShapesAssignment
{
    // Double buffering reduces flickering during animation.
    public class DrawingPanel : Panel
    {
        public DrawingPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }
    }
}