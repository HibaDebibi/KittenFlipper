
using System.Drawing;

namespace KittenFlipper.Contracts
{
    public interface IImageHandler
    {
        void RotateAndFlip(Image image, int rotationType = 1);
    }
}
