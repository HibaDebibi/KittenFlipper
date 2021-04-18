using System.Threading.Tasks;

namespace KittenFlipper.Contracts
{
    public interface IKittenFlipperService
    {
        Task<byte[]> RotateCatAsync(int rotationType);
    }
}
