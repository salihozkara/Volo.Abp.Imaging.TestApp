using Volo.Abp.DependencyInjection;
using Volo.Abp.Image.Abstractions.Volo.Abp.Image;

namespace Volo.Abp.Image.ImageSharp.Volo.Abp.Imaging;

public class ImageSharpResizer : IImageResizer, ITransientDependency
{
    
    public async Task<Stream> ResizeAsync(Stream stream, IImageResizeParameter resizeParameter, CancellationToken cancellationToken = default)
    {
        stream = await stream.ConvertToWritableStreamAsync(cancellationToken: cancellationToken);
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(stream, cancellationToken);
        await ApplyModeAsync(image, resizeParameter, cancellationToken);
        stream.Position = 0;
        await image.SaveAsync(stream, image.Metadata.DecodedImageFormat!, cancellationToken: cancellationToken);
        return stream;
    }

    public bool CanResize(IImageFormat? imageFormat)
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

    private Task ApplyModeAsync(SixLabors.ImageSharp.Image image, IImageResizeParameter resizeParameter,
        CancellationToken cancellationToken = default)
    {
        var width = resizeParameter.Width ?? image.Width;
        var height = resizeParameter.Height ?? image.Height;
        
        var defaultResizeOptions = new ResizeOptions
        {
            Size = new Size(width, height),
        };

        switch (resizeParameter.Mode)
        {
            case ImageResizeMode.Stretch:
                defaultResizeOptions.Mode = ResizeMode.Stretch;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.BoxPad:
                defaultResizeOptions.Mode = ResizeMode.BoxPad;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Min:
                defaultResizeOptions.Mode = ResizeMode.Min;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Max:
                defaultResizeOptions.Mode = ResizeMode.Max;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Crop:
                defaultResizeOptions.Mode = ResizeMode.Crop;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Pad:
                defaultResizeOptions.Mode = ResizeMode.Pad;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case ImageResizeMode.Fill:
                defaultResizeOptions.Mode = ResizeMode.Stretch;
                // defaultResizeOptions.Position = AnchorPositionMode.Center;
                image.Mutate(x => x.Resize(defaultResizeOptions));
                break;
            case null:
            case ImageResizeMode.Distort:
            default:
                throw new NotSupportedException($"{resizeParameter.Mode} mode is not supported!");
        }
        
        return Task.CompletedTask;
    }
}