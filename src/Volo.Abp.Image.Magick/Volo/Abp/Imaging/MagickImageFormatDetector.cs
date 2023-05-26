using ImageMagick;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Image.Abstractions.Volo.Abp.Image;

namespace Volo.Abp.Image.Magick.Volo.Abp.Imaging;

public class MagickImageFormatDetector : IImageFormatDetector, ITransientDependency
{
    public IImageFormat? FindFormat(Stream image)
    {
        using var magickImage = new MagickImage(image);
        var format = magickImage.FormatInfo;
        return format == null ? null : new ImageFormat(format.Format.ToString(), format.MimeType ?? string.Empty);
    }
}