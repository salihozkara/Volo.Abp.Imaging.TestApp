using Volo.Abp.DependencyInjection;
using Volo.Abp.Image.Abstractions.Volo.Abp.Image;

namespace Volo.Abp.Image.ImageSharp.Volo.Abp.Imaging;

public class ImageSharpCompressor : IImageCompressor, ITransientDependency
{
    public async Task<Stream> CompressAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        stream = await stream.ConvertToWritableStreamAsync(cancellationToken: cancellationToken);
        using var sixLaborsImage = await SixLabors.ImageSharp.Image.LoadAsync(stream, cancellationToken);
        sixLaborsImage.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = x.GetCurrentSize(),
            Compand = true,
            Mode = ResizeMode.Max
        }));
        stream.Position = 0;
        await sixLaborsImage.SaveAsync(stream, sixLaborsImage.Metadata.DecodedImageFormat!,
            cancellationToken: cancellationToken);
        return stream;
    }

    public bool CanCompress(IImageFormat? format)
    {
        return format?.MimeType switch
        {
            "image/jpeg" => true,
            "image/png" => true,
            "image/webp" => true,
            _ => false
        };
    }
}