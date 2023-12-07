using System.Drawing;

namespace RadianceOS.Render
{
    public static class Canvas
    {
        public static Cosmos.System.Graphics.Canvas canvas;

        public static void setup(Cosmos.System.Graphics.Canvas canv)
        {

            canvas = canv;

        }

        public static void DrawImageAlpha(Cosmos.System.Graphics.Image image, int x, int y)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = Color.FromArgb(image.RawData[i + j * image.Width]);
                    if (color.A == 0)
                        continue;
                    canvas.DrawPoint(color, x + i, y + j);
                }
            }
        }
    }
}