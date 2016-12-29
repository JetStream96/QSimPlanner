using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class ImageUtil
    {
        // Resize the image using highest quality interpolation mode.
        public static Bitmap Resize(Image image, Size newSize)
        {
            var newImage = new Bitmap(newSize.Width, newSize.Height);

            using (var g = Graphics.FromImage(newImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;//.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(image, new Rectangle(new Point(0, 0), newSize));
                return newImage;
            }
        }

        public static void SetImageHighQuality(this PictureBox picBox, Image image)
        {
            var newImage = Resize(image, picBox.Size);
            picBox.Image = newImage;
        }
    }
}