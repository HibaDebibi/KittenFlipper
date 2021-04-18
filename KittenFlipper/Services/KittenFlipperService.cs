using System;
using System.Drawing;
using System.Threading.Tasks;
using KittenFlipper.Contracts;

namespace KittenFlipper.Services
{
    public class KittenFlipperService : IKittenFlipperService
    {
        private readonly IImageHandler _imageHandler;
        private readonly IImageLoader _imageLoader;

        public KittenFlipperService(IImageHandler imageHandler, IImageLoader imageLoeader)
        {
            this._imageHandler = imageHandler;
            this._imageLoader = imageLoeader;
        }

        /// <summary>
        /// gets the image from caataas.com/cat and flip it
        /// </summary>
        /// <param name="rotationType">multiple manipulation types</param>
        /// <returns></returns>
        public async Task<byte[]> RotateCatAsync(int rotationType)
        {
            byte[] imageBytes = new byte[] { };

            Image image = await _imageLoader.LoadImageAsync(new Uri($"cat", UriKind.Relative));
            _imageHandler.RotateAndFlip(image, rotationType);
            //ImageConverter Class convert Image object to Byte array.
            imageBytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));

            return imageBytes;

        }
    }
}

