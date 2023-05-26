using ImageMagick;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Image.Abstractions.Volo.Abp.Image;

namespace Volo.Abp.Image.Magick.Volo.Abp.Imaging;

public class MagickImageResizer : IImageResizer, ITransientDependency
{
    public async Task<Stream> ResizeAsync(Stream stream, IImageResizeParameter resizeParameter, CancellationToken cancellationToken = default)
    {
        try
        {
            stream = await stream.ConvertToWritableStreamAsync(cancellationToken: cancellationToken);
            using var image = new MagickImage(stream);
            await ApplyModeAsync(image, resizeParameter, cancellationToken);
            stream.Position = 0;
            await image.WriteAsync(stream, cancellationToken);
            stream.Position = 0;
        }
        catch
        {
            // ignored
        }

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
    
    private Task ApplyModeAsync(IMagickImage image, IImageResizeParameter resizeParameter, CancellationToken cancellationToken = default)
    {
        var width = resizeParameter.Width ?? image.Width;
        var height = resizeParameter.Height ?? image.Height;
        var defaultMagickGeometry = new MagickGeometry(width, height);
        var imageRatio = image.Height / (float)image.Width;
        var percentHeight = MathF.Abs(height / (float)image.Height);
        var percentWidth = MathF.Abs(width / (float)image.Width);
        var ratio = height / (float)width;
        var newWidth = width;
        var newHeight = height;
        switch (resizeParameter.Mode)
        {
            case ImageResizeMode.Stretch:
                defaultMagickGeometry.IgnoreAspectRatio = true;
                image.Resize(defaultMagickGeometry);
                break;
            case ImageResizeMode.Pad:
                if (percentHeight < percentWidth)
                {
                    newWidth = (int)MathF.Round(image.Width * percentHeight);
                }
                else
                {
                    newHeight = (int)MathF.Round(image.Height * percentWidth);
                }
                defaultMagickGeometry.IgnoreAspectRatio = true;
                image.Resize(newWidth, newHeight);
                image.Extent(width, height, Gravity.Center);
                break;
            case ImageResizeMode.BoxPad:
                int boxPadWidth = width > 0 ? width : (int)MathF.Round(image.Width * percentHeight);
                int boxPadHeight = height > 0 ? height : (int)MathF.Round(image.Height * percentWidth);

                if (image.Width < boxPadWidth && image.Height < boxPadHeight)
                {
                    newWidth = boxPadWidth;
                    newHeight = boxPadHeight;
                }
                image.Resize(newWidth, newHeight);
                image.Extent(defaultMagickGeometry, Gravity.Center);
                break;
            case ImageResizeMode.Max:

                if (imageRatio < ratio)
                {
                    newHeight = (int)(image.Height * percentWidth);
                }
                else
                {
                    newWidth = (int)(image.Width * percentHeight);
                }
                image.Resize(newWidth, newHeight);
                break;
            case ImageResizeMode.Min:
                if(width > image.Width || height > image.Height)
                {
                    newWidth = image.Width;
                    newHeight = image.Height;
                }
                else
                {
                    int widthDiff = image.Width - width;
                    int heightDiff = image.Height - height;
                    
                    if (widthDiff > heightDiff)
                    {
                        newWidth = (int)MathF.Round(height / imageRatio);
                    }
                    else if (widthDiff < heightDiff)
                    {
                        newHeight = (int)MathF.Round(width * imageRatio);
                    }
                    else
                    {
                        if(height > width)
                        {
                            newHeight = (int)MathF.Round(image.Height * percentWidth);
                        }
                        else
                        {
                            newHeight = (int)MathF.Round(image.Height * percentWidth);
                        }
                    }
                }
                
                image.Resize(newWidth, newHeight);
                break;
            case ImageResizeMode.Crop:
                defaultMagickGeometry.IgnoreAspectRatio = true;
                image.Crop(width, height, Gravity.Center);
                image.Resize(defaultMagickGeometry);
                break;
            case ImageResizeMode.Distort:
                // image.Distort(DistortMethod.ScaleRotateTranslate, 0, 0, 0, 0, width, 0, 0, height, 0);
                // or
                image.Distort(DistortMethod.ScaleRotateTranslate,width, height);
                break;
            case ImageResizeMode.Fill:
                defaultMagickGeometry.IgnoreAspectRatio = true;
                defaultMagickGeometry.FillArea = true;
                image.Resize(defaultMagickGeometry);
                break;
            default:
                throw new NotSupportedException($"{resizeParameter.Mode} mode is not supported!");
        }
        return Task.CompletedTask;
    }
}