namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public interface IImageFormatDetector
{
    IImageFormat? FindFormat(Stream image);
}