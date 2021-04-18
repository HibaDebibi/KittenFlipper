using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using KittenFlipper.Contracts;

namespace KittenFlipper.Helpers
{
    public class ImageLoader : IImageLoader
    {

        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLoader" /> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        public ImageLoader(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Requests the http client to get a stream.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <returns>
        /// The response the image stream if exists.
        /// </returns>
        public async Task<Image> LoadImageAsync(Uri relativePath)
        {
            using (var httpClient = this.httpClientFactory.CreateClient(Constants.Constants.HttpClient))
            {
                var stream = await httpClient.GetStreamAsync(relativePath);
                return Image.FromStream(stream);
            }
        }
    }
}