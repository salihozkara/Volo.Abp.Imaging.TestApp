using ImageMagick;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Image.Abstractions.Volo.Abp.Image;

namespace Volo.Abp.Image.Magick.Volo.Abp.Imaging;

public class MagickImageCompressor : IImageCompressor, ITransientDependency
{
    private readonly ImageOptimizer _optimizer = new();

    public async Task<Stream> CompressAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        stream = await stream.ConvertToWritableStreamAsync(true, cancellationToken);
        _optimizer.IsSupported(stream);
        stream.Position = 0;
        _optimizer.Compress(stream);
        stream.Position = 0;
        return stream;
    }

    public bool CanCompress(IImageFormat? imageFormat)
    {
        return imageFormat?.MimeType switch
        {
            "image/jpeg" => true,
            "image/png" => true,
            "image/gif" => true,
            "image/bmp" => true,
            "image/tiff" => true,
            _ => false
        };
    }
}