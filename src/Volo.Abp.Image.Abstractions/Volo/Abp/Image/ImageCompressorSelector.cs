using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public class ImageCompressorSelector : IImageCompressorSelector, ITransientDependency
{
    private readonly IEnumerable<IImageCompressor> _imageCompressors;

    public ImageCompressorSelector(IEnumerable<IImageCompressor> imageCompressors)
    {
        _imageCompressors = imageCompressors;
    }

    public IImageCompressor? FindCompressor(IImageFormat? imageFormat)
    {
        return _imageCompressors.FirstOrDefault(x => x.CanCompress(imageFormat));
    }
}