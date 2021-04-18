using System;
using System.Drawing;
using System.Threading.Tasks;

namespace KittenFlipper.Contracts
{
    public interface IImageLoader
    {
        Task<Image> LoadImageAsync(Uri relativePath);
    }
}
