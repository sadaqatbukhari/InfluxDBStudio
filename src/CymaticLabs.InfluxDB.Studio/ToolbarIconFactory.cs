using System.Drawing;
using System.Drawing.Drawing2D;

namespace CymaticLabs.InfluxDB.Studio
{
    /// <summary>Creates small Visual Studio-style command icons for the query toolbar.</summary>
    internal static class ToolbarIconFactory
    {
        public static Bitmap CreateCommentSelectionIcon(bool uncomment)
        {
            var image = new Bitmap(20, 20);
            using var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var codePen = new Pen(Color.FromArgb(65, 83, 105), 1.7f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            graphics.DrawLine(codePen, 9, 4, 18, 4);
            graphics.DrawLine(codePen, 9, 8, 16, 8);
            graphics.DrawLine(codePen, 9, 12, 18, 12);
            graphics.DrawLine(codePen, 9, 16, 15, 16);

            using var commentPen = new Pen(Color.FromArgb(0, 122, 204), 2.0f)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            graphics.DrawLine(commentPen, 4, 3, 2, 10);
            graphics.DrawLine(commentPen, 8, 3, 6, 10);

            if (uncomment)
            {
                using var removePen = new Pen(Color.FromArgb(209, 52, 56), 2.0f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                };
                graphics.DrawLine(removePen, 2, 13, 7, 18);
                graphics.DrawLine(removePen, 7, 13, 2, 18);
            }

            return image;
        }
    }
}
