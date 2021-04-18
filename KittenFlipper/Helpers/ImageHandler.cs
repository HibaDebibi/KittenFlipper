using System.Drawing;
using KittenFlipper.Contracts;

namespace KittenFlipper.Helpers
{
    public class ImageHandler : IImageHandler
    {
        public ImageHandler()
        {
        }

        /// <summary>
        /// Rotates or flip image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="rotationType"></param>
        public void RotateAndFlip(Image image, int rotationType = 1)
        {
            if (rotationType == 1)
            {
                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if (rotationType == 2)
            {
                image.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            else if (rotationType == 3)
            {
                image.RotateFlip(RotateFlipType.Rotate180FlipXY);
            }
            else if (rotationType == 4)
            {
                image.RotateFlip(RotateFlipType.Rotate180FlipY);
            }
            else if (rotationType == 5)
            {
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            else if (rotationType == 6)
            {
                image.RotateFlip(RotateFlipType.Rotate270FlipX);
            }
            else if (rotationType == 7)
            {
                image.RotateFlip(RotateFlipType.Rotate270FlipXY);
            }
            else if (rotationType == 8)
            {
                image.RotateFlip(RotateFlipType.Rotate270FlipY);
            }
            else if (rotationType == 9)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (rotationType == 10)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipX);
            }
            else if (rotationType == 11)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipXY);
            }
            else if (rotationType == 12)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipY);
            }
            else if (rotationType == 13)
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
            }
            else if (rotationType == 14)
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            else if (rotationType == 15)
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            }
            else if (rotationType == 16)
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
        }
    }
}