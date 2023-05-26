using Volo.Abp.DependencyInjection;
using Volo.Abp.Image.Abstractions.Volo.Abp.Image;

namespace Volo.Abp.Image.ImageSharp.Volo.Abp.Imaging;

public class ImageSharpFormatDetector : IImageFormatDetector, ITransientDependency
{
    public virtual IImageFormat FindFormat(Stream stream)
    {
        var format = SixLabors.ImageSharp.Image.DetectFormat(stream);
        return new ImageFormat(format.Name, format.DefaultMimeType);
    }
}